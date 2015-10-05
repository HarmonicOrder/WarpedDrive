using UnityEngine;
using System.Collections;

public class tankVirus : VirusAI {

	public float lookAtSpeed = 2f;
	public float tooCloseDistance = 10f;
	public float engagementDistance = 200f;
	public float optimumRange = 60f;
	public float moveSpeed = 10f;

	public override string DisplayNameSingular {get{return "Armored";}}
	public override string DisplayNamePlural {get{return "Armored";}}

	protected override void OnAwake()
	{
		base.OnAwake();
		this.Info = new ActorInfo()
		{
			Name = "Tank",
			DamagePerHit = 3f,
			FireRate = 1f,
			HitPoints = 5f,
			ArmorPoints = 5f
		};
	}
	
	// Update is called once per frame
	protected override void OnUpdate () {
		if (this.targetT != null)
		{
			Vector3 relativePos = this.targetT.position - this.transform.position;
			FaceTarget(relativePos);
			//MoveToTarget(relativePos);
		}
	}

	private void FaceTarget(Vector3 relativePos){
		this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(relativePos), Time.deltaTime * lookAtSpeed);
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
		StartCoroutine(SelfDestruct());
	}
	
	private IEnumerator SelfDestruct()
	{		
		yield return new WaitForSeconds(1f);
		GameObject.Destroy(this.gameObject);
	}
}
