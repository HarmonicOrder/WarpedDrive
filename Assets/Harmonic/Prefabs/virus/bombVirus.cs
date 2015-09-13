using UnityEngine;
using System.Collections;

public class bombVirus : MonoBehaviour {

	public float lookAtSpeed = 2f;
	public float engagementDistance = 200f;
	public float moveSpeed = 20f;

	private Transform playerT;
	// Use this for initialization
	void Start () {
		playerT = GameObject.FindGameObjectWithTag("Player").transform;
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 relativePos = this.playerT.position - this.transform.position;
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

	}
}
