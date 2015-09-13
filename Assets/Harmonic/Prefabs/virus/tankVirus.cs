using UnityEngine;
using System.Collections;

public class tankVirus : MonoBehaviour {

	public float lookAtSpeed = 2f;
	public float tooCloseDistance = 10f;
	public float engagementDistance = 200f;
	public float optimumRange = 60f;
	public float moveSpeed = 4f;

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
}
