﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class bombVirus : VirusAI {

	public float lookAtSpeed = 2f;
	public float engagementDistance = 200f;
	public float moveSpeed = 20f;
	public playerHealth HealthScript;
	public Transform ExplosionPrefab;
	public Transform[] DestroyBeforeExplosion;
	public float ExplodingTime = 3f;

	private bool orbiting = true;

	public bombVirus()
	{
		this.Info = new ActorInfo()
		{ 
			Name = "Bomber",
			DamagePerHit = 5f,
			FireRate = 1f,
			HitPoints = 5f
		};
	}

	protected override void OnStart () {
		base.OnStart();
	}

	protected override void OnUpdate () {
		if (this.targetT != null)
			LookAtAndMoveToTarget();
	}

	private void LookAtAndMoveToTarget()
	{
		if (orbiting)
		{
			this.GetComponent<Orbit>().enabled = false;
			orbiting = false;
		}

		Vector3 relativePos = this.targetT.position - this.transform.position;
		this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(relativePos), Time.deltaTime * lookAtSpeed);
		
		//print (relativePos.magnitude);
		
		if (relativePos.magnitude > this.engagementDistance)
		{
			//do not engage
		} 
		else 
		{
			//within engagement, outside optimum, move closer
			this.transform.Translate(0, 0, Time.deltaTime * this.moveSpeed, Space.Self);
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
		s.TakeDamage(this.Info.DamagePerHit);

		GameObject.Instantiate(this.ExplosionPrefab, this.transform.position, Quaternion.identity);
		for(int i = 0; i < DestroyBeforeExplosion.Length; i++)
		{
			Transform t = DestroyBeforeExplosion[i];

			if (t != null)
				GameObject.Destroy(t.gameObject);
		}
		this.OnVirusDead();
		StartCoroutine(SelfDestruct());
	}

	private IEnumerator SelfDestruct()
	{		
		yield return new WaitForSeconds(this.ExplodingTime);
		GameObject.Destroy(this.gameObject);
	}
}
