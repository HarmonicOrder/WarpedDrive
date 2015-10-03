using UnityEngine;
using System.Collections;

public class Terminate : SubroutineFunction {

	public float LookAtSpeed = 2f;
	public float TerminateRange = 999f;

	internal bool TrackEnemy = false;
	private Transform closestEnemy;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (TrackEnemy)
		{
			FindClosestEnemy();

			if (this.closestEnemy != null)
			{
				Vector3 relativePos = this.closestEnemy.position - this.transform.position;
				this.Parent.FunctionRoot.rotation = Quaternion.Slerp(this.Parent.FunctionRoot.rotation, Quaternion.LookRotation(relativePos), Time.deltaTime * LookAtSpeed);
			}
		}
	}

	private void FindClosestEnemy()
	{
		float closest = TerminateRange;
		foreach( VirusAI vai in ActiveSubroutines.VirusList)
		{
			if ((vai.transform.position - this.transform.position).magnitude < closest)
			{
				this.closestEnemy = vai.transform;
			}
		}
	}
}
