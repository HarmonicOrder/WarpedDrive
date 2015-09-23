using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.UI;
using Prime31.TransitionKit;

public class CyberspaceDroneInput : MonoBehaviour {
	public float smoothing = 5f;
	public Transform strategySphere;

	public float xSensitivity = 2f,
				 ySensitivity = 2f,
				 zSensitivity = 2f;
	public float moveSpeed = 1f;
	public bool invertY = true;

	private Quaternion currentHeading;
	private Quaternion currentLookRotation;
	
	// Use this for initialization
	void Start () {
		currentHeading = strategySphere.rotation;
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
		bool fireSubroutine = CrossPlatformInputManager.GetButtonDown("Jump");

		//todo: make movement more intuitive - something's off 
		float vert = CrossPlatformInputManager.GetAxis("Vertical") * ySensitivity;
		float horz = -CrossPlatformInputManager.GetAxis("Horizontal") * xSensitivity;
		//float roll = -CrossPlatformInputManager.GetAxis("Roll") * zSensitivity;

		if (invertY) 
		{
			vert = -vert;
		}

		float dX = 0f, dY = 0f, dRoll = 0f;
		if (vert != 0f)
			dY = vert * moveSpeed;
		if (horz != 0f)
			dX = horz * moveSpeed;
		//if (roll != 0f)
		//	dRoll = roll * moveSpeed;

		currentHeading *= Quaternion.Euler(vert, 
		                                   horz, 
		                                   0);
		strategySphere.rotation = Quaternion.Slerp(strategySphere.rotation, currentHeading, smoothing * Time.deltaTime);

		//todo: clamp rotation to certain bits
		float x = -CrossPlatformInputManager.GetAxis("Mouse Y") * xSensitivity;
		float y = CrossPlatformInputManager.GetAxis("Mouse X") * ySensitivity;
		float roll = -CrossPlatformInputManager.GetAxis("Roll") * zSensitivity;
		
		currentLookRotation *= Quaternion.Euler(x, 
		                                   y, 
		                                   roll);
		Camera.main.transform.localRotation = Quaternion.Slerp(Camera.main.transform.localRotation, currentLookRotation, smoothing * Time.deltaTime);


		
	}

	
	private bool isNegative = false;
	private float BoundsCheckOneInput(float input, float max){
		isNegative = input < 0f;
		input = Mathf.Min( Mathf.Abs(input), max / Time.deltaTime);
		return (isNegative ? -input: input);
	}
}
