using UnityEngine;
using System.Collections;

public class VirusAI : Actor {
	
	protected Transform targetT;

	protected override void OnStart ()
	{
		ActiveSubroutines.VirusList.Add(this);
		base.OnStart();
	}

	public void OnSubroutineActive(Subroutine sub)
	{
		this.targetT = sub.transform;
	}
	
	public void OnSubroutineInactive(Subroutine sub)
	{
		this.targetT = null;
	}
}
