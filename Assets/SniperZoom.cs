using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.UI;

public class SniperZoom : MonoBehaviour {
	public float zoomedFOV = 20f;
	public float zoomedFOV2 = 10f;
	public float zoomedDelta = 50f;
	public float zoomedDelta2 = 100f;
	public Camera secondaryCamera;
	public float secondaryZoomedFOV = 30f;
	public float secondaryZoomedFOV2 = 15f;
	public float secondaryZoomRatio = .1f;
	public float MouseWheelSpeed = 5f;

	public enum ZoomLevel {None, FirstZoom, SecondZoom}
	public ZoomLevel CurrentZoomLevel = ZoomLevel.None;

	private float maxZoomOut;
	// Use this for initialization
	void Start () {
		originalFOV = Camera.main.fieldOfView;	
		originalSecondaryFOV = secondaryCamera.fieldOfView;
		maxZoomOut = Camera.main.fieldOfView;

		secondaryZoomRatio = originalFOV / originalSecondaryFOV;
	}

	private float originalFOV, originalSecondaryFOV;

	void Update () {
		bool startZoom = CrossPlatformInputManager.GetButtonDown("Fire2");
		if (startZoom)
		{
			//IncrementZoomFOV();
			IncrementZoom();
		}
		else {
			float wheel = CrossPlatformInputManager.GetAxis("Mouse ScrollWheel");

			if (wheel != 0f)
			{
				wheel *= MouseWheelSpeed * -1;

				if (((wheel < 0) && (Camera.main.fieldOfView > zoomedFOV2)) || //zoom in
				    ((wheel > 0) && (Camera.main.fieldOfView < maxZoomOut))) //zoom out
				{
					CurrentZoomLevel = ZoomLevel.None;
					
					Camera.main.fieldOfView += wheel;
					secondaryCamera.fieldOfView = Camera.main.fieldOfView * secondaryZoomRatio;
				}
			}
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

	private void IncrementZoomFOV()
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
