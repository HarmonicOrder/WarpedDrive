using UnityEngine;
using System.Collections;

public class ActorInfo {
	
	public float FireRate = 0f;
    public float HitChance = 0f;
    public float SaveChance = 0f;
    public float Reboots = 0f;
	public int CoreCost = 0;
	public string Name = "";


	public ActorInfo() {	
	}
	
	public string GetTargetRichText()
	{
		return string.Format("{0}\r\n<color=red>{1:##.#}% Kill</color> <color=green>{2:#0.#}% Block</color>", Name, HitChance, SaveChance);
	}
}
