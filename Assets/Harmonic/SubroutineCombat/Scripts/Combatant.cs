using UnityEngine;
using System.Collections;
using System;
using System.Text;

public class Combatant : Actor, ICombatant {


    public float KillChance
    {
        get
        {
            return this.Info.HitChance + (this.Info.HitChance * this.Info.BonusHitModifier) + this.StatusEffectHitModifier;
        }
    }

    public float BlockChance
    {
        get
        {
            return this.Info.BlockChance + this.StatusEffectBlockModifier;
        }
    }

    public float Cooldown
    {
        get
        {
            return this.Info.Cooldown * this.StatusEffectCooldownModifier;
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

    public StatusEffect FreezeEffect, LagEffect;
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
                    case AttackType.Lag:
                        target.Lag(this);
                        if (target is Combatant)
                            (target as Combatant).AddEffect(this.LagEffect.Clone());
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
        return RollDice(this.BlockChance);
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

    public void Lag(ICombatant attacker)
    {
        Popup.Create(this.transform.position + Vector3.up * 4, null, Popup.Popups.Lag, (this is Subroutine));
    }

    public string InformationReadout()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(this.Info.Name.ToUpper());
        sb.Append("\r\n--------\r\n");
        HitReadout(sb);
        sb.Append("\r\n\r\n");
        CooldownReadout(sb);
        sb.Append("\r\n\r\n");
        BlockReadout(sb);
        RebootReadout(sb);
        TargetReadout(sb);
        return sb.ToString();
    }

    protected void EffectReadout(StringBuilder sb, StatusEffect se, float effectAmount)
    {
        if (effectAmount > 0)
        {
            sb.Append("+");
            sb.Append(effectAmount.ToString().PadLeft(3, ' '));
            sb.Append("% [");
            sb.Append(se.Type.ToString());
            sb.Append("]\r\n");
        }
        else if (effectAmount < 0)
        {
            sb.Append("-");
            sb.Append(Math.Abs(effectAmount).ToString().PadLeft(3, ' '));
            sb.Append("% [");
            sb.Append(se.Type.ToString());
            sb.Append("]\r\n");
        }
    }

    public void HitReadout(StringBuilder sb)
    {
        sb.Append("HIT\r\n");
        FillHitReadout(sb);
        foreach(StatusEffect s in this.Effects)
        {
            EffectReadout(sb, s, s.HitModifier);
        }
        sb.AppendFormat("={0}%", this.KillChance.ToString().PadLeft(3, ' '));
    }

    protected virtual void FillHitReadout(StringBuilder sb)
    {
        sb.Append(this.Info.HitChance.ToString().PadLeft(4, ' '));
        sb.Append("%\r\n");
    }

    public void CooldownReadout(StringBuilder sb)
    {
        sb.Append("COOLDOWN\r\n");
        sb.Append(this.Info.Cooldown.ToString().PadLeft(4, ' '));
        sb.Append("s\r\n");
    }

    public void BlockReadout(StringBuilder sb)
    {
        sb.Append("BLOCK\r\n");
        FillBlockReadout(sb);
        foreach (StatusEffect s in this.Effects)
        {
            EffectReadout(sb, s, s.BlockModifier);
        }
        sb.AppendFormat("={0}%", this.BlockChance.ToString().PadLeft(3, ' '));
    }

    protected virtual void FillBlockReadout(StringBuilder sb)
    {
        sb.Append(this.Info.BlockChance.ToString().PadLeft(4, ' '));
        sb.Append("%\r\n");
    }

    public void RebootReadout(StringBuilder sb)
    {
        if (this.Info.Reboots > 0)
        {
            sb.Append("\r\n\r\n");
            sb.Append(this.Info.Reboots);
            sb.Append(" REBOOT");
        }
    }

    public void TargetReadout(StringBuilder sb)
    {
    }
}
