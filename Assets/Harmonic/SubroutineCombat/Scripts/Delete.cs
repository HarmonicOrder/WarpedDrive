using UnityEngine;
using System.Collections;

public class Delete : SubroutineFunction {

	public Transform lazerPrefab;
    public float angleTightness = 5f;

	private float LookAtSpeed = 5f;
	private Transform leftGun;
	private Transform rightGun;

    public override float TracerSlowRange { get { return 30f; } }
    public override float TracerStopRange { get { return 20f; } }

    // Use this for initialization
    void Start () {
		this.Parent.Info.FireRate = 1f;
        this.Parent.Info.HitChance += 15f;
        this.Parent.Info.CoreCost += 1;
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
				CooldownRemaining -= InterruptTime.deltaTime;
			}
			
			if (TrackEnemy && !isFiring)
			{
				
				if (this.Parent.LockedTarget != null)
				{
					Vector3 relativePos = this.Parent.LockedTarget.position - this.transform.position;
					this.Parent.FunctionRoot.rotation = Quaternion.Slerp(this.Parent.FunctionRoot.rotation, Quaternion.LookRotation(relativePos), InterruptTime.deltaTime * LookAtSpeed);
					float angle = Quaternion.Angle(this.Parent.FunctionRoot.rotation, Quaternion.LookRotation(relativePos));
					
					if ( (angle < angleTightness) && canFire)
					{
						FireAtEnemy(this.Parent.LockedTarget.position - this.transform.position, this.Parent.lockedMalware);
					}
				}
			}
		}
	}

	private bool onPrimary = true;
	private void FireAtEnemy(Vector3 relativePos, IMalware m){
		isFiring = true;
		CooldownRemaining = this.Parent.Info.FireRate;

		Transform laserStart;
		if (onPrimary)
			laserStart = leftGun;
		else
			laserStart = rightGun;

		onPrimary = !onPrimary;

		Transform t = (Transform)Instantiate(lazerPrefab, laserStart.position, laserStart.rotation);

		LazerTorpedo b = t.GetComponent<LazerTorpedo>();
        if (b != null)
        {
            b.origin = this.Parent;
            b.FireTorpedo(m);
        }

		Physics.IgnoreCollision(this.GetComponent<Collider>(), t.GetComponent<Collider>());

		StartCoroutine(this.WaitCooldown());
	}
}
