using UnityEngine;
using System.Collections;

public class SlavedRotation : MonoBehaviour {

	public Transform Master;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		this.transform.rotation = Master.rotation;
	}
}
