using UnityEngine;
using System.Collections;

public class SubroutineFunction : MonoBehaviour {

	public Subroutine Parent {get;set;}
	
	internal bool TrackEnemy = false;
	protected Transform closestTransform;
	protected IMalware closestVirus;
	protected float CooldownRemaining = -1f;
	protected bool isFiring = false;
	protected float Range = 999f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	protected void FindClosestTransform()
	{
		if (ActiveSubroutines.MalwareList.Count == 0)
		{
			this.closestVirus = null;
			this.closestTransform = null;
			return;
		}
		
		//comparing range squared vs magnitude squared is a performance enhancement
		//it eliminates the expensive square root calculation
		float closest = Range * Range;
		foreach( IMalware mal in ActiveSubroutines.MalwareList)
		{
			float dist = (mal.transform.position - this.transform.position).sqrMagnitude; 
			//if this has a higher priority than now
			//and the distance is closer
			if (dist < closest * mal.AttackPriority)
			{
				this.closestVirus = mal;
				this.closestTransform = mal.transform;
				closest = dist;
			}
		}
	}

	
	protected IEnumerator WaitCooldown()
	{		
		yield return new WaitForSeconds(this.Parent.Info.FireRate);
		isFiring = false;
	}

}
