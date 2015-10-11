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
	
	public override string DisplayNameSingular {get{return "Virus";}}
	public override string DisplayNamePlural {get{return "Virus";}}
	
	private Orbit OrbitScript;
	
	protected override void OnAwake()
	{
		base.OnAwake();
		this.Info = new ActorInfo()
		{
			Name = "Virus",
			DamagePerHit = 1f,
			FireRate = 1f,
			HitPoints = 5f,
			ArmorPoints = 0f
		};
		OrbitScript = this.GetComponent<Orbit>();
	}
	
	// Update is called once per frame
	protected override void OnUpdate () {
		if (this.targetT != null)
		{
			Vector3 relativePos = this.targetT.position - this.transform.position;
			
			if (relativePos.sqrMagnitude < this.engagementDistance * this.engagementDistance){
				FaceTarget(relativePos);
				MoveToTarget(relativePos);
			}
		}
	}
	
	private void FaceTarget(Vector3 relativePos){
		this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(relativePos), Time.deltaTime * lookAtSpeed);
		float angle = Quaternion.Angle(this.transform.rotation, Quaternion.LookRotation(relativePos));
		
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
			CooldownRemaining -= Time.deltaTime;
		}
		
		if ( (angle < 3f) && canFire)
		{
			Subroutine s = this.targetT.GetComponent<Subroutine>();
			if (s != null){
				FireAtEnemy(relativePos);
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
			this.transform.Translate(0, 0, -Time.deltaTime * this.moveSpeed, Space.Self);
		} 
		else if ((relativePos.magnitude > this.optimumRange - 10f) && (relativePos.magnitude < this.optimumRange + 10f))
		{
			//within 10 of optimum, just fire
		} 
		else if (relativePos.magnitude > this.optimumRange)
		{
			//within engagement, outside optimum, move closer
			this.transform.Translate(0, 0, Time.deltaTime * this.moveSpeed, Space.Self);
		} 
		else 
		{
			//within engagement, within optimum, move away
			//this.transform.Translate(0, 0, -Time.deltaTime * this.moveSpeed, Space.Self);
		}
	}
	
	
	protected override void OnVirusDead ()
	{
		if (this.GetComponent<Orbit>() != null)
		{
			this.GetComponent<Orbit>().RemoveOrbiter();
		}
		GameObject.Instantiate(this.ExplosionPrefab, this.transform.position, Quaternion.identity);
		base.OnVirusDead ();
		GameObject.Destroy(this.gameObject);
	}
	
	private bool isFiring = false;
	private float CooldownRemaining = 0f;
	private float LaserPersistTime = 1f;
	
	private void FireAtEnemy(Vector3 relativePos)
	{
		isFiring = true;
		
		if (OrbitScript != null)
			OrbitScript.IsOrbiting = false;
		
		CooldownRemaining = this.Info.FireRate;

		GameObject.Instantiate(this.LazerPrefab, this.LazerStart.position, this.LazerStart.rotation);

		StartCoroutine(this.WaitAndStopLaser());
	}
	
	
	private IEnumerator WaitAndStopLaser()
	{		
		yield return new WaitForSeconds(this.LaserPersistTime);
		isFiring = false;
		
		if (OrbitScript != null)
			OrbitScript.IsOrbiting = true;
	}
	
	
	protected override void OnTakeDamage(float damage, float armorPointsLost, float hitPointsLost)
	{
	}
}
