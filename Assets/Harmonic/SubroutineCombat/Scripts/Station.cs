using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Station : SubroutineMovement {	


	public float TimeToHardpoint = 4f;
	public MeshRenderer ShieldRenderer;

	private bool BeingFired = false; //maybe add public getter
	private float CurrentFireTime = 0f;
	//private List<Transform> targetsInView = new List<Transform>();
	private float originalAlpha;

	private Vector3 firePosition;
	// Use this for initialization
	void Start () {
		this.originalAlpha = this.ShieldRenderer.material.color.a;

		this.ShieldRenderer.material.color = HarmonicUtils.ColorWithAlpha(this.ShieldRenderer.material.color, 0);
	}
	
	public override void Fire()
	{
		firePosition = this.Parent.StartingPosition.position;
		CurrentFireTime = 0f;
		this.transform.SetParent(null);
		BeingFired = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (BeingFired)
		{
			if (CurrentFireTime > TimeToHardpoint)
			{
				BeingFired = false;
				this.transform.position = Parent.LockedTarget.position;
				this.ShieldRenderer.material.color = HarmonicUtils.ColorWithAlpha(this.ShieldRenderer.material.color, this.originalAlpha);
				if (this.Parent.Function.GetType() == typeof(Terminate))
				{
					(this.Parent.Function as Terminate).TrackEnemy = true;
				}
			} else {
				this.transform.position = Vector3.Lerp(firePosition, Parent.LockedTarget.position, CurrentFireTime / TimeToHardpoint);
			}
			CurrentFireTime += Time.deltaTime;
		}
		
		if (!BeingFired && Parent.IsActive && (Parent.LockedTarget != null))
		{

		}
	}
}
