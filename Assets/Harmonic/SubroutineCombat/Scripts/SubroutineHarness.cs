﻿using UnityEngine;
using System.Collections;
using System;

public class SubroutineHarness : MonoBehaviour {

    public Transform InstantiatePrefab;
    public Transform LazerPrefab;

    public Transform Delete, Terminate, Corrupt;
    public Transform Tracer, Station;

    public Transform FunctionRoot, MovementRoot;

    private MonoBehaviour FunctionScript, MovementScript;

    public void Assign(SubroutineInfo mySI)
    {
        SetFunction(mySI.FunctionName);
        SetMovement(mySI.MovementName);

        AdjustSynergy();
    }

    /// <summary>
    /// Adjust parameters in specific function/movement situations
    /// </summary>
    private void AdjustSynergy()
    {
        if ((FunctionScript is Delete) && (MovementScript is Station))
        {
            //tighten up accuracy when stationary
            (FunctionScript as Delete).angleTightness = 1f;
        }
        else if ((FunctionScript is Terminate) && (MovementScript is Tracer))
        {
            //slow down when using "heavy weapon"
            (MovementScript as Tracer).moveSpeed = 10f;
        }
    }

    private void SetMovement(string movementName)
    {
        Transform t = null;
        switch(movementName)
        {
            case "Tracer":
                t = InstantiateSubcomponent(Tracer, this.MovementRoot, movementName);
                this.MovementScript = this.gameObject.AddComponent<Tracer>();
                break;
            case "Station":
                t = InstantiateSubcomponent(Station, this.MovementRoot, movementName);
                this.MovementScript = this.gameObject.AddComponent<Station>();
                (this.MovementScript as Station).InstantiatePrefab = InstantiatePrefab;
                break;
        }
    }

    private void SetFunction(string functionName)
    {
        Transform t = null;
        switch (functionName)
        {
            case "Delete":
                t = InstantiateSubcomponent(Delete, this.FunctionRoot, functionName);
                this.FunctionScript = this.gameObject.AddComponent<Delete>();
                (this.FunctionScript as Delete).lazerPrefab = LazerPrefab;
                break;
            case "Terminate":
                t = InstantiateSubcomponent(Terminate, this.FunctionRoot, functionName);
                this.FunctionScript = this.gameObject.AddComponent<Terminate>();
                break;
        }
    }

    private Transform InstantiateSubcomponent(Transform original, Transform parent, string name)
    {
        Transform t = Instantiate<Transform>(original);
        t.SetParent(parent);
        t.localPosition = Vector3.zero;
        t.localRotation = Quaternion.identity;
        t.name = name;
        return t;
    }
}
