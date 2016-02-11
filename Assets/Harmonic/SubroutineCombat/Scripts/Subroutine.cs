﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Subroutine : Actor {

	public SubroutineFunction Function {get;set;}
	public SubroutineMovement Movement {get;set;}
	public Transform ExplosionPrefab;
	public Transform FunctionRoot;
    public Transform HealthBar;
    public Transform HealthPipPrefab;
    public bool IsInvulnerable { get; set; }

	public SubroutineStatus Status { 
		set
		{
			this.OnSubroutineActive += value.OnSubroutineActive;
			this.OnSubroutineDead += value.OnSubroutineDead;
			this.OnSubroutineTakeDamage += value.OnSubroutineTakeDamage;
		}
	}

	public delegate void StatusChange();
	public delegate void DamageReport(float currentHitpoints, float maxHitpoints);
	public event StatusChange OnSubroutineActive;
	public event StatusChange OnSubroutineDead;
	public event DamageReport OnSubroutineTakeDamage;

	private Transform _lockedTarget;
	public Transform LockedTarget {
		get{
            if (_lockedTarget == null)
            {
                FindClosestMalware();
            }
			return _lockedTarget;
		}
		set{
            if (value != null)
            {
                lockedMalware = value.GetComponent<IMalware>();

                if (lockedMalware is VirusAI)
                    lockedVirus = lockedMalware as VirusAI;
                else
                    lockedVirus = null;
            }
            else
            {
                lockedMalware = null;
                lockedVirus = null;
            }

			_lockedTarget = value;
		}
    }
    public IMalware lockedMalware;
    public VirusAI lockedVirus;

    public bool IsActive {get;set;}
    public SubroutineInfo SInfo { get; set; }
	public Transform StartingPosition {get;set;}

    private Machine DeployedMachine { get; set; }
    private Coroutine Raycasting;

	protected override void OnAwake()
    {
        this.Info = new ActorInfo()
        {
            Name = "Subroutine",
            MaxHitPoints = 10f,
            HitPoints = 10f,
            FireRate = 1f,
            DamagePerHit = 2f
        };
    }

	protected override void OnStart(){
	}

	public void Activate(SubroutineInfo si)
    {
        if (this._lockedTarget != null)
        {
            this.DeployedMachine = CyberspaceBattlefield.Current.FindByName(this._lockedTarget.root.name);
            this.DeployedMachine.OnSystemClean += OnMachineClean;
        }
        if (!this.IsActive)
        {
            this.Movement = this.GetComponent<SubroutineMovement>();
            this.Movement.Parent = this;

            this.Function = this.GetComponent<SubroutineFunction>();
            this.Function.Parent = this;

            this.StartingPosition = this.transform.parent;


            SInfo = si;

            this.IsActive = true;
			ActiveSubroutines.Add(this);
			this.Movement.Fire();
			if (this.OnSubroutineActive != null)
				this.OnSubroutineActive();
            if (this.Movement is Tracer)
                Raycasting = StartCoroutine(RaycastForward());
		}

        RefreshHealthDisplay();
    }

	public void Deactivate()
	{
		this.IsActive = false;
		ActiveSubroutines.Remove(this);

        if (Raycasting != null)
            StopCoroutine(Raycasting);
	}

	public void TakeDamage(float damage)
	{
        if (this.IsInvulnerable)
            return;

		this.Info.HitPoints -= damage;

		if (this.Info.HitPoints < 0f)
		{
            Die();
		} else {
			if (this.OnSubroutineTakeDamage != null)
				this.OnSubroutineTakeDamage(this.Info.HitPoints, this.Info.MaxHitPoints);
		}

        RefreshHealthDisplay();
    }

    /// <summary>
    /// public facing "this subroutine should explode" method
    /// </summary>
    public void Die()
    {
        GameObject.Instantiate(ExplosionPrefab, this.transform.position, Quaternion.identity);
        DoOnDeath();
    }

    /// <summary>
    /// internal "do these things when subroutines die" method
    /// </summary>
    private void DoOnDeath()
    {
        this.Deactivate();
        if (this.OnSubroutineDead != null)
            this.OnSubroutineDead();

        CyberspaceBattlefield.Current.ReclaimCores((int)this.SInfo.CoreCost);

        GameObject.Destroy(this.gameObject);
    }

    protected override void _OnDestroy()
    {
        if (this.DeployedMachine != null)
        {
            this.DeployedMachine.OnSystemClean -= OnMachineClean;
        }
    }

    private void OnMachineClean()
    {
        Die();
    }

    private void RefreshHealthDisplay()
    {
        if (this.HealthBar != null)
        {
            foreach(Transform t in this.HealthBar)
            {
                GameObject.Destroy(t.gameObject);
            }

            for (int i = 0; i < this.Info.HitPoints; i++)
            {
                Transform t = GameObject.Instantiate<Transform>(this.HealthPipPrefab);
                t.SetParent(this.HealthBar);
                t.localPosition = Vector3.zero;
                t.Rotate(Vector3.forward, (i - ((this.Info.HitPoints - 1) / 2))*15);
                //t.localScale = Vector3.one * 5f;
            }
        }
    }

    private IEnumerator RaycastForward()
    {
        while(this.IsActive)
        {
            RaycastHit rayHit;
            if (Physics.Raycast(this.transform.position, this.transform.forward, out rayHit, Mathf.Infinity, 1 << LayerMask.NameToLayer("Default")))
            {
                if (rayHit.collider != null)
                {
                    //always prints false, which is good
                    //print(rayHit.collider.gameObject == this.gameObject);
                    if (rayHit.collider.GetComponent<Tracer>() != null)
                    {
                        if (this.Movement is Tracer)
                        {
                            (this.Movement as Tracer).MoveToClearFiringLine = true;
                        }
                    }
                }
                else if (this.Movement is Tracer)
                {
                    (this.Movement as Tracer).MoveToClearFiringLine = false;
                }
            }

            yield return null;
            yield return null;
        }
    }


    protected void FindClosestMalware()
    {
        if (ActiveSubroutines.MalwareList.Count == 0)
        {
            this.lockedMalware = null;
            this.lockedVirus = null;
            this._lockedTarget = null;
            return;
        }

        float range = 500;
        //comparing range squared vs magnitude squared is a performance enhancement
        //it eliminates the expensive square root calculation
        float closest = range * range;
        foreach (IMalware mal in ActiveSubroutines.MalwareList)
        {
            float dist = (mal.transform.position - this.transform.position).sqrMagnitude / mal.AttackPriority;
            //if this has a higher priority than now
            //and the distance is closer
            if (dist < closest)
            {
                if (!this.Function.OnlyTrackActiveViruses || (mal is VirusAI))
                {
                    _lockedTarget = mal.transform;
                    this.lockedMalware = mal;

                    if (mal is VirusAI)
                        this.lockedVirus = mal as VirusAI;

                    closest = dist;
                }
            }
        }
    }
}
