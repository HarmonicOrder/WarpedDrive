using UnityEngine;
using System.Collections;
using System;

public class Corrupt : SubroutineFunction {
    private GameObject ParticleSource;

    public ImplodingEmitter Emitter { get; private set; }

    // 1/3 of standard 90f range
    public override float TracerSlowRange { get { return 30f; } }
    // 1/4 of the standard 80f range
    public override float TracerStopRange { get { return 20f; } }

    // Use this for initialization
    void Start () {
        this.Parent.Info.Cooldown = .3f;
        this.Parent.Info.HitChance += 2f;
        this.Parent.MyActorInfo.FunctionHitChance = 2f;
        this.Parent.Info.CoreCost += 1;

        this.ParticleSource = this.transform.Find("FunctionRoot/Corrupt/DrainParticles").gameObject;
        this.Emitter = this.transform.Find("FunctionRoot/Corrupt/Emitter").GetComponent<ImplodingEmitter>();
    }
	
	// Update is called once per frame
	void Update () {
	    if (CanAttackEnemy())
        {
            DoAttack();
        }
	}

    private void DoAttack()
    {
        //if we've had a previous target
        if (this.Emitter.isAttracting)
        {
            //and we're attacking the same target
            if (this.Emitter.partSys.transform.parent == this.Parent.lockedMalware.transform)
            {
                ReallyDoAttack();
            }
            else
            {
                //wait for the current particles to get home to do the next attack!
            }
        }
        else //we have no previous target!
        {
            ReallyDoAttack();

            this.Emitter.partSys.transform.SetParent(this.Parent.lockedMalware.transform);
            this.Emitter.partSys.transform.localPosition = Vector3.zero;
            this.Parent.lockedMalware.Emitters.Add(this.Emitter.partSys.gameObject);
            this.Emitter.BeginAttraction();
        }
    }

    private void ReallyDoAttack()
    {
        CooldownRemaining = this.Parent.Info.Cooldown;
        this.Parent.DoAttack(this.Parent.lockedMalware);
    }

    void OnDestroy()
    {
        GameObject.Destroy(this.Emitter.particleSystemObj);
    }
}
