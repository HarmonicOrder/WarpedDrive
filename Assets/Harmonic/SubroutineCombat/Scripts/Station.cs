using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Station : SubroutineMovement {	


	public float TimeToInstantiate = 4f;
	public MeshRenderer ShieldRenderer;
	public Transform InstantiatePrefab;

	private bool BeingFired = false; //maybe add public getter
	private float CurrentInstantiateTime = 0f;

	//private List<Transform> targetsInView = new List<Transform>();
	private float originalAlpha;
	private Transform CurrentInstantiateCube;

	// Use this for initialization
	void Start () {
		this.originalAlpha = this.ShieldRenderer.material.color.a;

		this.ShieldRenderer.material.color = HarmonicUtils.ColorWithAlpha(this.ShieldRenderer.material.color, 0);
	}
	
	public override void Fire()
	{
		CurrentInstantiateTime = 0f;
		CurrentInstantiateCube = (Transform)GameObject.Instantiate(InstantiatePrefab, Parent.LockedTarget.position, Parent.LockedTarget.rotation);

		//prefab can handle this?
		//CurrentInstantiateCube.GetComponent<Scaler>().duration = TimeToInstantiate;

		this.transform.SetParent(null);
		this.transform.position = Parent.LockedTarget.position;
		this.transform.rotation = Parent.LockedTarget.rotation;
		this.gameObject.AddComponent<Scaler>();
		this.gameObject.GetComponent<Scaler>().duration = TimeToInstantiate;
		this.gameObject.GetComponent<Scaler>().scaleUp = true;
		BeingFired = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (BeingFired)
		{
			if (CurrentInstantiateTime > TimeToInstantiate)
			{
				BeingFired = false;

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
