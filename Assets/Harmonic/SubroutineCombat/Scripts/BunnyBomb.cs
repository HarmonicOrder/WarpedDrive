using UnityEngine;
using System.Collections;

public class BunnyBomb : VirusAI {

    public override string DisplayNameSingular { get { return "Rabbit"; } }
    public override string DisplayNamePlural { get { return "Rabbits"; } }

    protected override void OnAwake()
    {
        base.OnAwake();
        this.Info = new ActorInfo()
        {
            Name = "Rabbit",
            DamagePerHit = 0f,
            FireRate = 0f,
            HitPoints = 1f,
            ArmorPoints = 0f
        };
    }
}
