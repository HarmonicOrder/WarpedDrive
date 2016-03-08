using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SubroutineMovement : MonoBehaviour {

    public static float TimeToInstantiate = 2f;

    public Subroutine Parent {get;set;}

    protected float CurrentInstantiateTime = 0f;
    protected bool BeingFired { get; set; }

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
