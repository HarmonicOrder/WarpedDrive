using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CyberspaceEnvironment {

    public uint MaximumRAM { get; set; }
    public uint CurrentRAMUsed { get; set; }

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
            MaximumRAM = 2,
            Subroutines = new List<SubroutineInfo>()
                    {
                        new SubroutineInfo(),
                        new SubroutineInfo()
                        {
                            FunctionName = "Terminate",
                            Hotkey = 2,
                            MovementName = "Station",
                            ID = "a2"
                        }
                    }
        };
    }

    internal static void SetInstance(CyberspaceEnvironment env)
    {
        _instance = env;
    }
}
