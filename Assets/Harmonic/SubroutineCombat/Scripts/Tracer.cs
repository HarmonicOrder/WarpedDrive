using UnityEngine;
using System.Collections;

public class Tracer : SubroutineMovement {
	
	public float lookAtSpeed = 2f;
	public float engagementDistance = 200f;
	public float followDistance = 30f;
	public float moveSpeed = 20f;
	public float fireSpeed = 100f;
	public float FireDistance = 200f;
	public Transform targetT;


	private bool BeingFired = false; //maybe add public getter
	private float CurrentFireDistance = 0f;
	// Use this for initialization
	void Start () {
		
	}

	public override void Fire()
	{
		this.transform.SetParent(null);
		BeingFired = true;
	}

	// Update is called once per frame
	void Update () {
		if (BeingFired)
		{
			if (CurrentFireDistance >= FireDistance)
			{
				BeingFired = false;
			} else {
				this.transform.Translate(0, 0, Time.deltaTime * this.fireSpeed, Space.Self);
				CurrentFireDistance += Time.deltaTime * this.fireSpeed;
			}
		}

		if (!BeingFired && (targetT != null))
		{
			Vector3 relativePos = this.targetT.position - this.transform.position;
			this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(relativePos), Time.deltaTime * lookAtSpeed);
			
			//print (relativePos.magnitude);
			
			if (relativePos.magnitude > this.engagementDistance)
			{
				//do not engage
			} 
			else if (relativePos.magnitude < followDistance){
				this.transform.Translate(0, 0, Time.deltaTime * this.moveSpeed / 2f, Space.Self);
			}
			else 
			{
				//within engagement, outside optimum, move closer
				this.transform.Translate(0, 0, Time.deltaTime * this.moveSpeed, Space.Self);
			}
		}
	}
}
