﻿using UnityEngine;
using System.Collections;

public class Actor : MonoBehaviour {

	public ActorInfo Info {get;set;}

	void Awake () {
		print ("awake!");
		OnAwake();
	}
	
	protected virtual void OnAwake(){}

	// Use this for initialization
	void Start () {
		OnStart();
	}
	
	protected virtual void OnStart(){}
	
	// Update is called once per frame
	void Update () {
		OnUpdate();
	}
	
	protected virtual void OnUpdate(){}
}
