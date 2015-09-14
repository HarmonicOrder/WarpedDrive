using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.UI;
using Prime31.TransitionKit;

public class CyberspaceShellInput : MonoBehaviour {
	
	public EngineThruster thruster;
	public Image ThrottlePanel;

	public float maximumRollPerSecond = 10f;
	public float maximumPitchPerSecond = 10f;
	public float maximumYawPerSecond = 10f;

	public float smoothing = 5f;

	public float xSensitivity = 2f,
				 ySensitivity = 2f,
				 zSensitivity = 2f;
	public float moveSpeed = .25f;
	public float boostMultiplier = 2f;


	private bool boost = false, boostStart = false, boostStop = false;
	private float forward = 0f, vert, horz = 0f;
	private Quaternion currentHeading;
	private float roll = 0f;
	private Transform playerPrefab;
	private Color normalThrottlePanelColor;
	private short throttleBoostDirection;
	
	// Use this for initialization
	void Start () {
		playerPrefab = this.transform.parent;
		currentHeading = playerPrefab.rotation;
		normalThrottlePanelColor = ThrottlePanel.color;
		Cursor.lockState = CursorLockMode.Confined;
		Cursor.visible = false;
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

		boost = CrossPlatformInputManager.GetButton("Jump");
		boostStart = CrossPlatformInputManager.GetButtonDown("Jump");
		boostStop = CrossPlatformInputManager.GetButtonUp("Jump");
		
		vert = -CrossPlatformInputManager.GetAxis("Mouse Y") * ySensitivity;
		horz = CrossPlatformInputManager.GetAxis("Mouse X") * xSensitivity;
		roll = -CrossPlatformInputManager.GetAxis("Roll") * zSensitivity;

		//print(string.Format("Boosting:{0}, X:{1}, Y:{2}, Z:{3}", boost, horz, vert, roll));
		
		/*if (thruster != null){
			if (currentThrottle != 0f){
				if (boost) {
					thruster.ChangeEngineState(EngineThruster.EngineState.Boost);
				} else {
					thruster.ChangeEngineState(EngineThruster.EngineState.Engaged);
				}
			} else {
				thruster.ChangeEngineState(EngineThruster.EngineState.Idle);
			}
		}*/

		if (boostStart){
			throttleBoostDirection = 1;
		} else if (boostStop){
			throttleBoostDirection = -1;
		} else if ((throttleBoostDirection == -1) && (ThrottlePanel.color.Equals(normalThrottlePanelColor))){
			throttleBoostDirection = 0;
		}
		
		if (throttleBoostDirection == 1){
			ThrottlePanel.color = Color.Lerp(ThrottlePanel.color, Color.red, 4f*Time.deltaTime);
		} else if (throttleBoostDirection == -1){
			ThrottlePanel.color = Color.Lerp(ThrottlePanel.color, normalThrottlePanelColor, 4f*Time.deltaTime);
		}

		Vector3 translateV = new Vector3(
			CrossPlatformInputManager.GetAxis("Horizontal") * moveSpeed * (boost ? boostMultiplier : 1),
			CrossPlatformInputManager.GetAxis("Elevator") * moveSpeed * (boost ? boostMultiplier : 1),
			CrossPlatformInputManager.GetAxis("Vertical") * moveSpeed * (boost ? boostMultiplier : 1)
			);
		translateV.Normalize();

		if (translateV.magnitude > 0)
		{
			this.transform.parent.transform.Translate(translateV);	
		}
		
		BoundsCheckInput();
		
		currentHeading *= Quaternion.Euler(vert, 
		                                   horz, 
		                                   roll);
		
		playerPrefab.rotation = Quaternion.Slerp(playerPrefab.rotation, currentHeading, smoothing * Time.deltaTime);
	}
	
	private void BoundsCheckInput()
	{
		vert = BoundsCheckOneInput(vert, maximumPitchPerSecond);
		horz = BoundsCheckOneInput(horz, maximumYawPerSecond);
		roll = BoundsCheckOneInput(roll, maximumRollPerSecond);
	}
	
	private bool isNegative = false;
	private float BoundsCheckOneInput(float input, float max){
		isNegative = input < 0f;
		input = Mathf.Min( Mathf.Abs(input), max / Time.deltaTime);
		return (isNegative ? -input: input);
	}
}
