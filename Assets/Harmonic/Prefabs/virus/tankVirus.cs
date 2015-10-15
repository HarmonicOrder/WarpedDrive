using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class tankVirus : VirusAI {

	public float lookAtSpeed = 2f;
	public float tooCloseDistance = 10f;
	public float engagementDistance = 200f;
	public float optimumRange = 60f;
	public float moveSpeed = 10f;
	
	public ParticleSystem PulseParticles;
	public ParticleSystem BurstParticles;
	public LineRenderer LineRenderer;

	public List<Transform> ArmorPlates;

	public override string DisplayNameSingular {get{return "Armored";}}
	public override string DisplayNamePlural {get{return "Armored";}}

	private OrbitAround OrbitScript;

	protected override void OnAwake()
	{
		base.OnAwake();
		this.Info = new ActorInfo()
		{
			Name = "Tank",
			DamagePerHit = 3f,
			FireRate = 3f,
			HitPoints = 5f,
			ArmorPoints = 6f
		};
		LineRenderer.SetVertexCount(0);
		OrbitScript = this.GetComponent<OrbitAround>();
	}
	
	// Update is called once per frame
	protected override void OnUpdate () {
		if (this.targetT != null)
		{
			Vector3 relativePos = this.targetT.position - this.transform.position;

			if (relativePos.sqrMagnitude < this.engagementDistance * this.engagementDistance){
				FaceTarget(relativePos);
			}
			//MoveToTarget(relativePos);
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
				s.TakeDamage(this.Info.DamagePerHit);
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

	private void FireAtEnemy(Vector3 relativePos)
	{
		isFiring = true;

		if (OrbitScript != null)
			OrbitScript.IsOrbiting = false;

		CooldownRemaining = this.Info.FireRate;

		this.LineRenderer.SetVertexCount(2);
		this.LineRenderer.SetPosition(0, Vector3.zero);
		this.LineRenderer.SetPosition(1, Vector3.forward * relativePos.magnitude);

		this.PulseParticles.startSpeed = relativePos.magnitude;
		this.PulseParticles.Play();

		this.BurstParticles.Emit(100);
		this.BurstParticles.transform.localPosition = Vector3.forward * relativePos.magnitude / 2;
		this.BurstParticles.transform.localScale = Vector3.right * relativePos.magnitude / 2;

		StartCoroutine(this.WaitAndStopLaser());
	}
	
	
	private IEnumerator WaitAndStopLaser()
	{		
		yield return new WaitForSeconds(this.LaserPersistTime);
		this.LineRenderer.SetVertexCount(0);		
		this.PulseParticles.Stop();
		isFiring = false;
		
		if (OrbitScript != null)
			OrbitScript.IsOrbiting = true;
	}

	
	protected override void OnTakeDamage(float damage, float armorPointsLost, float hitPointsLost)
	{
		if (armorPointsLost > 0f)
		{
			Transform killedArmorPlate;
			for (int i = 0; i < armorPointsLost; i++) {
				if (ArmorPlates.Count > 0)
				{
					killedArmorPlate = ArmorPlates[ArmorPlates.Count-1];
					ArmorPlates.Remove(killedArmorPlate);
					GameObject.Destroy(killedArmorPlate.gameObject);
				}
			}
		}
	}
}
