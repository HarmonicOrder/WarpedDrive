using UnityEngine;
using System.Collections;

public class Subroutine : Actor {

	public SubroutineFunction Function {get;set;}
	public SubroutineMovement Movement {get;set;}

	protected override void OnStart(){
		this.Movement = this.GetComponent<SubroutineMovement>();
		this.Function = this.GetComponent<SubroutineFunction>();
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
