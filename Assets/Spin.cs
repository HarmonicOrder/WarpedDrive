using UnityEngine;
using System.Collections;

public class Spin : MonoBehaviour {

	public float ZOffset = 1f;
	public float speed = 10f;


	private Vector3 position;
	void Start () {
		position = this.transform.position;
		position.z += ZOffset;
	}


	// Update is called once per frame
	void Update () {
		this.transform.RotateAround(position, Vector3.up, speed*Time.deltaTime);
	}
}
