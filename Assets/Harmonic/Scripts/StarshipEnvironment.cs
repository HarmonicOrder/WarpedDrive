using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class StarshipEnvironment {

    public Dictionary<string, bool> OpenDoors = new Dictionary<string, bool>();

    //all kilograms!
    public double OxygenStorage { get; set; }
    public double OxygenLevel { get; set; }
    public double OxygenProductionPerSecond { get; set; }
    public DateTime GameStartTime { get; set; }

    public const double OxygenConsumedPerSecond = 0.84f / 24 / 60 / 60;

    public double SecondsTilOxygenRunsOut
    {
        get
        {
            return OxygenLevel / (OxygenConsumedPerSecond - OxygenProductionPerSecond);
        }
    }

    public StarshipEnvironment()
    {
        UnityEngine.Debug.Log("oxygen is " + OxygenConsumer.Instance.enabled);
    }

    private static StarshipEnvironment _instance;
    public static StarshipEnvironment Instance
    {
        get
        {
            if (_instance == null)
                _instance = GetDefault();
            return _instance;
        }
    }

    public static StarshipEnvironment GetDefault()
    {
        return new StarshipEnvironment()
        {
            OxygenLevel = OxygenConsumedPerSecond * 60 * 5,
            GameStartTime = DateTime.Now
        };
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
