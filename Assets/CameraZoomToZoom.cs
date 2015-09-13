using UnityEngine;
using System.Collections;

public class CameraZoomToZoom : MonoBehaviour {

	public Transform MainCamera;
	public Transform ZoomCamera;
	private Vector3 ZoomFromPosition;
	private Vector3 ZoomToPosition;
	private Quaternion ZoomFromRotation;
	private Quaternion ZoomToRotation;


	public delegate void AfterZoomFinished();

	public AfterZoomFinished afterZoom;

	private short zoomDirection; //positive for zoom out negative for zoom in
	// Use this for initialization
	void Start () {
		ZoomFromPosition = MainCamera.position;
		ZoomFromRotation = MainCamera.rotation;
		ZoomToPosition = ZoomCamera.position;
		ZoomToRotation = ZoomCamera.rotation;
		ZoomCamera.gameObject.SetActive(false);	
	}

	public float ZoomTime = 2f;
	private float currentZoomTime = 0f;
	private float zoomPercentage {
		get {
			return currentZoomTime / ZoomTime;
		}
	}
	private float fovMax = -1;

	// Update is called once per frame
	void Update () {
		currentZoomTime += Time.deltaTime;
		if (zoomDirection > 0){
			DoZoom();

			if (currentZoomTime > ZoomTime){
				AfterZoom();
			}
		} else if (zoomDirection < 0){
			DoZoom();

			if (currentZoomTime > ZoomTime)
				ResetZoom();
		}
	}

	private void DoZoom()
	{
		ZoomCamera.position = Vector3.Lerp(ZoomFromPosition, ZoomToPosition, Mathfx.Hermite(0, 1, zoomPercentage));
		ZoomCamera.rotation = Quaternion.Slerp(ZoomFromRotation, ZoomToRotation, zoomPercentage);

		if (fovMax > 0f){
			ZoomCamera.GetComponent<Camera>().fieldOfView = 60 + ((60 - fovMax) * Mathf.Sin( Mathf.PI * zoomPercentage));
		} else {
			ZoomCamera.GetComponent<Camera>().fieldOfView = 60;
		}
	}

	private void Zoom(bool isZoomingOut){
		this.currentZoomTime = 0;
		if (isZoomingOut){
			this.zoomDirection = 1;
			this.ZoomCamera.gameObject.SetActive(true);
			this.MainCamera.gameObject.SetActive(false);
		} else {
			this.zoomDirection = -1;
		}
	}

	public void ZoomTo(bool isZoomingOut){
		if (isZoomingOut) {
			ZoomFromPosition = MainCamera.position;
			ZoomFromRotation = MainCamera.rotation;
			ZoomToPosition = ZoomCamera.parent.position;
			ZoomToRotation = ZoomCamera.parent.rotation;
		} else {
			ZoomFromPosition = ZoomCamera.parent.position;
			ZoomFromRotation = ZoomCamera.parent.rotation;
			ZoomToPosition = MainCamera.position;
			ZoomToRotation = MainCamera.rotation;
		}
		Zoom(isZoomingOut);
	}

	public void ZoomTo(Transform target, float overTime = 1f, float fovMax = -1){
		this.ZoomTime = overTime;
		this.ZoomFromPosition = ZoomCamera.position;
		this.ZoomFromRotation = ZoomCamera.rotation;
		this.ZoomToPosition = target.position;
		this.ZoomToRotation = target.rotation;
		this.fovMax = fovMax;
		this.Zoom(true);
	}

	private void ResetZoom(){
		this.zoomDirection = 0;
		this.MainCamera.gameObject.SetActive(true);
		this.ZoomCamera.gameObject.SetActive(false);
	}

	private void AfterZoom(){
		this.zoomDirection = 0;
		if (this.afterZoom != null){
			this.afterZoom();
		}
	}

	public void MainCameraToZoom(){
		this.MainCamera.position = this.ZoomCamera.position;
		this.MainCamera.rotation = this.ZoomCamera.rotation;
		this.MainCamera.gameObject.SetActive(true);
		this.ZoomCamera.gameObject.SetActive(false);
	}
}
