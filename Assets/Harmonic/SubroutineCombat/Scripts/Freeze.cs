using UnityEngine;
using System.Collections;

public class Freeze : SubroutineFunction
{
    
    public float LaserPersistTime = .5f;
    public override bool OnlyTrackActiveViruses { get { return true; } }
    
    public ParticleSystem FrostParticles;
    public LineRenderer FreezeLineRenderer;

    // Use this for initialization
    void Start()
    {
        this.FreezeLineRenderer = this.transform.Find("FunctionRoot/Freeze").GetComponent<LineRenderer>();
        this.FrostParticles = this.transform.Find("FunctionRoot/Freeze/BurstParticles").GetComponent<ParticleSystem>();
        FreezeLineRenderer.SetVertexCount(0);
        this.Parent.Info.Cooldown = 5f;
        this.Parent.Info.HitChance += 50f;
        this.Parent.MyActorInfo.FunctionHitChance = 50f;
        this.Parent.Info.CoreCost += 2;

        this.Parent.FreezeEffect = new Actor.StatusEffect()
        {
            BlockModifier = -100,
            Duration = 2,
            CooldownModifier = 0,
            Type = Actor.StatusType.Frozen           
        };
    }

    // Update is called once per frame
    void Update()
    {
        if (CanAttackEnemy())
        {
            FireAtEnemy(this.Parent.LockedTarget.position - this.transform.position);
        }
    }

    private void FireAtEnemy(Vector3 relativePos)
    {
        isFiring = true;
        CooldownRemaining = this.Parent.Info.Cooldown;
        
        this.Parent.DoAttack(this.Parent.lockedMalware, AttackType.Freeze);

        this.FreezeLineRenderer.SetVertexCount(2);
        this.FreezeLineRenderer.SetPosition(0, Vector3.zero);
        this.FreezeLineRenderer.SetPosition(1, Vector3.forward * relativePos.magnitude);
        
        this.FrostParticles.Emit(100);
        this.FrostParticles.transform.localPosition = Vector3.forward * relativePos.magnitude / 2;
        this.FrostParticles.transform.localScale = Vector3.right * relativePos.magnitude / 2;
        StartCoroutine(this.WaitAndStopLaser());
    }


    private IEnumerator WaitAndStopLaser()
    {
        yield return new WaitForSecondsInterruptTime(this.LaserPersistTime);
        this.FreezeLineRenderer.SetVertexCount(0);
        isFiring = false;
    }
}
