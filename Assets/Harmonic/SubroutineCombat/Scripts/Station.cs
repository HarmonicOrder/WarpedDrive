using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Station : SubroutineMovement {
    
	public Transform InstantiatePrefab;
    
	//private List<Transform> targetsInView = new List<Transform>();
	private float originalAlpha;
	private Transform CurrentInstantiateCube;
	private MeshRenderer ShieldRenderer;

	// Use this for initialization
	void Start () {
        ShieldRenderer = this.transform.FindChild("MovementRoot/Station/StationHemisphere").GetComponent<MeshRenderer>();
		this.originalAlpha = this.ShieldRenderer.material.color.a;

		this.ShieldRenderer.material.color = HarmonicUtils.ColorWithAlpha(this.ShieldRenderer.material.color, 0);

        this.Parent.Info.BlockChance += 20f;
        this.Parent.MyActorInfo.MovementBlockChance = 20f;
    }
	
	public override void Fire()
	{
		CurrentInstantiateTime = 0f;
		CurrentInstantiateCube = (Transform)GameObject.Instantiate(InstantiatePrefab, Parent.LockedTarget.position, Parent.LockedTarget.rotation);
        CurrentInstantiateCube.SetParent(this.transform.parent);

		//ugly hack
		CurrentInstantiateCube.GetChild(0).GetComponent<Scaler>().duration = TimeToInstantiate;
		CurrentInstantiateCube.GetChild(0).GetComponent<Scaler>().scaleUp = true;
        
		this.transform.position = Parent.LockedTarget.position;
		this.transform.rotation = Parent.LockedTarget.rotation;
        this.Parent.LockedTarget = null;
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

                GameObject.Destroy(CurrentInstantiateCube.gameObject);

				this.ShieldRenderer.material.color = HarmonicUtils.ColorWithAlpha(this.ShieldRenderer.material.color, this.originalAlpha);
				
				this.Parent.Function.TrackEnemy = true;
			}
			CurrentInstantiateTime += InterruptTime.deltaTime;
		}
		
		//if (!BeingFired && Parent.IsActive && (Parent.LockedTarget != null))
		//{
  //          //noop
		//}
	}

    void OnDestroy()
    {
        if (CurrentInstantiateCube != null)
            GameObject.Destroy(CurrentInstantiateCube);
    }
}
