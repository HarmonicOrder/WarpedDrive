using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Station : SubroutineMovement {
	
	public float lookAtSpeed = 2f;
	public float engagementDistance = 200f;
	public float followDistance = 30f;
	public float moveSpeed = 20f;
	public float fireSpeed = 100f;
	public float FireDistance = 200f;
	public float TimeToHardpoint = 4f;
	
	private bool BeingFired = false; //maybe add public getter
	private float CurrentFireTime = 0f;
	private List<Transform> targetsInView = new List<Transform>();

	// Use this for initialization
	void Start () {
		
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
			if (CurrentFireTime < TimeToHardpoint)
			{
				BeingFired = false;
				this.transform.position = Parent.LockedTarget.position;
			} else {
				this.transform.position = Vector3.Lerp(this.transform.position, Parent.LockedTarget.position, CurrentFireTime / TimeToHardpoint);
			}
		}
		
		if (!BeingFired && Parent.IsActive && (Parent.LockedTarget != null))
		{

		}
	}
}
