using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.UI;
using Prime31.TransitionKit;

public class CyberspaceDroneInput : MonoBehaviour {
	public float smoothing = 5f;
	public Transform strategyYawSphere;
	public Transform strategyPitchSphere;
	public float xSensitivity = 2f,
				 ySensitivity = 2f,
				 zSensitivity = 2f;
	public float moveSpeed = 1f;
	public bool invertY = true;
	public Transform AlphaSubPrefab;

	private Quaternion currentHeading;
	private Quaternion currentLookRotation;
	private Transform CurrentAlphaSubroutine;

	// Use this for initialization
	void Start () {
		//currentHeading = strategySphere.rotation;
		currentLookRotation = Camera.main.transform.localRotation;
		Cursor.lockState = CursorLockMode.Confined;
		//Cursor.visible = false;
	}

	// Update is called once per frame
	void Update () {
		
		if (CrossPlatformInputManager.GetButtonDown("Cancel")){
			var pixelater = new PixelateTransition()
			{
				finalScaleEffect = PixelateTransition.PixelateFinalScaleEffect.ToPoint,
				duration = 1.0f
			};
			pixelater.nextScene = 0;
			TransitionKit.instance.transitionWithDelegate( pixelater );
			return;
		}

		//bool  = CrossPlatformInputManager.GetButton("Jump");
		bool fireSubroutine = Input.GetKeyDown(KeyCode.Alpha1);
		if (fireSubroutine)
		{
			CurrentAlphaSubroutine = AlphaSubPrefab;
			CurrentAlphaSubroutine.GetComponent<Tracer>().Fire();
		}

		float horz = CrossPlatformInputManager.GetAxis("Vertical") * ySensitivity;
		float vert = -CrossPlatformInputManager.GetAxis("Horizontal") * xSensitivity;

		if (invertY) 
		{
			vert = -vert;
		}

		float dX = 0f, dY = 0f;
		if (vert != 0f)
			dY = vert * moveSpeed;
		if (horz != 0f)
			dX = horz * moveSpeed;

		SlerpRotate(strategyPitchSphere, dX, 0, 91f);

		SlerpRotate(strategyYawSphere, 0, dY);

		float x = -CrossPlatformInputManager.GetAxis("Mouse Y") * xSensitivity;
		float y = CrossPlatformInputManager.GetAxis("Mouse X") * ySensitivity;
		float roll = -CrossPlatformInputManager.GetAxis("Roll") * zSensitivity;
		
		currentLookRotation *= Quaternion.Euler(x, 
		                                   y, 
		                                   roll);
		Camera.main.transform.localRotation = Quaternion.Slerp(Camera.main.transform.localRotation, currentLookRotation, smoothing * Time.deltaTime);		
	}

	private void SlerpRotate(Transform target, float deltaX, float deltaY, float? xRange = null)
	{
		Quaternion newRotation = target.localRotation * Quaternion.Euler(deltaX, deltaY, 0);
		if (xRange.HasValue && (Quaternion.Angle(Quaternion.identity, newRotation) > xRange))
		{
			//try and get closer to the max
			if (target.localRotation.x < xRange)
				SlerpRotate(target, deltaX / 2, deltaY, xRange);
			return;
		}

		target.localRotation = Quaternion.Slerp(target.localRotation, newRotation, smoothing * Time.deltaTime);
	}

}
