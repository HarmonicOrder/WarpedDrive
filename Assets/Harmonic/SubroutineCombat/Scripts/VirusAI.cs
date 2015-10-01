using UnityEngine;
using System.Collections;

public class VirusAI : Actor, ILockTarget {
	
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

	
	public void EnableLockedOnGui()
	{
		LockedOnGUI.enabled = true;
	}

	public void DisableLockedOnGui()
	{
		LockedOnGUI.enabled = false;
	}
}
