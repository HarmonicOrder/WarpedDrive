using UnityEngine;
using System.Collections;

public class SubnetSceneSelector : MonoBehaviour {

	public MeshRenderer SubnetLabelRenderer;
	public Transform FiberGateway;

	// Use this for initialization
	void Start () {
		SubnetLabelRenderer.enabled = false;	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnHoverOn(){
		SubnetLabelRenderer.enabled = true;
	}

	public void OnHoverOff(){
		SubnetLabelRenderer.enabled = false;
	}
}
