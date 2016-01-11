using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class CyberspaceEnvironment {

    public uint MaximumRAM { get; set; }

    public List<SubroutineInfo> Subroutines { get; set; }

    private static CyberspaceEnvironment _instance;
    public static CyberspaceEnvironment Instance
    {
        get
        {
            if (_instance == null)
                _instance = GetDefault();
            return _instance;
        }
    }

    public static CyberspaceEnvironment GetDefault()
    {
        return new CyberspaceEnvironment()
        {
            MaximumRAM = 3,
            Subroutines = new List<SubroutineInfo>()
                    {
                        new SubroutineInfo() {
                            LoadedIntoRAM = true
                        },
                        new SubroutineInfo()
                        {
                            FunctionName = "Terminate",
                            Hotkey = 2,
                            MovementName = "Station",
                            ID = "a2",
                            LoadedIntoRAM = true                            
                        }
                    }
        };
    }

    internal static void SetInstance(CyberspaceEnvironment env)
    {
        _instance = env;
    }

    internal uint CurrentRAMUsed
    {
        get
        {
            uint result = 0;
            foreach (var sub in this.Subroutines)
            {
                result += sub.RAMCost;
            }
            return result;
        }
    }

    internal void SetNewID(SubroutineInfo newSI)
    {
        if (this.Subroutines.Count <= 8)
        {
            newSI.ID = "a" + (this.Subroutines.Count + 1);
            newSI.Hotkey = this.Subroutines.Count + 1;

        }
        else
        {
            //TODO: do c-z
            newSI.ID = "b" + (this.Subroutines.Count % 9 + 1);
            newSI.Hotkey = this.Subroutines.Count & 9 + 1;
        }
        UnityEngine.Debug.Log("assigning new id " +newSI.ID);
    }
}
