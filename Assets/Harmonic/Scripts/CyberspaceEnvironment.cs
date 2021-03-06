﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class CyberspaceEnvironment {

    public uint MaximumRAM { get; set; }

    public List<SubroutineInfo> Subroutines { get; set; }
    public List<string> UnlockedFunctions { get; set; }

    private static CyberspaceEnvironment _instance;
    public static CyberspaceEnvironment Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GetDefault();
                if (Application.isEditor)
                {
                    AddTestingData(_instance);
                }
            }
            return _instance;
        }
    }

    private static void AddTestingData(CyberspaceEnvironment _instance)
    {
        _instance.Subroutines.Add(new SubroutineInfo()
        {
            FunctionName = "Honeypot",
            Hotkey = 3,
            MovementName = "Station",
            ID = "a3",
            LoadedIntoRAM = true
        });
        _instance.Subroutines.Add(new SubroutineInfo()
        {
            FunctionName = "Sandbox",
            Hotkey = 4,
            MovementName = "Station",
            ID = "a4",
            LoadedIntoRAM = true
        });
        _instance.Subroutines.Add(new SubroutineInfo()
        {
            FunctionName = "Freeze",
            Hotkey = 5,
            MovementName = "Station",
            ID = "a5",
            LoadedIntoRAM = true
        });
        _instance.Subroutines.Add(new SubroutineInfo()
        {
            FunctionName = "Lag",
            Hotkey = 6,
            MovementName = "Station",
            ID = "a6",
            LoadedIntoRAM = true
        });
        _instance.Subroutines.Add(new SubroutineInfo()
        {
            FunctionName = "Corrupt",
            Hotkey = 7,
            MovementName = "Tracer",
            ID = "a7",
            LoadedIntoRAM = true
        });
        _instance.MaximumRAM +=6;
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
                    },
            UnlockedFunctions = new List<string>()
            {
                "Delete",
                "Terminate"
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

    public static string[] FunctionNames = new string[7]
    {
        "Delete",
        "Lag",
        "Freeze",
        "Corrupt",
        "Terminate",
        "Honeypot",
        "Sandbox"
    };
}
