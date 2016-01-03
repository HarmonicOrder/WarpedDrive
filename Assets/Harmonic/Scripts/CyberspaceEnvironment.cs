using UnityEngine;
using System.Collections;

public class CyberspaceEnvironment {

    public uint MaximumRAM { get; set; }
    public uint CurrentRAMUsed { get; set; }

    private static CyberspaceEnvironment _instance;
    public static CyberspaceEnvironment Instance
    {
        get
        {
            if (_instance == null)
                _instance = new CyberspaceEnvironment()
                {
                    MaximumRAM = 2
                };
            return _instance;
        }
    }

    internal static void SetInstance(CyberspaceEnvironment env)
    {
        _instance = env;
    }
}
