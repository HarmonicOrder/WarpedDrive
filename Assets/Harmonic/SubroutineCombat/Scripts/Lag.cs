using UnityEngine;
using System.Collections;
using System;

public class Lag : SubroutineFunction
{
    public float LookAtSpeed = 2f;
    public float LaserPersistTime = .5f;
    public Transform LagBombVisualization;

    private HarmonicUtils.LerpContext BombLerp;
    private GameObject CurrentLagBomb;


    // Use this for initialization
    void Start()
    {
        this.Parent.Info.Cooldown = 5f;
        this.Parent.Info.HitChance += 100f;
        this.Parent.MyActorInfo.FunctionHitChance = 100f;
        this.Parent.Info.CoreCost += 2;

        this.Parent.LagEffect = new Actor.StatusEffect()
        {
            BlockModifier = -10f,
            Duration = 2f,
            CooldownModifier = 1f,
            HitModifier = -10f,
            Type = Actor.StatusType.Lagged
        };
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
                if (this.Parent.LockedTarget != null)
                {
                    Vector3 relativePos = this.Parent.LockedTarget.position - this.transform.position;
                    this.Parent.FunctionRoot.rotation = Quaternion.Slerp(this.Parent.FunctionRoot.rotation, Quaternion.LookRotation(relativePos), InterruptTime.deltaTime * LookAtSpeed);
                    float angle = Quaternion.Angle(this.Parent.FunctionRoot.rotation, Quaternion.LookRotation(relativePos));

                    if ((angle < 5f) && canFire)
                    {
                        FireAtEnemy(this.Parent.LockedTarget.position);
                    }
                }
            }
        }
    }
    
    private void FireAtEnemy(Vector3 targetPos)
    {
        CooldownRemaining = this.Parent.Info.Cooldown;

        CurrentLagBomb = GetLagBomb(targetPos);
        CurrentLagBomb.GetComponent<LagBomb>().Fire(this.Parent, targetPos, 2f, 10);
    }

    private GameObject GetLagBomb(Vector3 targetPos)
    {
        GameObject result = null;
        if (this.CurrentLagBomb == null)
        {
            this.CurrentLagBomb = GameObject.Instantiate<Transform>(LagBombVisualization).gameObject;
        }
        result = this.CurrentLagBomb;
        result.SetActive(true);
        result.transform.position = this.transform.position;
        result.transform.rotation = Quaternion.LookRotation(targetPos - this.transform.position);

        return result;
    }
    
}
