using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VirusAI : Actor, ILockTarget, IMalware {
	
	public Transform ExplosionPrefab;
	public MeshRenderer LockedOnGUI;
	public virtual short AttackPriority {get{return 2;}}
	
	public virtual string DisplayNameSingular {get{return "Virus";}}
	public virtual string DisplayNamePlural {get{return "Viruses";}}

	protected Transform targetT;
	
	protected override void OnAwake ()
	{
		ActiveSubroutines.AddVirus(this);
	}

	protected override void OnStart ()
	{
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
		ActiveSubroutines.RemoveVirus(this);
	}

	//todo - leftover damage from taking down armor needs to go to health
	public void TakeDamage(float amount)
	{
		if(this.Info.ArmorPoints > 0f)
		{
			this.Info.ArmorPoints -= amount;
		}
		else 
		{
			this.Info.HitPoints -= amount;

			if (this.Info.HitPoints < 0f)
				this.OnVirusDead();
		}
	}

	protected virtual void OnTakeDamage(float damage)
	{
	}
}
