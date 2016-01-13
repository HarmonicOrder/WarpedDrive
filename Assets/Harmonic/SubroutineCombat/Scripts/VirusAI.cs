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
		//todo: remove magic number
		this.targetT = ActiveSubroutines.FindClosestActiveSubroutine(this.transform.position, 300f);
	}
	
	public void OnSubroutineInactive(Subroutine sub)
	{
		//todo: remove magic number
		this.targetT = ActiveSubroutines.FindClosestActiveSubroutine(this.transform.position, 300f);
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

	public void TakeDamage(float amount)
	{
		float armorPointsLost = 0;
		float hitPointsLost = 0;

		if(this.Info.ArmorPoints > 0f)
		{
			if (amount > this.Info.ArmorPoints)
			{
				armorPointsLost = this.Info.ArmorPoints;
				amount -= this.Info.ArmorPoints;
				this.Info.ArmorPoints = 0f;
			}
			else
			{
				armorPointsLost = amount;
				this.Info.ArmorPoints -= amount;
				amount = 0f;
			}
		}

		if (amount > 0f)
		{
			hitPointsLost = amount;
			this.Info.HitPoints -= amount;

			if (this.Info.HitPoints <= 0f)
				this.OnVirusDead();
		}

		this.OnTakeDamage(amount, armorPointsLost, hitPointsLost);
	}

	protected virtual void OnTakeDamage(float damage, float armorPointsLost, float hitPointsLost)
	{
	}

	protected override void _OnDestroy(){
        //print("removing virusAI from virus list");
		ActiveSubroutines.RemoveVirus(this);

        if (CyberspaceDroneInput.CurrentLock == this)
            CyberspaceDroneInput.CurrentLock = null;
	}

    public void ForceAggro(Transform t)
    {
        this.targetT = t;
    }
}
