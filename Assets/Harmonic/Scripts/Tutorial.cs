using UnityEngine;
using System.Collections;
using System;

public class Tutorial {
    [Flags]
    public enum MeatspaceProgress
    {
        None = 0,
        EnteredCyberspace,
        EnteredCryosleep
    }

    [Flags]
    public enum CyberspaceProgress
    {
        None = 0,
        ZoomedOutOfCyberspace,
        SelectedSubnet,
        SelectedServer,
    }

    [Flags]
    public enum WorkstationProgress
    { 
        None = 0,
        EnteredWorkstation,
        CreatedNewSubroutine,
        AssignedStrategy,
        AssignedFunction,
        AssignedUpgrade
    }

    [Flags]
    public enum HackingProgress
    {
        None = 0,
        StrategyView,
        SwitchedMachines,
        TargetedMalware,
        CreatedTracer,
        CreatedStation,
        CleanedMachine
    }
}
