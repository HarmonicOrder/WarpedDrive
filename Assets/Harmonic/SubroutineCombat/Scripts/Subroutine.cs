using UnityEngine;
using System.Collections;

public class Subroutine : Actor {

	public SubroutineFunction Function {get;set;}
	public SubroutineMovement Movement {get;set;}
	public Transform ExplosionPrefab;
	public Transform FunctionRoot;
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
			return _lockedTarget;
		}
		set{
			_lockedTarget = value;

		}
	}

	public bool IsActive {get;set;}

	public Transform StartingPosition {get;set;}

	protected override void OnAwake(){
		this.Movement = this.GetComponent<SubroutineMovement>();
		this.Movement.Parent = this;
		
		this.Function = this.GetComponent<SubroutineFunction>();
		this.Function.Parent = this;
		
		this.StartingPosition = this.transform.parent;
		
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

	public void Activate()
	{
		if (!this.IsActive)
		{
			this.IsActive = true;
			ActiveSubroutines.Add(this);
			this.Movement.Fire();
			if (this.OnSubroutineActive != null)
				this.OnSubroutineActive();
		}
	}

	public void Deactivate()
	{
		this.IsActive = false;
		ActiveSubroutines.Remove(this);
	}

	public void TakeDamage(float damage)
	{
		this.Info.HitPoints -= damage;

		if (this.Info.HitPoints < 0f)
		{
			GameObject.Instantiate(ExplosionPrefab, this.transform.position, Quaternion.identity);

			this.transform.SetParent(this.StartingPosition);
			this.transform.localPosition = Vector3.zero;
			this.transform.localRotation = Quaternion.Euler(Vector3.forward);
			this.Deactivate();
			if (this.OnSubroutineDead != null)
				this.OnSubroutineDead();
		} else {
			if (this.OnSubroutineTakeDamage != null)
				this.OnSubroutineTakeDamage(this.Info.HitPoints, this.Info.MaxHitPoints);
		}
	}
}
