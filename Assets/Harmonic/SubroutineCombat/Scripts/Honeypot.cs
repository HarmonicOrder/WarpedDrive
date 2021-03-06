﻿using UnityEngine;
using System.Collections;

public class Honeypot : SubroutineFunction
{
    public float AngleThreshold = .1f;
    public override bool OnlyTrackActiveViruses { get { return true; } }

    public ParticleSystem PulseParticles;
    //public ParticleSystem BurstParticles;
    public LineRenderer HoneypotLineRenderer;

    // Use this for initialization
    void Start()
    {
        this.HoneypotLineRenderer = this.transform.Find("FunctionRoot/Honeypot").GetComponent<LineRenderer>();
        this.PulseParticles = this.transform.Find("FunctionRoot/Honeypot/PulseParticles").GetComponent<ParticleSystem>();
        HoneypotLineRenderer.SetVertexCount(0);
        this.Parent.Info.Cooldown = 5f;
        this.Parent.MyActorInfo.FunctionHitChance = 100f;
        this.Parent.Info.CoreCost += 2;
        this.LookAtSpeed = 4f;
    }

    // Update is called once per frame
    void Update()
    {
        if (this.Parent.IsActive)
        {
            bool canFire = false;
            if (CooldownRemaining <= 0f)
            {
                canFire = true;
            }
            else
            {
                CooldownRemaining -= InterruptTime.deltaTime;
            }

            if (TrackEnemy)
            {
                if (isFiring)
                {
                    if (this.Parent.LockedTarget == null)
                    {
                        StopLaser();
                    }
                    else
                    {
                        this.PulseParticles.transform.position = this.Parent.LockedTarget.position;
                        LookAtClosestTransform();
                        this.HoneypotLineRenderer.SetPosition(1, Vector3.forward * (this.Parent.LockedTarget.position - this.transform.position).magnitude);
                    }
                }
                else
                {
                    if (this.Parent.LockedTarget != null)
                    {
                        float angle = LookAtClosestTransform();

                        if ((angle < AngleThreshold) && canFire)
                        {
                            FireAtEnemy(this.Parent.LockedTarget.position - this.transform.position);
                        }
                    }
                }
            }
        }
    }

    private float LookAtClosestTransform()
    {
        Vector3 relativePos = this.Parent.LockedTarget.position - this.transform.position;
        this.Parent.FunctionRoot.rotation = Quaternion.Slerp(this.Parent.FunctionRoot.rotation, Quaternion.LookRotation(relativePos), InterruptTime.deltaTime * LookAtSpeed);
        float angle = Quaternion.Angle(this.Parent.FunctionRoot.rotation, Quaternion.LookRotation(relativePos));
        return angle;
    }

    private void FireAtEnemy(Vector3 relativePos)
    {
        isFiring = true;
        CooldownRemaining = this.Parent.Info.Cooldown;
        //this.closestVirus.TakeDamage(this.Parent.Info.DamagePerHit);

        this.Parent.lockedVirus.ForceAggro(this.transform);
        this.HoneypotLineRenderer.SetVertexCount(2);
        this.HoneypotLineRenderer.SetPosition(0, Vector3.zero);
        this.HoneypotLineRenderer.SetPosition(1, Vector3.forward * relativePos.magnitude);
        this.PulseParticles.transform.position = this.Parent.lockedVirus.transform.position;
        this.PulseParticles.startSpeed = -relativePos.magnitude;
        this.PulseParticles.Play();
        //this.BurstParticles.Emit(100);
        //this.BurstParticles.transform.localPosition = Vector3.forward * relativePos.magnitude / 2;
        //this.BurstParticles.transform.localScale = Vector3.right * relativePos.magnitude / 2;
        //StartCoroutine(this.WaitAndStopLaser());
    }


    private void StopLaser()
    {
        isFiring = false;
        this.HoneypotLineRenderer.SetVertexCount(0);
        this.PulseParticles.Stop();
    }

    void OnDestroy()
    {
        if (this.Parent.lockedVirus != null)
            this.Parent.lockedVirus.UnforceAggro();
    }
}
