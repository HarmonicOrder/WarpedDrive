using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
		if (LockedOnGUI != null)
			LockedOnGUI.enabled = false;
	}

	protected virtual void OnVirusDead()
	{
		ActiveSubroutines.VirusList.Remove(this);
	}

	public void TakeDamage(float damage)
	{
		if(this.Info.ArmorPoints > 0f)
		{
			this.Info.ArmorPoints -= damage;
		}
		else 
		{
			this.Info.HitPoints -= damage;

			if (this.Info.HitPoints < 0f)
				this.OnVirusDead();
		}
	}
}
