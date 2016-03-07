using UnityEngine;
using System.Collections;

public class Corrupt : SubroutineFunction {

	// Use this for initialization
	void Start () {
        this.Parent.Info.Cooldown = .3f;
        this.Parent.Info.HitChance += 2f;
        this.Parent.MyActorInfo.FunctionHitChance = 2f;
        this.Parent.Info.CoreCost += 1;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
