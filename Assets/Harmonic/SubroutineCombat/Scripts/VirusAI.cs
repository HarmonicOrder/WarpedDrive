using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VirusAI : Actor, ILockTarget, IMalware, ISubroutineListener {
	
	public Transform ExplosionPrefab;
	public MeshRenderer LockedOnGUI;
	public virtual short AttackPriority {get{return 2;}}
	
	public virtual VirusType Type {get{return VirusType.Virus;}}

	protected Transform targetT;
	protected bool IsAggroForced { get; private set; }
    public bool IsImmobile { get; private set; }
    public bool IsSandboxed { get; internal set; }

    protected override void OnAwake ()
	{
        DoOnAwake();
	}

    //see octopus spawner to see why
    //see why it's virtual in stealth virus
    public virtual void DoOnAwake()
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

    /// <summary>
    /// Subroutine parameter not guaranteed to be non null
    /// </summary>
    /// <param name="sub"></param>
	public void OnSubroutineActive(Subroutine sub)
	{
        //if you aren't forced, then get the target
        if (!this.IsAggroForced)
            TargetClosestActiveSubroutine();
    }
	
	public void OnSubroutineInactive(Subroutine sub)
	{
        //keep aggro'd if you have a target, you're forced, and the inactive subroutine isn't the one you're targeting
        if ((this.targetT != null) && this.IsAggroForced && (sub.transform != this.targetT))
            return;

        TargetClosestActiveSubroutine();
	}

    public void TargetClosestActiveSubroutine()
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
        this.IsAggroForced = true;
    }

    public void UnforceAggro()
    {
        this.targetT = null;
        this.IsAggroForced = false;

        TargetClosestActiveSubroutine();
    }

    public void ForceImmobilization()
    {
        this.IsImmobile = true;
        OnImmobilized();
    }

    public void UnforceImmobilization()
    {
        this.IsImmobile = false;
        OnMobilized();
    }

    protected virtual void OnImmobilized() { }
    protected virtual void OnMobilized() { }

    public enum VirusType { Virus, Ransomware, Wabbit, Bomb, Tank, Spawner, Trojan, Stealth }
    public static string GetPluralVirusType(VirusType v)
    {
        if (v == VirusType.Virus)
        {
            return "Viruses";
        }

        return v.ToString() + "s";
    }
}
