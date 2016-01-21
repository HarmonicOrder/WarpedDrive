using UnityEngine;
using System.Collections;
using System;

public class Tutorial {

    public static bool HasDone(Enum check)
    {
        if (HarmonicSerialization.Instance.CurrentSave != null)
        {
            if (HarmonicSerialization.Instance.CurrentSave.HideTutorial)
                return true;
            else
            {
                if (check is MeatspaceProgress)
                    return (HarmonicSerialization.Instance.CurrentSave.MeatspaceProgression & ((MeatspaceProgress)check)) != 0;
                else if (check is CyberspaceProgress)
                    return (HarmonicSerialization.Instance.CurrentSave.CyberspaceProgression & ((CyberspaceProgress)check)) != 0;
                else if (check is WorkstationProgress)
                    return (HarmonicSerialization.Instance.CurrentSave.WorkstationProgression & ((WorkstationProgress)check)) != 0;
                else if (check is HackingProgress)
                    return (HarmonicSerialization.Instance.CurrentSave.HackingProgression & ((HackingProgress)check)) != 0;

                UnityEngine.Debug.LogWarning("flag enum is none of any progression enums!");
                return false;
            }
        }
        else
        {
            UnityEngine.Debug.LogWarning("no current save to check enums against!");
            return false;
        }
    }

    public static void JustDid(Enum aThing)
    {
        if (HarmonicSerialization.Instance.CurrentSave != null)
        {
            if (aThing is MeatspaceProgress)
                HarmonicSerialization.Instance.CurrentSave.MeatspaceProgression |= ((MeatspaceProgress)aThing);
            else if (aThing is CyberspaceProgress)
                HarmonicSerialization.Instance.CurrentSave.CyberspaceProgression |= ((CyberspaceProgress)aThing);
            else if (aThing is WorkstationProgress)
                HarmonicSerialization.Instance.CurrentSave.WorkstationProgression |= ((WorkstationProgress)aThing);
            else if (aThing is HackingProgress)
                HarmonicSerialization.Instance.CurrentSave.HackingProgression |= ((HackingProgress)aThing);
            else
                UnityEngine.Debug.LogWarning("flag enum to set is none of any progression enums!");
        }
        else
        {
            UnityEngine.Debug.LogWarning("no current save to set enum on!");
        }

    }

    [Flags]
    public enum MeatspaceProgress
    {
        None = 0,
        EnteredCyberspace,
        EnteredCryosleep,
        PickedUpOxygen,
        PickedUpRAM
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
        EnteredStrategyView,
        SwitchedMachines,
        TargetedMalware,
        CreatedTracer,
        CreatedStation,
        CleanedMachine
    }
}
