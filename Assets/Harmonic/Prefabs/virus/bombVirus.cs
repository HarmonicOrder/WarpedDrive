using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class bombVirus : VirusAI {

	public float lookAtSpeed = 45f;
	public float engagementDistance = 200f;
	public float moveSpeed = 20f;
	public playerHealth HealthScript;
	public float ExplodingTime = 3f;

    public override VirusType Type { get { return VirusType.Bomb; } }
	private bool orbiting = true;

	protected override void OnAwake()
	{
		base.OnAwake();
		this.Info = new ActorInfo()
		{ 
			Name = "Bomber",
			FireRate = 1f,
            HitChance = 100,
            SaveChance = 0
		};
	}

	protected override void OnUpdate () {
		if (!IsImmobile && (this.targetT != null))
			LookAtAndMoveToTarget();
	}

	private void LookAtAndMoveToTarget()
	{
		if (orbiting)
		{
			this.GetComponent<OrbitAround>().RemoveOrbiter();
			orbiting = false;
		}

		Vector3 relativePos = this.targetT.position - this.transform.position;
		this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(relativePos), InterruptTime.deltaTime * lookAtSpeed);
		
		if (relativePos.magnitude > this.engagementDistance)
		{
			//do not engage
		}
		else 
		{
			//within engagement, outside optimum, move closer
			this.transform.Translate(0, 0, InterruptTime.deltaTime * this.moveSpeed, Space.Self);
		} 
		
		if (relativePos.magnitude < 1)
		{
			Subroutine targetSub = this.targetT.GetComponent<Subroutine>();
			if (targetSub != null)
			{
				OnCollideWithSubroutine(targetSub);
			}
		}

	}

	private void OnCollideWithSubroutine(Subroutine s)
	{
        this.DoAttack(s);
		this.OnVirusDead();
	}

	protected override void OnVirusDead()
	{
		GameObject.Instantiate(this.ExplosionPrefab, this.transform.position, Quaternion.identity);
        base.OnVirusDead();
        GameObject.Destroy(this.gameObject);
    }
}
