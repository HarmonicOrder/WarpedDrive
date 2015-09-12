using UnityEngine;
using System.Collections;

public class CameraZoomToZoom : MonoBehaviour {

	public Transform MainCamera;
	public Transform ZoomCamera;
	public Transform ZoomFromPosition;
	public Transform ZoomToPosition;

	private short zoomDirection; //positive for zoom out negative for zoom in
	// Use this for initialization
	void Start () {
		ZoomFromPosition = MainCamera.transform;
		ZoomCamera.gameObject.SetActive(false);	
	}

	public float ZoomTime = 2f;
	private float currentZoomTime = 0f;
	private float zoomPercentage {
		get {
			return currentZoomTime / ZoomTime;
		}
	}

	// Update is called once per frame
	void Update () {
		currentZoomTime += Time.deltaTime;

		if (zoomDirection > 0){
			ZoomCamera.position = Vector3.Lerp(ZoomFromPosition.position, ZoomToPosition.position, zoomPercentage);
			ZoomCamera.rotation = Quaternion.Slerp(ZoomFromPosition.rotation, ZoomToPosition.rotation, zoomPercentage);

			if (currentZoomTime > ZoomTime)
				zoomDirection = 0;
		} else if (zoomDirection < 0){
			ZoomCamera.position = Vector3.Lerp(ZoomToPosition.position, ZoomFromPosition.position, zoomPercentage);
			ZoomCamera.rotation = Quaternion.Slerp(ZoomToPosition.rotation, ZoomFromPosition.rotation, zoomPercentage);

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

	public void ZoomTo(Transform target, float overTime = 1f){
		ZoomTime = overTime;
		ZoomFromPosition = ZoomCamera;
		ZoomToPosition = target;
		Zoom(true);
	}

	private void OnFullyZoomedIn(){
		zoomDirection = 0;
		MainCamera.gameObject.SetActive(true);
		ZoomCamera.gameObject.SetActive(false);
	}
}
