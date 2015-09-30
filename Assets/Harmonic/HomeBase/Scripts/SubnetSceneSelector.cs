using UnityEngine;
using System.Collections;

public class SubnetSceneSelector : MonoBehaviour {

	public MeshRenderer SubnetLabelRenderer;
	public Transform FiberGateway;

	// Use this for initialization
	void Start () {
		SubnetLabelRenderer.enabled = true;	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnHoverOn(){
		//todo: change color, not set renderer enabled
		SubnetLabelRenderer.enabled = true;
	}

	public void OnHoverOff(){
		SubnetLabelRenderer.enabled = true;
	}
}
