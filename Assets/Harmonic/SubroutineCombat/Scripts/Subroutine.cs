using UnityEngine;
using System.Collections;

public class Subroutine : Actor {

	public SubroutineFunction Function {get;set;}
	public SubroutineMovement Movement {get;set;}

	private Transform _lockedTarget;
	public Transform LockedTarget {
		get{
			return _lockedTarget;
		}
		set{
			_lockedTarget = value;

		}
	}

	protected override void OnStart(){
		this.Movement = this.GetComponent<SubroutineMovement>();
		this.Movement.Parent = this;

		this.Function = this.GetComponent<SubroutineFunction>();
		this.Function.Parent = this;
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
}
