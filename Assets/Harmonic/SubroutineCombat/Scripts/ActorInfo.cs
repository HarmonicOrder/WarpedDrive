using UnityEngine;
using System.Collections;

public class ActorInfo {
	
	public float DamagePerHit = 0f;
	public float FireRate = 0f;
	public float OffenseRating {
		get {
			return DamagePerHit * FireRate;
		}
	}
	public float MaxHitPoints = 0f;
	public float HitPoints = 0f;
	public float ArmorPoints = 0f;
	public float DefenseRating {
		get {
			return HitPoints + ArmorPoints;
		}
	}
	public int CoreCost = 0;
	public string Name = "";


	public ActorInfo() {	
	}
	
	public string GetTargetRichText()
	{
		return string.Format("{0}\r\n<color=red>{1} ATK</color> <color=green>{2} DEF</color>", Name, OffenseRating, DefenseRating);
	}
}
