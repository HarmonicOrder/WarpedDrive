using UnityEngine;
using System.Collections;

public class Subroutine : Actor {

	public SubroutineFunction Function {get;set;}
	public SubroutineMovement Movement {get;set;}
	public Transform ExplosionPrefab;

	private Transform _lockedTarget;
	public Transform LockedTarget {
		get{
			return _lockedTarget;
		}
		set{
			_lockedTarget = value;

		}
	}

	private Transform StartingPosition {get;set;}

	protected override void OnStart(){
		this.Movement = this.GetComponent<SubroutineMovement>();
		this.Movement.Parent = this;

		this.Function = this.GetComponent<SubroutineFunction>();
		this.Function.Parent = this;

		this.StartingPosition = this.transform.parent;

		this.Info = new ActorInfo()
		{ 
			Name = "Subroutine",
			HitPoints = 10f
		};
	}

	public void Activate()
	{
		ActiveSubroutines.Add(this);
		this.Movement.Fire();
	}

	public void Deactivate()
	{
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
		}
	}
}
