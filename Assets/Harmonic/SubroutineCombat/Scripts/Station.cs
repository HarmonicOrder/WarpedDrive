using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Station : SubroutineMovement {	


	public float TimeToInstantiate = 4f;
	public Transform InstantiatePrefab;

	private bool BeingFired = false; //maybe add public getter
	private float CurrentInstantiateTime = 0f;

	//private List<Transform> targetsInView = new List<Transform>();
	private float originalAlpha;
	private Transform CurrentInstantiateCube;
	private MeshRenderer ShieldRenderer;

	// Use this for initialization
	void Start () {
        ShieldRenderer = this.transform.FindChild("MovementRoot/Station/StationHemisphere").GetComponent<MeshRenderer>();
		this.originalAlpha = this.ShieldRenderer.material.color.a;

		this.ShieldRenderer.material.color = HarmonicUtils.ColorWithAlpha(this.ShieldRenderer.material.color, 0);
        this.Parent.IsInvulnerable = true;
    }
	
	public override void Fire()
	{
		CurrentInstantiateTime = 0f;
		CurrentInstantiateCube = (Transform)GameObject.Instantiate(InstantiatePrefab, Parent.LockedTarget.position, Parent.LockedTarget.rotation);

		//ugly hack
		CurrentInstantiateCube.GetChild(0).GetComponent<Scaler>().duration = TimeToInstantiate;
		CurrentInstantiateCube.GetChild(0).GetComponent<Scaler>().scaleUp = true;

		this.transform.SetParent(null);
		this.transform.position = Parent.LockedTarget.position;
		this.transform.rotation = Parent.LockedTarget.rotation;
        Scaler s = this.gameObject.AddComponent<Scaler>();
		s.duration = TimeToInstantiate;
		s.ScaleAgain();

		BeingFired = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (BeingFired)
		{
			if (CurrentInstantiateTime > TimeToInstantiate)
			{
				BeingFired = false;
                this.Parent.IsInvulnerable = false;

                GameObject.Destroy(CurrentInstantiateCube.gameObject);

				this.ShieldRenderer.material.color = HarmonicUtils.ColorWithAlpha(this.ShieldRenderer.material.color, this.originalAlpha);
				
				this.Parent.Function.TrackEnemy = true;
			}
			CurrentInstantiateTime += Time.deltaTime;
		}
		
		if (!BeingFired && Parent.IsActive && (Parent.LockedTarget != null))
		{

		}
	}
}
