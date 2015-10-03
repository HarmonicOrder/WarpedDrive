using UnityEngine;
using System.Collections;

public class Terminate : SubroutineFunction {

	public float LookAtSpeed = 2f;
	public float TerminateRange = 999f;
	public float LaserPersistTime = 1f;

	public ParticleSystem PulseParticles;
	public ParticleSystem BurstParticles;
	public LineRenderer TerminateLineRenderer;

	internal bool TrackEnemy = false;
	private Transform closestTransform;
	private VirusAI closestVirus;
	private float CooldownRemaining = -1f;
	private bool isFiringLaser = false;

	// Use this for initialization
	void Start () {
		TerminateLineRenderer.SetVertexCount(0);
		this.Parent.Info.DamagePerHit = 5f;
		this.Parent.Info.FireRate = 5f;
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

			if (TrackEnemy && !isFiringLaser)
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

	private void FindClosestTransform()
	{
		float closest = TerminateRange;
		foreach( VirusAI vai in ActiveSubroutines.VirusList)
		{
			if ((vai.transform.position - this.transform.position).magnitude < closest)
			{
				this.closestVirus = vai;
				this.closestTransform = vai.transform;
			}
		}
	}

	private void FireAtEnemy(Vector3 relativePos)
	{
		print (relativePos.magnitude);
		isFiringLaser = true;
		CooldownRemaining = this.Parent.Info.FireRate;
		this.closestVirus.TakeDamage(this.Parent.Info.DamagePerHit);
		this.TerminateLineRenderer.SetVertexCount(2);
		this.TerminateLineRenderer.SetPosition(0, Vector3.zero);
		this.TerminateLineRenderer.SetPosition(1, Vector3.forward * relativePos.magnitude);
		this.PulseParticles.startSpeed = relativePos.magnitude;
		this.PulseParticles.Play();
		this.BurstParticles.Emit(100);
		this.BurstParticles.transform.localPosition = Vector3.forward * relativePos.magnitude / 2;
		this.BurstParticles.transform.localScale = Vector3.right * relativePos.magnitude / 2;
		//this.BurstParticles.
		StartCoroutine(this.WaitAndStopLaser());
	}
	
	
	private IEnumerator WaitAndStopLaser()
	{		
		yield return new WaitForSeconds(this.LaserPersistTime);
		this.TerminateLineRenderer.SetVertexCount(0);		
		this.PulseParticles.Stop();
		isFiringLaser = false;
	}
}
