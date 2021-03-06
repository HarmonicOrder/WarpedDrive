﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class VirusAI : Combatant, ILockTarget, IMalware, ISubroutineListener {
	
	public Transform ExplosionPrefab;
	public MeshRenderer LockedOnGUI;
	public virtual short AttackPriority {get{return 2;}}
	
	public virtual VirusType Type {get{return VirusType.Virus;}}

	protected Transform targetT;
	protected bool IsAggroForced { get; private set; }
    public bool IsImmobile { get; private set; }
    public bool IsSandboxed { get; internal set; }

    public List<GameObject> Emitters { get; set; }

    protected override void OnAwake ()
	{
        Emitters = new List<GameObject>();
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

    internal static void UnparentEmitters(List<GameObject> emitters)
    {
        foreach (GameObject g in emitters)
        {
            if (g != null)
                g.transform.SetParent(null);
        }
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

    public override void DoOnKilled(ICombatant attacker)
    {
        OnVirusDead();
    }

    protected virtual void OnVirusDead()
    {
        ActiveSubroutines.RemoveVirus(this);
	}
    
	protected override void _OnDestroy(){
		ActiveSubroutines.RemoveVirus(this);

        if (ActiveSubroutines.MalwareList.Contains(this))
            print("!!!!! still in list!");

        UnparentEmitters(this.Emitters);

#pragma warning disable CS0252 // Possible unintended reference comparison; left hand side needs cast
        if (CyberspaceDroneInput.CurrentLock == this)
#pragma warning restore CS0252 // Possible unintended reference comparison; left hand side needs cast
            CyberspaceDroneInput.CurrentLock = null;
	}

    internal static void AfterInstantiateVirus(Transform newVirus, Transform parent)
    {
        newVirus.SetParent(parent);

        VirusAI newV = newVirus.GetComponent<VirusAI>();
        if (newV != null)
        {
            newV.DoOnAwake();
            if (ActiveSubroutines.List.Count > 0)
            {
                newV.OnSubroutineActive(null);
            }
        }
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
