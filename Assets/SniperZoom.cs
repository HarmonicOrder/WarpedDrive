using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.UI;

public class SniperZoom : MonoBehaviour {
	public float zoomedFOV = 20f;
	public float zoomedFOV2 = 10f;
	public Camera secondaryCamera;
	public float secondaryZoomedFOV = 30f;
	public float secondaryZoomedFOV2 = 15f;

	public enum ZoomLevel {None, FirstZoom, SecondZoom}
	public ZoomLevel CurrentZoomLevel = ZoomLevel.None;

	// Use this for initialization
	void Start () {
		originalFOV = Camera.main.fieldOfView;	
		originalSecondaryFOV = secondaryCamera.fieldOfView;
	}

	private float originalFOV, originalSecondaryFOV;

	void Update () {
		bool startZoom = CrossPlatformInputManager.GetButtonDown("Fire2");
		if (startZoom)
		{
			IncrementZoom();
		}
	}

	private void IncrementZoom()
	{
		switch(CurrentZoomLevel)
		{
		case ZoomLevel.None:
			CurrentZoomLevel = ZoomLevel.FirstZoom;
			Camera.main.fieldOfView = zoomedFOV;
			secondaryCamera.fieldOfView = secondaryZoomedFOV;
			break;
		case ZoomLevel.FirstZoom:
			CurrentZoomLevel = ZoomLevel.SecondZoom;
			Camera.main.fieldOfView = zoomedFOV2;
			secondaryCamera.fieldOfView = secondaryZoomedFOV2;
			break;
		case ZoomLevel.SecondZoom:
			CurrentZoomLevel = ZoomLevel.None;
			Camera.main.fieldOfView = originalFOV;
			secondaryCamera.fieldOfView = originalSecondaryFOV;
			break;
		}
	}
}
