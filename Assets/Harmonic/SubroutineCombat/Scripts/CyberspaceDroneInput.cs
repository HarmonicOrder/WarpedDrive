﻿using UnityEngine;
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
	public Text TargetGuiText;
	public Transform PivotTransform;
	public MeshRenderer HitCrosshair;

	private Quaternion currentHeading;
	private Quaternion currentLookRotation;
	private Transform CurrentAlphaSubroutine;
	private VirusAI CurrentVirusLock;

	// Use this for initialization
	void Start () {
		//currentHeading = strategySphere.rotation;
		currentLookRotation = PivotTransform.localRotation;
		Cursor.lockState = CursorLockMode.Confined;
		Cursor.visible = false;
		HitCrosshair.enabled = false;
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

		bool LeftClick = CrossPlatformInputManager.GetButtonDown("Fire1");

		RaycastHit rayHit;
		if (Physics.Raycast(PivotTransform.position, PivotTransform.forward, out rayHit, Mathf.Infinity, 1 << LayerMask.NameToLayer("TargetRaycast")))
		{
			if (rayHit.collider != null)
			{
				//print (rayHit.collider.transform.parent.name);
				VirusAI v = (VirusAI)rayHit.collider.GetComponentInParent(typeof(VirusAI));
				if (v)
				{
					TargetGuiText.text = v.Info.GetTargetRichText();
					
					if (LeftClick)
					{
						if (CurrentVirusLock != null)
						{
							CurrentVirusLock.LockedOnGUI.enabled = false;
						}
						CurrentVirusLock = v;
						CurrentVirusLock.LockedOnGUI.enabled = true;
					}
				}
			}
			
			HitCrosshair.enabled = true;
		}
		else 
		{
			if (LeftClick && CurrentVirusLock != null)
			{
				CurrentVirusLock.LockedOnGUI.enabled = false;
				CurrentVirusLock = null;
			}
			HitCrosshair.enabled = false;
		}


		//bool  = CrossPlatformInputManager.GetButton("Jump");
		bool fireSubroutine = Input.GetKeyDown(KeyCode.Alpha1);
		if (fireSubroutine && CurrentVirusLock != null)
		{
			CurrentAlphaSubroutine = AlphaSubPrefab;
			CurrentAlphaSubroutine.GetComponent<Subroutine>().LockedTarget = CurrentVirusLock.transform;
			CurrentAlphaSubroutine.GetComponent<Subroutine>().Activate();
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
		PivotTransform.localRotation = Quaternion.Slerp(PivotTransform.localRotation, currentLookRotation, smoothing * Time.deltaTime);	
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
