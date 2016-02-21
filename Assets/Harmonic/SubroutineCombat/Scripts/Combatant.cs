using UnityEngine;
using System.Collections;
using System;

public class Combatant : Actor, ICombatant {

    public float KillChance
    {
        get
        {
            return this.Info.HitChance;
        }
    }

    public float SaveChance
    {
        get
        {
            return this.Info.SaveChance;
        }
    }

    public float Reboots
    {
        get
        {
            return this.Info.Reboots;
        }
    }
    
    public virtual void DoAttack(ICombatant target)
    {
        if (RollDice(this.KillChance))
        {
            if (target.TrySave(this))
            {
                target.DoOnBlock(this);
            }
            else
            {
                //this reads backwards
                //it's actually "kill the target, this is who killed the target"
                target.Kill(this);
            }
        }
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

    public virtual void DoOnReboot(){ }

    public virtual void DoOnBlock(ICombatant attacker)
    {
        throw new NotImplementedException();
    }

    public virtual void DoOnKilled(ICombatant attacker)
    {
        throw new NotImplementedException();
    }

    private bool RollDice(float chance)
    {
        return UnityEngine.Random.Range(1, 100f) <= chance;
    }
}
