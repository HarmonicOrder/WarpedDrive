﻿using UnityEngine;
using System.Collections;

public class OrbitAround : SubroutineMovement {
	public Transform OrbitAnchor;
	public float orbitSpeed = 5f;
	public bool IsOrbiting = true;
    public bool CanOrbit = true;
	public Vector3 RotationAxis = Vector3.up;
	public CircleDraw OrbitRenderer;

	// Use this for initialization
	void Start () {
		if (OrbitRenderer != null)
		{
			OrbitRenderer.NumberOfOrbiters += 1;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if ((OrbitAnchor != null) && CanOrbit && IsOrbiting)
		{
			transform.RotateAround(OrbitAnchor.position, RotationAxis, InterruptTime.deltaTime * orbitSpeed);
		}
	}

	public void RemoveOrbiter()
	{
		IsOrbiting = false;
		if (OrbitRenderer != null)
		{
			OrbitRenderer.NumberOfOrbiters -= 1;
		}
	}
}
