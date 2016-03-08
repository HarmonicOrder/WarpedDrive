using UnityEngine;
using System.Collections;

public class ActorInfo {
	
	public float Cooldown = 0f;
    public float HitChance = 0f;
    public float BonusHitModifier = 0f;
    public float BlockChance = 0f;
    public float Reboots = 0f;
	public int CoreCost = 0;
	public string Name = "";


    public ActorInfo() {	
	}
	
	public string GetTargetRichText()
	{
		return string.Format("{0}\r\n<color=red>{1:##.#}% Kill</color> <color=green>{2:#0.#}% Block</color>", Name, HitChance, BlockChance);
	}
}

public class SubroutineActorInfo : ActorInfo
{
    public float MovementBlockChance = 0f;
    public float MovementHitModifier = 1f;
    public float FunctionBlockChance = 0f;
    public float FunctionHitChance = 0f;
}
