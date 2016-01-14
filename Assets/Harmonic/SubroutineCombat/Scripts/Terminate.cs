using UnityEngine;
using System.Collections;

public class Terminate : SubroutineFunction {

	public float LookAtSpeed = 2f;
	public float LaserPersistTime = .5f;

	public ParticleSystem PulseParticles;
	public ParticleSystem BurstParticles;
	public LineRenderer TerminateLineRenderer;

	// Use this for initialization
	void Start () {
        this.TerminateLineRenderer = this.transform.Find("FunctionRoot/Terminate").GetComponent<LineRenderer>();
        this.PulseParticles = this.transform.Find("FunctionRoot/Terminate/PulseParticles").GetComponent<ParticleSystem>();
        this.BurstParticles = this.transform.Find("FunctionRoot/Terminate/BurstParticles").GetComponent<ParticleSystem>();
        TerminateLineRenderer.SetVertexCount(0);
		this.Parent.Info.DamagePerHit = 5f;
		this.Parent.Info.FireRate = 5f;
		this.Parent.Info.CoreCost += 2;
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
				if (this.Parent.LockedTarget != null)
				{
					Vector3 relativePos = this.Parent.LockedTarget.position - this.transform.position;
					this.Parent.FunctionRoot.rotation = Quaternion.Slerp(this.Parent.FunctionRoot.rotation, Quaternion.LookRotation(relativePos), Time.deltaTime * LookAtSpeed);
					float angle = Quaternion.Angle(this.Parent.FunctionRoot.rotation, Quaternion.LookRotation(relativePos));

					if ( (angle < 5f) && canFire)
					{
						FireAtEnemy(this.Parent.LockedTarget.position - this.transform.position);
					}
				}
			}
		}
	}

	private void FireAtEnemy(Vector3 relativePos)
	{
		isFiring = true;
		CooldownRemaining = this.Parent.Info.FireRate;
		this.Parent.lockedMalware.TakeDamage(this.Parent.Info.DamagePerHit);
		this.TerminateLineRenderer.SetVertexCount(2);
		this.TerminateLineRenderer.SetPosition(0, Vector3.zero);
		this.TerminateLineRenderer.SetPosition(1, Vector3.forward * relativePos.magnitude);
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
		this.TerminateLineRenderer.SetVertexCount(0);		
		this.PulseParticles.Stop();
		isFiring = false;
	}
}
