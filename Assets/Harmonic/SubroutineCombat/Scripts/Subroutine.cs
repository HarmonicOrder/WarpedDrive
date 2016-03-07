using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;

public class Subroutine : Combatant {

	public SubroutineFunction Function {get;set;}
	public SubroutineMovement Movement {get;set;}
	public Transform ExplosionPrefab;
	public Transform FunctionRoot;
    public Transform HealthBar;
    public Transform HealthPipPrefab;

    [Obsolete]
    public delegate void StatusChange();
    [Obsolete]
    public delegate void DamageReport(float currentHitpoints, float maxHitpoints);

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
    public SubroutineActorInfo MyActorInfo { get { return Info as SubroutineActorInfo;  } }
	public Transform StartingPosition {get;set;}

    private Machine DeployedMachine { get; set; }
    private Coroutine Raycasting;

	protected override void OnAwake()
    {
        this.Info = new SubroutineActorInfo()
        {
            Name = "Subroutine",
        };
    }

	protected override void OnStart(){
	}

	public void Activate(SubroutineInfo si, Machine m)
    {
        if (m != null)
        {
            this.DeployedMachine = m;
            this.DeployedMachine.OnMachineClean += OnMachineClean;
        }

        if (!this.IsActive)
        {
            this.Movement = this.GetComponent<SubroutineMovement>();
            this.Movement.Parent = this;

            this.Function = this.GetComponent<SubroutineFunction>();
            this.Function.Parent = this;

            this.StartingPosition = this.transform.parent;
            
            SInfo = si;
            
			this.Movement.Fire();

            StartCoroutine(DelayActivation());
		}
    }

    private IEnumerator DelayActivation()
    {
        yield return new WaitForSecondsInterruptTime(SubroutineMovement.TimeToInstantiate);
        FinishActivation();

        if (this.Movement is Tracer)
        {
            Raycasting = StartCoroutine(RaycastForward());
        }
    }

    private void FinishActivation()
    {
        this.IsActive = true;
        ActiveSubroutines.Add(this);
    }

	public void Deactivate()
	{
		this.IsActive = false;
		ActiveSubroutines.Remove(this);

        if (Raycasting != null)
            StopCoroutine(Raycasting);
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

        CyberspaceBattlefield.Current.ReclaimCores((int)this.SInfo.CoreCost);

        GameObject.Destroy(this.gameObject);
    }

    protected override void _OnDestroy()
    {
        if (this.DeployedMachine != null)
        {
            this.DeployedMachine.OnMachineClean -= OnMachineClean;
        }
    }

    private void OnMachineClean()
    {
        this.Die();
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
            if (mal.IsLurking())
                continue;

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
    
    public override void DoOnKilled(ICombatant attacker)
    {
        this.Die();
    }
    
    private void HitAndBlockReadout(StringBuilder sb, float movementAmount, float functionAmount)
    {
        sb.Append(movementAmount.ToString().PadLeft(4, ' '));
        sb.Append("% [");
        sb.Append(this.SInfo.MovementName.ToUpper());
        sb.Append("]\r\n");

        if (functionAmount > 0f)
        {
            sb.Append("+");
            sb.Append(functionAmount.ToString().PadLeft(3, ' '));
            sb.Append("% [");
            sb.Append(this.SInfo.FunctionName.ToUpper());
            sb.Append("]\r\n");
        }
    }

    protected override void FillHitReadout(StringBuilder sb)
    {
        HitAndBlockReadout(sb, MyActorInfo.MovementHitChance, MyActorInfo.FunctionHitChance);
    }

    protected override void FillBlockReadout(StringBuilder sb)
    {
        HitAndBlockReadout(sb, MyActorInfo.MovementBlockChance, MyActorInfo.FunctionBlockChance);
    }
}
