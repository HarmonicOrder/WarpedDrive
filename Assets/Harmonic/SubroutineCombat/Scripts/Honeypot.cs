using UnityEngine;
using System.Collections;

public class Honeypot : SubroutineFunction
{
    public float LookAtSpeed = 2f;
    public float LaserPersistTime = .5f;

    public ParticleSystem PulseParticles;
    //public ParticleSystem BurstParticles;
    public LineRenderer HoneypotLineRenderer;

    // Use this for initialization
    void Start()
    {
        this.HoneypotLineRenderer = this.transform.Find("FunctionRoot/Honeypot").GetComponent<LineRenderer>();
        this.PulseParticles = this.transform.Find("FunctionRoot/Honeypot/PulseParticles").GetComponent<ParticleSystem>();
        //this.BurstParticles = this.transform.Find("FunctionRoot/Terminate/BurstParticles").GetComponent<ParticleSystem>();
        HoneypotLineRenderer.SetVertexCount(0);
        this.Parent.Info.DamagePerHit = 5f;
        this.Parent.Info.FireRate = 5f;
        this.Parent.Info.CoreCost += 2;
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
                CooldownRemaining -= Time.deltaTime;
            }

            if (TrackEnemy && !isFiring)
            {
                FindClosestMalware(true);

                if (this.closestTransform != null)
                {
                    Vector3 relativePos = this.closestTransform.position - this.transform.position;
                    this.Parent.FunctionRoot.rotation = Quaternion.Slerp(this.Parent.FunctionRoot.rotation, Quaternion.LookRotation(relativePos), Time.deltaTime * LookAtSpeed);
                    float angle = Quaternion.Angle(this.Parent.FunctionRoot.rotation, Quaternion.LookRotation(relativePos));

                    if ((angle < 5f) && canFire)
                    {
                        FireAtEnemy(this.closestTransform.position - this.transform.position);
                    }
                }
            }
        }
    }

    private void FireAtEnemy(Vector3 relativePos)
    {
        isFiring = true;
        CooldownRemaining = this.Parent.Info.FireRate;
        //this.closestVirus.TakeDamage(this.Parent.Info.DamagePerHit);
        this.closestVirus.ForceAggro(this.transform);
        this.HoneypotLineRenderer.SetVertexCount(2);
        this.HoneypotLineRenderer.SetPosition(0, Vector3.zero);
        this.HoneypotLineRenderer.SetPosition(1, Vector3.forward * relativePos.magnitude);
        this.PulseParticles.startSpeed = -relativePos.magnitude;
        this.PulseParticles.Play();
        //this.BurstParticles.Emit(100);
        //this.BurstParticles.transform.localPosition = Vector3.forward * relativePos.magnitude / 2;
        //this.BurstParticles.transform.localScale = Vector3.right * relativePos.magnitude / 2;
        //StartCoroutine(this.WaitAndStopLaser());
    }


    private void StopLaser()
    {
        this.HoneypotLineRenderer.SetVertexCount(0);
        this.PulseParticles.Stop();
    }
}
