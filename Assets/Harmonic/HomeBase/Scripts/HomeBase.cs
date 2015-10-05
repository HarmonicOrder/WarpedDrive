using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;
using Prime31.TransitionKit;

public class HomeBase : MonoBehaviour {
	public Camera zoomCamera;
	public Texture2D cursorTexture;
	public Transform homeFiberGate;
	public Transform WarpBubblePrefab;
	public Color _highlightColor;
	public static Color SubnetTextHighlightColor;

	private CameraZoomToZoom zoomScript;
	// Use this for initialization
	void Start () {
		SubnetTextHighlightColor = this._highlightColor;
		Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);
		zoomScript = this.transform.GetComponent<CameraZoomToZoom>();
		Cursor.visible = true;
	}

	private bool isZoomedOut = false, isSelectingServer = false, isWarping = false;
	private SubnetSceneSelector currentSubnet;
	private ServerSelector currentServer;
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
			if (CrossPlatformInputManager.GetButtonDown("Fire1")){
				if (currentServer != null)
					OnServerSelect();
				else if (currentSubnet != null)
					OnSubnetSelect();
			} else if (!isWarping){
				RaycastHit hit;
				Ray pointRay = zoomCamera.ScreenPointToRay(Input.mousePosition);

				if (Physics.Raycast(pointRay, out hit, Mathf.Infinity)){

					if (hit.collider != null){
						//print ("got a collider: "+hit.collider.gameObject.name);
						SubnetSceneSelector sss = hit.collider.GetComponent<SubnetSceneSelector>();

						if (sss != null){
							currentSubnet = sss;
							sss.OnHoverOn();
						}

						ServerSelector s = hit.collider.GetComponent<ServerSelector>();
						
						if (s != null){
							if (currentServer != null)
							{
								currentServer.OnHoverOff();
							}
							currentServer = s;
							s.OnHoverOver();
						}
					}
				} else {
					if (!isSelectingServer && (currentSubnet != null)){
						currentSubnet.OnHoverOff();
						currentSubnet = null;
					}
					if (currentServer != null){
						currentServer.OnHoverOff();
						currentServer = null;
					}
				}
			}
		}
	}

	private void OnSubnetSelect()
	{
		isSelectingServer = true;
		Transform zoomTo = currentSubnet.transform.FindChild("ZoomPosition");
		//zoomScript.afterZoom = new CameraZoomToZoom.AfterZoomFinished(AfterSubnetZoom);
		TextMesh[] meshes = currentSubnet.GetComponentsInChildren<TextMesh>();
		for (int i = 0; i < meshes.Length; i++) {
			meshes[i].transform.rotation = Quaternion.Euler(45f, 0f, 0f);
		}
		zoomScript.ZoomTo(zoomTo, 1f);
	}

	private void OnServerSelect()
	{
		if(FindNetworkLocation(currentServer.name))
		{
			isSelectingServer = false;
			isWarping = true;
			homeFiberGate.LookAt(currentSubnet.transform.position);
			zoomScript.afterZoom = new CameraZoomToZoom.AfterZoomFinished(AfterSubnetZoom);
			zoomScript.ZoomTo(homeFiberGate.FindChild("gatewayStartNode"), 2f);
		}
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
		zoomScript.ZoomTo(currentSubnet.transform.FindChild("gateway"), 6f, 80f);
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
			nextScene = NetworkMap.CurrentLocation.sceneIndex,
			finalScaleEffect = PixelateTransition.PixelateFinalScaleEffect.ToPoint,
			duration = 1.0f
		};
		TransitionKit.instance.transitionWithDelegate( pixelater );
	}

	private bool FindNetworkLocation(string name)
	{
		NetworkMap.CurrentLocation = NetworkMap.GetLocationByLocationName(name);
		return (NetworkMap.CurrentLocation != null);
	}
}
