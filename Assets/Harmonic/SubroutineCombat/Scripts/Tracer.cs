using UnityEngine;
using System.Collections;
using System;

public class Tracer : SubroutineMovement {
	
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
	}

    private const float Divergence = 15f;
    private const float maximumEngagementDistance = 300f;

    private void CalculateFiringLineRotation()
    {
        switch(UnityEngine.Random.Range(0, 8))
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
        this.Parent.Function.TrackEnemy = true;
    }

    // Update is called once per frame
    void Update () {
		if(Parent.IsActive)
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
                    look *= ClearFiringLineRotation;

				this.transform.rotation = Quaternion.Slerp(this.transform.rotation, look, InterruptTime.deltaTime * lookAtSpeed);
				
				if (relativePos.magnitude > maximumEngagementDistance)
				{
					//do not engage
				} 
				else 
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
            //else
            //{
            //    IMalware closest = ActiveSubroutines.FindClosestMalware(this.transform.position, 100f);
            //    if (closest != null)
            //    {
            //        Parent.LockedTarget = closest.transform;
            //    }
            //}
		}
	}
}
