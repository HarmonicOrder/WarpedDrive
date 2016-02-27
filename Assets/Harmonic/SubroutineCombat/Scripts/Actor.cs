using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Actor : MonoBehaviour {

	public ActorInfo Info {get;set;}
    public float StatusEffectBlockModifier { get; set; }
    public float StatusEffectHitModifier { get; set; }
    public float StatusEffectFireRateModifier { get; set; }

    void Awake () {
		OnAwake();
	}
	
	protected virtual void OnAwake(){}

	// Use this for initialization
	void Start () {
		OnStart();
	}
	
	protected virtual void OnStart(){}
	
	// Update is called once per frame
	void Update () {
        IterateEffects();

        if (EnumExtensions.Has(this.ActiveStatuses, StatusType.Frozen))
        {
            //haha you're frozen
        }
        else
        {
		    OnUpdate();
        }
	}

    protected virtual void OnUpdate() { }


    void OnDestroy()
    {
        _OnDestroy();
    }

    protected virtual void _OnDestroy() { }
	
	protected IEnumerator SelfDestruct(float timer)
	{		
		yield return new WaitForSecondsInterruptTime(timer);
		GameObject.Destroy(this.gameObject);
	}

    public StatusType ActiveStatuses = StatusType.None;
    public List<StatusEffect> Effects = new List<StatusEffect>();

    public void AddEffect(StatusEffect newStatus)
    {
        newStatus.Countdown = newStatus.Duration;
        ActiveStatuses |= newStatus.Type;
        StatusEffectBlockModifier += newStatus.BlockModifier;
        StatusEffectFireRateModifier += newStatus.FireRateModifier;
        StatusEffectHitModifier += newStatus.HitModifier;
        Effects.Add(newStatus);
    }

    private void RemoveEffect(StatusEffect oldStatus)
    {
        ActiveStatuses &= ~oldStatus.Type;
        StatusEffectBlockModifier -= oldStatus.BlockModifier;
        StatusEffectFireRateModifier -= oldStatus.FireRateModifier;
        StatusEffectHitModifier -= oldStatus.HitModifier;
        Effects.Remove(oldStatus);
    }

    public void IterateEffects()
    {
        //when removing go backwards through the list to avoid index issues
        for (int i = Effects.Count - 1; i >= 0; i--)
        {
            Effects[i].Countdown -= InterruptTime.deltaTime;
            if (Effects[i].Countdown < 0)
            {
                RemoveEffect(Effects[i]);
            }
        }
    }

    public class StatusEffect
    {
        public StatusType Type { get; set; }
        public float Duration { get; set; }
        public float Countdown { get; set; }
        public float BlockModifier { get; set; }
        public float HitModifier { get; set; }
        public float FireRateModifier { get; set; }

        public StatusEffect Clone()
        {
            return new StatusEffect()
            {
                Type = this.Type,
                Duration = this.Duration,
                BlockModifier = this.BlockModifier,
                HitModifier = this.HitModifier,
                FireRateModifier = this.FireRateModifier
            };
        }
    }

    [Flags]
    public enum StatusType { None, Frozen, Sandboxed, Lagged }
}
