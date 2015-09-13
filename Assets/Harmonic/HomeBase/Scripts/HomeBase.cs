using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;
using Prime31.TransitionKit;

public class HomeBase : MonoBehaviour {
	public Camera zoomCamera;
	public Texture2D cursorTexture;
	public Transform homeFiberGate;
	public Transform WarpBubblePrefab;

	private CameraZoomToZoom zoomScript;

	// Use this for initialization
	void Start () {
		Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);
		zoomScript = this.transform.GetComponent<CameraZoomToZoom>();
	}

	private bool isZoomedOut = false, isWarping = false;
	private SubnetSceneSelector currentHover;
	// Update is called once per frame
	void Update () {
		if (CrossPlatformInputManager.GetButtonDown("Zoom")){
			isZoomedOut = !isZoomedOut;
			this.transform.GetComponent<CameraZoomToZoom>().ZoomTo(isZoomedOut);
		}
		if (CrossPlatformInputManager.GetButtonDown("Jump")){
			TransitionToBattlespace();
		}

		if (isZoomedOut){
			if (CrossPlatformInputManager.GetButtonDown("Fire1") && (currentHover != null)){
				OnSubnetSelect();
			} else if (!isWarping){
				RaycastHit hit;
				Ray pointRay = zoomCamera.ScreenPointToRay(Input.mousePosition);
				//Debug.DrawRay(pointRay.GetPoint(0), pointRay.direction, Color.red, 99f);
				if (Physics.Raycast(pointRay, out hit, Mathf.Infinity)){
					if (hit.collider != null){
						SubnetSceneSelector sss = hit.collider.GetComponent<SubnetSceneSelector>();

						if (sss != null){
							currentHover = sss;
							sss.OnHoverOn();
						}
					}
				} else {
					if (currentHover != null){
						currentHover.OnHoverOff();
						currentHover = null;
					}
				}
			}
		}
	}

	private void OnSubnetSelect()
	{
		isWarping = true;
		homeFiberGate.LookAt(currentHover.transform.position);
		zoomScript.afterZoom = new CameraZoomToZoom.AfterZoomFinished(AfterSubnetZoom);
		zoomScript.ZoomTo(homeFiberGate.FindChild("gatewayStartNode"), 2f);
	}

	private void AfterSubnetZoom(){
		StartCoroutine(BeforeWarpStart());
	}

	private IEnumerator BeforeWarpStart(){
		yield return new WaitForSeconds(1f);
		OnWarpStart();
	}

	private Transform warpBubble;
	public void OnWarpStart(){
		warpBubble = (Transform)Instantiate(WarpBubblePrefab, zoomCamera.transform.position, zoomCamera.transform.rotation);
		warpBubble.parent = zoomCamera.transform;
		zoomScript.afterZoom = new CameraZoomToZoom.AfterZoomFinished(OnWarpEnd);
		zoomScript.ZoomTo(currentHover.transform.FindChild("gateway"), 6f, 80f);
	}

	public void OnWarpEnd(){
		GameObject.Destroy(warpBubble.gameObject);
		isWarping = false;
		zoomScript.MainCameraToZoom();
		TransitionToBattlespace();
	}

	private void TransitionToBattlespace()
	{
		var pixelater = new PixelateTransition()
		{
			nextScene = 2,
			finalScaleEffect = PixelateTransition.PixelateFinalScaleEffect.ToPoint,
			duration = 1.0f
		};
		TransitionKit.instance.transitionWithDelegate( pixelater );
	}
}
