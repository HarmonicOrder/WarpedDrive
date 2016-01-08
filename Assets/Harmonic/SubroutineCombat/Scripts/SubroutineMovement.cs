using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SubroutineMovement : MonoBehaviour {

	public Subroutine Parent {get;set;}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public virtual void Fire(){}

    private static Dictionary<string, uint> RAMCosts = new Dictionary<string, uint>()
    {
        {"tracer", 2},
        {"station", 1 }
    };
    public static uint GetRAMCost(string name)
    {
        return RAMCosts[name.ToLower()];
    }
}
