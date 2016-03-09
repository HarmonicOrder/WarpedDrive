using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Ability {

    public static List<Ability> List = new List<Ability>()
    {
        new Ability()
        {
            Name = "suspend",
            Cooldown = 90,
            GoodChance = 75,
            BadChance = 25,
            Duration = 5
        },
        new Ability()
        {
            Name = "overclock",
            Cooldown = 180,
            GoodChance = 65,
            BadChance = 35,
            Duration = 60
        },
        new Ability()
        {
            Name = "fork",
            Cooldown = 120,
            GoodChance = 75,
            BadChance = 25,
            Duration = 0
        },
        new Ability()
        {
            Name = "hyperthread",
            Cooldown = 90,
            GoodChance = 66,
            BadChance = 33,
            Duration = 5
        },
    };

    public static Ability ByName(string name)
    {
        return List.Find(l => l.Name == name);
    }

    public static void SetAvailable(List<string> unlockedAbilities)
    {
        foreach(string unlock in unlockedAbilities)
        {
            Ability a = ByName(unlock);
            if (a != null)
                a.IsAvailable = true;
        }
    }

    public bool IsAvailable { get; set; }
    public string Name { get; set; }
    public float Cooldown { get; set; }
    public float GoodChance { get; set; }
    public float BadChance { get; set; }
    public float Duration { get; set; }

    private GameObject targetG;
    private ICombatant target;
    private Vector3 originalPosition;
    private bool WentWell;

    public void Activate(ILockTarget target)
    {
        if (target != null)
        {
            this.targetG = target.transform.gameObject;
            if (target is ICombatant)
            {
                this.target = (ICombatant)target;
            }
            else
            {
                this.target = target.transform.GetComponent<ICombatant>();
            }
        }

        switch (Name)
        {
            case "suspend":
                if (RollDice())
                {
                    originalPosition = this.target.transform.position;
                    this.target.transform.position = Vector3.one * 999f;
                }
                else
                {
                    (this.target as Combatant).AddEffect(new Actor.StatusEffect()
                    {
                        BlockModifier = 100f,
                        Duration = 5f,
                        Type = Actor.StatusType.Superblock
                    });
                    ToastLog.Toast("SUSPEND Failure!\nVirus +100% Block");
                }
                break;
            case "overclock":
                break;
            case "fork":
                break;
            case "hyperthread":
                if (RollDice())
                {
                    (this.target as Subroutine).AddEffect(new Actor.StatusEffect()
                    {
                        CooldownModifier = -.5f,
                        Duration = 5f,
                        Type = Actor.StatusType.Hyperthread
                    });
                }
                else
                {
                    (this.target as Subroutine).AddEffect(new Actor.StatusEffect()
                    {
                        Duration = 5f,
                        Type = Actor.StatusType.Frozen
                    });
                    this.target.Freeze(null);
                    ToastLog.Toast("HYPERTHREAD Failure!\nFrozen for 5s");
                }
                break;
        }
    }

    private bool RollDice()
    {
        WentWell = Combatant.RollDice(this.GoodChance);
        return WentWell;
    }

    public bool CanActivate(ILockTarget target)
    {
        switch (Name)
        {
            case "suspend":
                return target != null && target is VirusAI;
            case "overclock":
                return true;
            case "fork":
                return target != null && target is SubroutineHarness;
            case "hyperthread":
                return target != null && target is SubroutineHarness;
        }
        return false;
    }

    public void Deactivate()
    {
        switch (Name)
        {
            case "suspend":
                if (WentWell)
                {
                    if (this.targetG != null && this.target != null)
                    {
                        this.target.transform.position = originalPosition;
                    }
                }
                else
                {

                }
                break;
            case "overclock":
                break;
            case "fork":
                break;
            case "hyperthread":
                break;
        }
    }
}
