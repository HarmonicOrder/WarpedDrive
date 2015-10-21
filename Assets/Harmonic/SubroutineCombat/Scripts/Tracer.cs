using UnityEngine;
using System.Collections;

public class Tracer : SubroutineMovement {
	
	public float lookAtSpeed = 2f;
	public float engagementDistance = 200f;
	public float followDistance = 30f;
	public float moveSpeed = 20f;
	public float fireSpeed = 100f;
	public float FireTime = 1f;

	private bool BeingFired = false; //maybe add public getter
	private float CurrentFireTime = 0f;

	// Use this for initialization
	void Start () {
		
	}

	public override void Fire()
	{
		CurrentFireTime = 0f;
		this.transform.SetParent(null);
		this.Parent.Function.TrackEnemy = true;
		//BeingFired = true;
	}

	// Update is called once per frame
	void Update () {
		if(Parent.IsActive)
		{
			float moveSpeed = this.moveSpeed;

			if (CurrentFireTime <= FireTime)
			{
				CurrentFireTime += Time.deltaTime;
				moveSpeed = this.fireSpeed;
			}

			if (Parent.LockedTarget != null)
			{
				Vector3 relativePos = this.Parent.LockedTarget.position - this.transform.position;
				this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(relativePos), Time.deltaTime * lookAtSpeed);
				
				if (relativePos.magnitude > this.engagementDistance)
				{
					//do not engage
				} 
				else if (relativePos.magnitude < followDistance){
					this.transform.Translate(0, 0, Time.deltaTime * moveSpeed / 3f, Space.Self);
				}
				else 
				{
					//within engagement, outside optimum, move closer
					this.transform.Translate(0, 0, Time.deltaTime * moveSpeed, Space.Self);
				}
			}
		}
	}
}
