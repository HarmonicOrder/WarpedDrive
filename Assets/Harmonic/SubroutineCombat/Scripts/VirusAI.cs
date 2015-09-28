using UnityEngine;
using System.Collections;

public class VirusAI : Actor {
	
	public MeshRenderer LockedOnGUI;
	protected Transform targetT;

	protected override void OnStart ()
	{
		ActiveSubroutines.VirusList.Add(this);
		if (LockedOnGUI != null)
		{
			LockedOnGUI.enabled = false;
		}
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
