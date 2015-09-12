using UnityEngine;
using System.Collections;

public class CameraZoomToZoom : MonoBehaviour {

	public Transform MainCamera;
	public Transform ZoomCamera;
	public Transform ZoomToPosition;

	private short zoomDirection; //positive for zoom out negative for zoom in
	// Use this for initialization
	void Start () {
		ZoomCamera.gameObject.SetActive(false);	
	}

	public float ZoomTime = 1f;
	private float currentZoomTime = 0f;
	// Update is called once per frame
	void Update () {
		currentZoomTime += Time.deltaTime;

		if (zoomDirection > 0){
			ZoomCamera.position = Vector3.Lerp(MainCamera.position, ZoomToPosition.position, currentZoomTime);
			ZoomCamera.rotation = Quaternion.Slerp(MainCamera.rotation, ZoomToPosition.rotation, currentZoomTime);

			if (currentZoomTime > ZoomTime)
				zoomDirection = 0;
		} else if (zoomDirection < 0){
			ZoomCamera.position = Vector3.Lerp(ZoomToPosition.position, MainCamera.position, currentZoomTime);
			ZoomCamera.rotation = Quaternion.Slerp(ZoomToPosition.rotation, MainCamera.rotation, currentZoomTime);

			if (currentZoomTime > ZoomTime)
				OnFullyZoomedIn();
		}
	}

	public void Zoom(bool isZoomingOut){
		currentZoomTime = 0;
		if (isZoomingOut){
			zoomDirection = 1;
			ZoomCamera.gameObject.SetActive(true);
			MainCamera.gameObject.SetActive(false);
		} else {
			zoomDirection = -1;
		}
	}

	private void OnFullyZoomedIn(){
		zoomDirection = 0;
		MainCamera.gameObject.SetActive(true);
		ZoomCamera.gameObject.SetActive(false);
	}
}
