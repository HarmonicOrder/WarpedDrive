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

    public float angleTightness = 5f;

    internal bool TrackEnemy = false;
	protected float CooldownRemaining = -1f;
	protected bool isFiring = false;
    protected float LookAtSpeed = 2f;
    
	public virtual float TracerSlowRange { get { return 90f; } }
    public virtual float TracerStopRange { get { return 80f; } }
	
	protected IEnumerator WaitCooldown()
	{		
		yield return new WaitForSecondsInterruptTime(this.Parent.Info.Cooldown);
		isFiring = false;
	}

    protected bool CanAttackEnemy()
    {
        if (this.Parent.IsActive && !EnumExtensions.Has(this.Parent.ActiveStatuses, Actor.StatusType.Frozen))
        {
            bool canFire = false;
            if (CooldownRemaining <= 0f)
            {
                canFire = true;
            }
            else
            {
                CooldownRemaining -= InterruptTime.deltaTime;
            }

            if (TrackEnemy && !isFiring)
            {
                if (this.Parent.LockedTarget != null)
                {
                    Vector3 relativePos = this.Parent.LockedTarget.position - this.transform.position;
                    this.Parent.FunctionRoot.rotation = Quaternion.Slerp(this.Parent.FunctionRoot.rotation, Quaternion.LookRotation(relativePos), InterruptTime.deltaTime * LookAtSpeed);
                    float angle = Quaternion.Angle(this.Parent.FunctionRoot.rotation, Quaternion.LookRotation(relativePos));

                    if ((angle < angleTightness) && canFire)
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }


    public static uint GetCoreCost(string name)
    {
        switch (name.ToLower())
        {
            case "delete":
                return 1;
            case "corrupt":
                return 1;
            case "terminate":
                return 2;
            case "honeypot":
                return 3;
            case "freeze":
                return 2;
            case "lag":
                return 2;
            case "sandbox":
                return 3;
        }
        print("fallthrough core cost");
        return 1;
    }
}
