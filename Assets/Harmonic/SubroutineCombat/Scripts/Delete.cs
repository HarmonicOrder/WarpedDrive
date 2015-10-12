using UnityEngine;
using System.Collections;

public class Delete : SubroutineFunction {

	public Transform lazerPrefab;

	private float LookAtSpeed = 5f;
	private Transform leftGun;
	private Transform rightGun;

	// Use this for initialization
	void Start () {
		this.Parent.Info.DamagePerHit = 1f;
		this.Parent.Info.FireRate = 1f;	
		leftGun = HarmonicUtils.FindInChildren(this.Parent.FunctionRoot, "CrosshairLeft");
		rightGun = HarmonicUtils.FindInChildren(this.Parent.FunctionRoot, "CrosshairRight");
	}
	
	// Update is called once per frame
	void Update () {
		if (this.Parent.IsActive)
		{
			bool canFire = false;
			if (CooldownRemaining <= 0f)
			{
				canFire = true;
			}
			else
			{
				CooldownRemaining -= Time.deltaTime;
			}
			
			if (TrackEnemy && !isFiring)
			{
				FindClosestTransform();
				
				if (this.closestTransform != null)
				{
					Vector3 relativePos = this.closestTransform.position - this.transform.position;
					this.Parent.FunctionRoot.rotation = Quaternion.Slerp(this.Parent.FunctionRoot.rotation, Quaternion.LookRotation(relativePos), Time.deltaTime * LookAtSpeed);
					float angle = Quaternion.Angle(this.Parent.FunctionRoot.rotation, Quaternion.LookRotation(relativePos));
					
					if ( (angle < 5f) && canFire)
					{
						FireAtEnemy(this.closestTransform.position - this.transform.position);
					}
				}
			}
		}
	}

	private bool onPrimary = true;
	private void FireAtEnemy(Vector3 relativePos){
		isFiring = true;
		CooldownRemaining = this.Parent.Info.FireRate;

		Transform laserStart;
		if (onPrimary)
			laserStart = leftGun;
		else
			laserStart = rightGun;

		onPrimary = !onPrimary;

		Transform t = (Transform)Instantiate(lazerPrefab, laserStart.position, laserStart.rotation);

		LazerBeam b = t.GetComponent<LazerBeam>();
		if (b != null)
			b.damage = this.Parent.Info.DamagePerHit;

		Physics.IgnoreCollision(this.GetComponent<Collider>(), t.GetComponent<Collider>());

		StartCoroutine(this.WaitCooldown());
	}
}
