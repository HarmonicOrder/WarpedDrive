using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;
using Prime31.TransitionKit;

public class HomeBase : MonoBehaviour {
	public Camera zoomCamera;
	public Texture2D cursorTexture;
	public Transform homeFiberGate;

	// Use this for initialization
	void Start () {
		Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);
	}

	private bool isZoomedOut = false;
	private SubnetSceneSelector currentHover;
	// Update is called once per frame
	void Update () {
		if (CrossPlatformInputManager.GetButtonDown("Zoom")){
			isZoomedOut = !isZoomedOut;
			this.transform.GetComponent<CameraZoomToZoom>().Zoom(isZoomedOut);
		}
		if (CrossPlatformInputManager.GetButtonDown("Jump")){
			var pixelater = new PixelateTransition()
			{
				nextScene = 2,
				finalScaleEffect = PixelateTransition.PixelateFinalScaleEffect.ToPoint,
				duration = 1.0f
			};
			TransitionKit.instance.transitionWithDelegate( pixelater );
		}

		if (isZoomedOut){
			if (CrossPlatformInputManager.GetButtonDown("Fire1") && (currentHover != null)){
				print ("firing");
				homeFiberGate.LookAt(currentHover.transform.position);
				this.transform.GetComponent<CameraZoomToZoom>().ZoomTo(homeFiberGate.FindChild("gatewayStartNode"), 5f);
			} else {
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
}
