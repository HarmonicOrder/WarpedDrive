using UnityEngine;
using System.Collections;

public class virus : VirusAI {
	
	public float lookAtSpeed = 2f;
	public float tooCloseDistance = 10f;
	public float engagementDistance = 200f;
	public float optimumRange = 60f;
	public float moveSpeed = 10f;

	public Transform LazerPrefab;
	public Transform LazerStart;

    public override VirusAI.VirusType Type { get { return VirusAI.VirusType.Virus; } }

    private OrbitAround OrbitScript;
	
	protected override void OnAwake()
	{
		base.OnAwake();
		this.Info = new ActorInfo()
		{
			Name = "Virus",
			FireRate = 1f,
            HitChance = 15,
            SaveChance = 15
        };
		OrbitScript = this.GetComponent<OrbitAround>();
	}
	
	// Update is called once per frame
	protected override void OnUpdate () {
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
            //this.transform.Translate(0, 0, -InterruptTime.InterruptTime * this.moveSpeed, Space.Self);
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
	
	private bool isFiring = false;
	private float CooldownRemaining = 0f;
	private float LaserPersistTime = 1f;
	
	private void FireAtEnemy(Vector3 relativePos, Subroutine s)
	{
		isFiring = true;
		
		if (OrbitScript != null)
			OrbitScript.IsOrbiting = false;
		
		CooldownRemaining = this.Info.FireRate;

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
		isFiring = false;
		
		if (OrbitScript != null)
			OrbitScript.IsOrbiting = true;
	}
}
