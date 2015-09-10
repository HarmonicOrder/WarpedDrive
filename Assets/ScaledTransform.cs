using UnityEngine;
using System.Collections;

public class ScaledTransform : MonoBehaviour {

	public Transform Master;
	public float ScaleMovement = 9999f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		this.transform.localPosition = new Vector3(
			Master.position.x / ScaleMovement,
			Master.position.y / ScaleMovement,
			Master.position.z / ScaleMovement
			);
	}
}
