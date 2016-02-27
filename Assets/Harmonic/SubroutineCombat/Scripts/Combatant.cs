using UnityEngine;
using System.Collections;
using System;

public class Combatant : Actor, ICombatant {
    //idea: status effects as list of effect classes
    //each with their own duration counter
    //when duration < 0, remove from list and reverse changes
    public float KillChance
    {
        get
        {
            return this.Info.HitChance + this.StatusEffectHitModifier;
        }
    }

    public float SaveChance
    {
        get
        {
            return this.Info.SaveChance + this.StatusEffectBlockModifier;
        }
    }

    public float FireRate
    {
        get
        {
            return this.Info.FireRate * this.StatusEffectFireRateModifier;
        }
    }

    public float Reboots
    {
        get
        {
            return this.Info.Reboots;
        }
    }

    public bool Defenseless
    {
        get { return false; }
    }

    public StatusEffect FreezeEffect;
    public virtual bool DoAttack(ICombatant target, AttackType type = AttackType.Kill)
    {
        if (target.Defenseless || RollDice(this.KillChance))
        {
            if (target.TrySave(this))
            {
                target.DoOnBlock(this);
            }
            else
            {
                switch (type)
                {
                    case AttackType.Freeze:
                        target.Freeze(this);
                        if (target is Combatant)
                            (target as Combatant).AddEffect(this.FreezeEffect.Clone());
                        break;
                    default:
                        //this reads backwards
                        //it's actually "kill the target, this is who killed the target"
                        target.Kill(this);
                        break;
                }

            }

            return true;
        }
        return false;
    }


    public virtual bool TrySave(ICombatant attacker)
    {
        return RollDice(this.SaveChance);
    }

    public virtual void Kill(ICombatant attacker)
    {
        if (this.Info.Reboots > 0)
        {
            this.Info.Reboots -= 1;
            this.DoOnReboot();
        }
        else
        {
            this.DoOnKilled(attacker);
        }
    }

    public virtual void DoOnReboot()
    {
        Popup.Create(this.transform.position + Vector3.up * 4, null, Popup.Popups.Reboot, (this is Subroutine));
    }

    public virtual void DoOnBlock(ICombatant attacker)
    {
        Popup.Create(this.transform.position + Vector3.up * 4, null, Popup.Popups.Block, (this is Subroutine));
    }

    public virtual void DoOnKilled(ICombatant attacker)
    {
        throw new NotImplementedException();
    }

    public static bool RollDice(float chance)
    {
        return UnityEngine.Random.Range(1, 100f) <= chance;
    }
    
    public void Freeze(ICombatant attacker)
    {
        Popup.Create(this.transform.position + Vector3.up * 4, null, Popup.Popups.Freeze, (this is Subroutine));
    }
}
