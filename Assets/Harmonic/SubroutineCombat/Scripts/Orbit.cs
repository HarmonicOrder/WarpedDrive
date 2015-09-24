using UnityEngine;
using System.Collections;

public class Orbit : MonoBehaviour {
	public Transform OrbitAnchor;
	public float orbitSpeed = 5f;
	public bool IsOrbiting = true;
	public Vector3 RotationAxis = Vector3.up;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if ((OrbitAnchor != null) && IsOrbiting)
		{
			transform.RotateAround(OrbitAnchor.position, RotationAxis, Time.deltaTime * orbitSpeed);
		}
	}
}
