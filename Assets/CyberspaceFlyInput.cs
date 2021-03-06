﻿using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class CyberspaceFlyInput : MonoBehaviour {

	//public EngineThruster thruster;
	public float maximumRollPerSecond = 10f;
	public float maximumPitchPerSecond = 10f;
	public float maximumYawPerSecond = 10f;
	public float smoothing = 5f;
	public float xSensitivity = 2f,
				 ySensitivity = 2f,
				 zSensitivity = 2f;
	public float boostFOVDelta = 10f;
	public float timeToFullThrottle = 2f;
	
	private bool boost = false, boostStart = false, boostStop = false;
	private float forward = 0f, vert, horz = 0f;
	private float moveSpeed = 1f;
	private float boostMultiplier = 2f;
	private float currentSpeed = 0f;
	private float currentThrottle = 0f;
	private Quaternion currentHeading;
	private float roll = 0f;
	private Transform playerPrefab;
	private float normalFOV;
	private short fovDirection = 0; 

	// Use this for initialization
	void Start () {
		playerPrefab = this.transform.parent;
		currentHeading = playerPrefab.rotation;
		normalFOV = Camera.main.fieldOfView;
		Cursor.lockState = CursorLockMode.Confined;
	}

	private float GetThrottleAxis(){
		if (Input.GetKey(KeyCode.LeftShift)){
			return timeToFullThrottle * Time.deltaTime;
		} else if (Input.GetKey(KeyCode.LeftControl)){
			return -timeToFullThrottle * Time.deltaTime;
		}
		return 0;
	}

	// Update is called once per frame
	void Update () {
		boost = CrossPlatformInputManager.GetButton("Jump");
		boostStart = CrossPlatformInputManager.GetButtonDown("Jump");
		boostStop = CrossPlatformInputManager.GetButtonUp("Jump");

		vert = CrossPlatformInputManager.GetAxis("Mouse Y") * ySensitivity;
		horz = CrossPlatformInputManager.GetAxis("Mouse X") * xSensitivity;
		roll = CrossPlatformInputManager.GetAxis("Roll") * zSensitivity;

		forward = GetThrottleAxis();

		currentThrottle += forward;

		if (currentThrottle > 1f)
			currentThrottle = 1f;
		else if (currentThrottle < 0f)
			currentThrottle = 0f;

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
		currentSpeed = moveSpeed * Mathf.Abs(currentThrottle) * (boost ? boostMultiplier : 1);

		//fov widens when boosting, temporarily
		if (boostStart){
			fovDirection = 1;
		//if we're widening and we're at or past the wide fov, go negative
		} else if (boostStop){
			fovDirection = -1;
		//if we're narrowing and we're at or below the narrow fov, don't change fov
		} else if ((fovDirection == -1) && (Camera.main.fieldOfView <= normalFOV-1)){
			fovDirection = 0;
		}

		if (fovDirection != 0){
			Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, ((fovDirection == 1)? normalFOV+boostFOVDelta : normalFOV), 4f*Time.deltaTime);
		}


		if (currentThrottle > 0f){
			this.transform.parent.transform.Translate(Vector3.back * currentSpeed);	
		} else if (currentThrottle < 0f) {
			this.transform.parent.transform.Translate(Vector3.forward * currentSpeed);	
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
