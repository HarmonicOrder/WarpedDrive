using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StarshipEnvironment {

    public Dictionary<string, bool> OpenDoors = new Dictionary<string, bool>();

    //all kilograms!
    public float OxygenStorage { get; set; }
    public float OxygenLevel { get; set; }
    public float OxygenProductionPerSecond { get; set; }

    public const float OxygenConsumedPerSecond = 0.84f;

    public float SecondsTilOxygenRunsOut
    {
        get
        {
            return OxygenLevel / (OxygenConsumedPerSecond - OxygenProductionPerSecond);
        }
    }

    private static StarshipEnvironment _instance;
    public static StarshipEnvironment Instance
    {
        get
        {
            if (_instance == null)
                _instance = new StarshipEnvironment();
            return _instance;
        }
    }

    internal static void SetInstance(StarshipEnvironment env)
    {
        _instance = env;
    }

    public bool DoorIsOpen(string name)
    {
        return OpenDoors.ContainsKey(name) && OpenDoors[name];
    }

    public static string[] DoorNames = new string[]
    {
        "infosec",
        "cargobay",
        "biomed",
        "bridge"
    };
}
