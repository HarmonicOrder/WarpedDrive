using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SubroutineFunction : MonoBehaviour {

	public Subroutine Parent {get;set;}
    public List<Upgrade> Upgrades { get; set; }
    /// <summary>
    /// As opposed to passive ones, like meshes
    /// </summary>
    public virtual bool OnlyTrackActiveViruses { get { return false; } }

    internal bool TrackEnemy = false;
	protected float CooldownRemaining = -1f;
	protected bool isFiring = false;
    
	public virtual float TracerSlowRange { get { return 90f; } }
    public virtual float TracerStopRange { get { return 80f; } }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	protected IEnumerator WaitCooldown()
	{		
		yield return new WaitForSecondsInterruptTime(this.Parent.Info.Cooldown);
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
            case "honeypot":
                return 3;
            case "freeze":
                return 2;
        }
        print("fallthrough core cost");
        return 1;
    }
}
