using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SubroutineFunction : MonoBehaviour {

	public Subroutine Parent {get;set;}
    public List<Upgrade> Upgrades { get; set; }

    internal bool TrackEnemy = false;
	protected Transform closestTransform;
	protected IMalware closestMalware;
    protected VirusAI closestVirus;
	protected float CooldownRemaining = -1f;
	protected bool isFiring = false;
	public float Range = 300f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    protected void FindClosestMalware(bool onlyVirus = false)
	{
		if (ActiveSubroutines.MalwareList.Count == 0)
		{
			this.closestMalware = null;
			this.closestTransform = null;
			return;
		}
		
		//comparing range squared vs magnitude squared is a performance enhancement
		//it eliminates the expensive square root calculation
		float closest = Range * Range;
		foreach( IMalware mal in ActiveSubroutines.MalwareList)
		{
			float dist = (mal.transform.position - this.transform.position).sqrMagnitude / mal.AttackPriority; 
			//if this has a higher priority than now
			//and the distance is closer
			if (dist < closest)
			{
                if (!onlyVirus || (mal is VirusAI))
                {
				    this.closestMalware = mal;
				    this.closestTransform = mal.transform;

                    if (mal is VirusAI)
                        this.closestVirus = mal as VirusAI;

				    closest = dist;
                }
			}
		}
	}

	
	protected IEnumerator WaitCooldown()
	{		
		yield return new WaitForSeconds(this.Parent.Info.FireRate);
		isFiring = false;
	}


    public static uint GetCoreCost(string name)
    {
        switch (name.ToLower())
        {
            case "delete":
                return 1;
            case "terminate":
                return 2;
        }
        print("fallthrough core cost");
        return 1;
    }
}
