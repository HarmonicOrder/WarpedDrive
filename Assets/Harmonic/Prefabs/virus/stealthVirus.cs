using UnityEngine;
using System.Collections;
using System;

public class stealthVirus : VirusAI, ILurker {

    public float lookAtSpeed = 2f;
    public float tooCloseDistance = 10f;
    public float engagementDistance = 200f;
    public float optimumRange = 60f;
    public float moveSpeed = 10f;

    public Transform LazerPrefab;
    public Transform LazerStart;
    /// <summary>
    /// The transform that turns off when lurking
    /// </summary>
    public Transform RenderBottleneck;
    public bool WaitUntilWholeSubnetIsClean = false;
    public float MinimumAmbushMinutes = 2f;
    public float MaximumAmbushMinutes = 5f;

    public override VirusAI.VirusType Type { get { return VirusAI.VirusType.Stealth; } }

    private OrbitAround OrbitScript;
    public bool IsLurking { get; set; }

	protected override void OnAwake()
	{
        this.IsLurking = true;
		base.OnAwake();
		this.Info = new ActorInfo()
		{
			Name = "Stealth Virus",
			Cooldown = 1f,
            HitChance = 20,
            BlockChance = 25
		};
		OrbitScript = this.GetComponent<OrbitAround>();
        this.RenderBottleneck.gameObject.SetActive(!IsLurking);
	}

    // Update is called once per frame
    protected override void OnUpdate () {
        if (this.IsLurking)
            return;

		if (this.targetT != null)
		{
			Vector3 relativePos = this.targetT.position - this.transform.position;
			
			if (relativePos.sqrMagnitude < this.engagementDistance * this.engagementDistance){
				FaceTarget(relativePos);

                if (!IsImmobile)
				    MoveToTarget(relativePos);
			}
		}
	}
	
	private void FaceTarget(Vector3 relativePos){
		this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(relativePos), InterruptTime.deltaTime * lookAtSpeed);
		float angle = Quaternion.Angle(this.transform.rotation, Quaternion.LookRotation(relativePos));
		
        if (!IsSandboxed)
		    PossiblyFireAtTarget(relativePos, angle);
	}
	
	private void PossiblyFireAtTarget(Vector3 relativePos, float angle)
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
		
		if ( (angle < 3f) && canFire)
		{
			Subroutine s = this.targetT.GetComponent<Subroutine>();
			if (s != null){
				FireAtEnemy(relativePos, s);
			}
		}
	}
	
	private void MoveToTarget(Vector3 relativePos){
		
		if (relativePos.magnitude > this.engagementDistance)
		{
			//do not engage
		} 
		else if (relativePos.magnitude < this.tooCloseDistance) 
		{
			//within too close, move away
			this.transform.Translate(0, 0, -InterruptTime.deltaTime * this.moveSpeed, Space.Self);
		} 
		else if ((relativePos.magnitude > this.optimumRange - 10f) && (relativePos.magnitude < this.optimumRange + 10f))
		{
			//within 10 of optimum, just fire
		} 
		else if (relativePos.magnitude > this.optimumRange)
		{
			//within engagement, outside optimum, move closer
			this.transform.Translate(0, 0, InterruptTime.deltaTime * this.moveSpeed, Space.Self);
		} 
		else 
		{
            //within engagement, within optimum, move away
            //this.transform.Translate(0, 0, -InterruptTime.deltaTime * this.moveSpeed, Space.Self);
        }
    }
	
	
	protected override void OnVirusDead()
	{
		if (this.GetComponent<OrbitAround>() != null)
		{
			this.GetComponent<OrbitAround>().RemoveOrbiter();
		}
		GameObject.Instantiate(this.ExplosionPrefab, this.transform.position, Quaternion.identity);
		base.OnVirusDead ();
		GameObject.Destroy(this.gameObject);
	}
	
	private float CooldownRemaining = 0f;
	private float LaserPersistTime = 1f;
	
	private void FireAtEnemy(Vector3 relativePos, Subroutine s)
	{		
		if (OrbitScript != null)
			OrbitScript.IsOrbiting = false;
		
		CooldownRemaining = this.Info.Cooldown;

		Transform t = (Transform)GameObject.Instantiate(this.LazerPrefab, this.LazerStart.position, this.LazerStart.rotation);
        LazerTorpedo lb = t.GetComponent<LazerTorpedo>();

        if (lb != null)
        {
            lb.origin = this;
            lb.FireTorpedo(s);
        }

        Physics.IgnoreCollision(this.GetComponent<Collider>(), t.GetComponent<Collider>(), true);

		StartCoroutine(this.WaitAndStopLaser());
	}
	
	
	private IEnumerator WaitAndStopLaser()
	{		
		yield return new WaitForSecondsInterruptTime(this.LaserPersistTime);
		
		if (OrbitScript != null)
			OrbitScript.IsOrbiting = true;
	}

    protected override void _OnDestroy()
    {
        base._OnDestroy();
    }

    private Coroutine lurkTimer;
    public void OnMachineClean()
    {
        if (WaitUntilWholeSubnetIsClean)
        {
            //do nothing
        }
        else
        {
            float randomWaitMinutes = UnityEngine.Random.Range(MinimumAmbushMinutes, MaximumAmbushMinutes);
            lurkTimer = StartCoroutine(WaitToUnlurk(randomWaitMinutes * 60));
        }
    }

    private IEnumerator WaitToUnlurk(float randomWaitSeconds)
    {
        print("stealth is waiting to lurk seconds of " + randomWaitSeconds);
        yield return new WaitForSecondsInterruptTime(randomWaitSeconds);
        print("time to unlurk ");
        Unlurk();
        lurkTimer = null;
    }

    public void OnSubnetSupposedlyClean()
    {
        if (lurkTimer != null){
            StopCoroutine(lurkTimer);
        }
        Unlurk();
    }

    private void Unlurk()
    {
        this.IsLurking = false;
        this.RenderBottleneck.gameObject.SetActive(true);
    }
}
