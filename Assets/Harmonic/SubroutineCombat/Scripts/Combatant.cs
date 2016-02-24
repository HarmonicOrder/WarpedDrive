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
    
    public virtual bool DoAttack(ICombatant target)
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

    private bool RollDice(float chance)
    {
        return UnityEngine.Random.Range(1, 100f) <= chance;
    }
}
