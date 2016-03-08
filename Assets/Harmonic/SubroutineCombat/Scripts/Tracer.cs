using UnityEngine;
using System.Collections;
using System;

public class Tracer : SubroutineMovement {
	public static int firingLineID = 0;

	public float lookAtSpeed = 2f;
    //public float engagementDistance = 200f;
    public float avoidDistance = 100f;
    public bool doAvoid = false;
	public float followDistance = 30f;
	public float moveSpeed = 20f;
	public float fireSpeed = 40f;
	public float FireTime = 1f;
    public bool MoveToClearFiringLine = false;

    private float CurrentFireTime = 0f;
    private Quaternion ClearFiringLineRotation;

    // Use this for initialization
    void Start () {
        CalculateFiringLineRotation();
        this.Parent.Info.BonusHitModifier = 1f;
    }

    private const float Divergence = 15f;
    private const float maximumEngagementDistance = 300f;

    private void CalculateFiringLineRotation()
    {
        //monotonically increasing, maybe better than random.range(0,8)
        switch(firingLineID++ % 8)
        {
            case 0:
                ClearFiringLineRotation = Quaternion.Euler(0, -Divergence, 0);
                break;
            case 1:
                ClearFiringLineRotation = Quaternion.Euler(0, Divergence, 0);
                break;
            case 2:
                ClearFiringLineRotation = Quaternion.Euler(-Divergence, 0, 0);
                break;
            case 3:
                ClearFiringLineRotation = Quaternion.Euler(Divergence, 0, 0);
                break;
            case 4:
                ClearFiringLineRotation = Quaternion.Euler(Divergence, Divergence, 0);
                break;
            case 5:
                ClearFiringLineRotation = Quaternion.Euler(Divergence, -Divergence, 0);
                break;
            case 6:
                ClearFiringLineRotation = Quaternion.Euler(-Divergence, -Divergence, 0);
                break;
            default:
                ClearFiringLineRotation = Quaternion.Euler(-Divergence, Divergence, 0);
                break;
        }
    }

    public override void Fire()
    {
        CurrentFireTime = 0f;
        this.transform.SetParent(null);
        BeingFired = true;
    }

    // Update is called once per frame
    void Update () {
        if (BeingFired)
        {
            if (CurrentInstantiateTime > TimeToInstantiate)
            {
                BeingFired = false;

                this.Parent.Function.TrackEnemy = true;
            }
            CurrentInstantiateTime += InterruptTime.deltaTime;
            float t = CurrentInstantiateTime / TimeToInstantiate;
            this.transform.Translate(0, 0, Mathf.Lerp(0, moveSpeed, t) * 3 * InterruptTime.deltaTime, Space.Self);
            
        }
        else if (Parent.IsActive)
		{
			float moveSpeed = this.moveSpeed;

			if (CurrentFireTime <= FireTime)
			{
				CurrentFireTime += InterruptTime.deltaTime;
				moveSpeed = this.fireSpeed;
			}

			if (Parent.LockedTarget != null)
			{
				Vector3 relativePos = this.Parent.LockedTarget.position - this.transform.position;
                Quaternion look = Quaternion.LookRotation(relativePos);

                if (MoveToClearFiringLine)
                {
                    look *= ClearFiringLineRotation;
                }

				this.transform.rotation = Quaternion.Slerp(this.transform.rotation, look, InterruptTime.deltaTime * lookAtSpeed);
				
				if (relativePos.magnitude > maximumEngagementDistance)
				{
					//do not engage
				} 
				else 
                //if we're an avoidance type try to move away
                if (doAvoid && relativePos.magnitude < avoidDistance)
                {
                    this.transform.Translate(0, 0, -InterruptTime.deltaTime * moveSpeed, Space.Self);
                }
                else if (relativePos.magnitude < Parent.Function.TracerStopRange)
                {

                }
                //move slowly
                else if (relativePos.magnitude < Parent.Function.TracerSlowRange){
					this.transform.Translate(0, 0, InterruptTime.deltaTime * moveSpeed / 3f, Space.Self);
				}
				else 
				{
					//within engagement, outside optimum, move closer
					this.transform.Translate(0, 0, InterruptTime.deltaTime * moveSpeed, Space.Self);
				}
			}
		}
	}
}
