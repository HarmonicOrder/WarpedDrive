using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.UI;

public class SniperZoom : MonoBehaviour {
	public float zoomedFOV = 20f;

	// Use this for initialization
	void Start () {
		originalFOV = Camera.main.fieldOfView;	
	}

	private float originalFOV;

	void Update () {
		bool startZoom = CrossPlatformInputManager.GetButtonDown("Fire2");
		bool zoom = CrossPlatformInputManager.GetButton("Fire2");
		bool endZoom = CrossPlatformInputManager.GetButtonUp("Fire2");
		if (startZoom){
			Camera.main.fieldOfView = zoomedFOV;
		}
		if (endZoom)
		{
			Camera.main.fieldOfView = originalFOV;
		}
	}
}
