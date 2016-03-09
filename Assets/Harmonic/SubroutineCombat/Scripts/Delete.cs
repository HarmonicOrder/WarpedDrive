using UnityEngine;
using System.Collections;

public class Delete : SubroutineFunction {

	public Transform lazerPrefab;
    
	private Transform leftGun;
	private Transform rightGun;

    public override float TracerSlowRange { get { return 30f; } }
    public override float TracerStopRange { get { return 20f; } }

    // Use this for initialization
    void Start () {
		this.Parent.Info.Cooldown = 1f;
        this.Parent.Info.HitChance += 15f;
        this.Parent.MyActorInfo.FunctionHitChance = 15f;
        this.Parent.Info.CoreCost += 1;
        this.LookAtSpeed = 5f;
		leftGun = HarmonicUtils.FindInChildren(this.Parent.FunctionRoot, "CrosshairLeft");
		rightGun = HarmonicUtils.FindInChildren(this.Parent.FunctionRoot, "CrosshairRight");
	}
	
	// Update is called once per frame
	void Update () {
        if (CanAttackEnemy())
        {
            FireAtEnemy(this.Parent.LockedTarget.position - this.transform.position, this.Parent.lockedMalware);
        }
	}

	private bool onPrimary = true;
	private void FireAtEnemy(Vector3 relativePos, IMalware m){
		isFiring = true;
        CooldownRemaining = this.Parent.Cooldown;

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
