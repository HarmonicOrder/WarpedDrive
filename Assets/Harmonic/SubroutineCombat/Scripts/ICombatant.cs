using UnityEngine;
using System.Collections;

public interface ICombatant {
    bool Defenseless { get; }
    float KillChance { get;}
    float SaveChance { get;}
    float Reboots { get;}
    bool DoAttack(ICombatant target, AttackType type = AttackType.Kill);
    bool TrySave(ICombatant attacker);
    void Kill(ICombatant attacker);
    void Freeze(ICombatant attacker);
    void DoOnBlock(ICombatant attacker);
    Transform transform { get; }
    GameObject gameObject { get; }
}

public enum AttackType { Kill, Freeze, Lag, Sandbox }

public static class ICombatantExtensions
{
    public static float GetKillChance(this ICombatant attacker, ICombatant target)
    {
        return attacker.KillChance / 100f * target.SaveChance / 100f;
    }
}
