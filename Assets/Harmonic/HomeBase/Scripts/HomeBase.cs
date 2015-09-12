using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;
using Prime31.TransitionKit;

public class HomeBase : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

	private bool isZoomedOut = false;
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
	}
}
