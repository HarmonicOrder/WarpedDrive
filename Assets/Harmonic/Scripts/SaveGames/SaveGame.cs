using System;
using System.Collections;

public class SaveGame {
    public string Name { get; set; }
    public DateTime LastSaved { get; set; }
    public CyberspaceEnvironment Cyberspace { get; set; }
    public StarshipEnvironment Meatspace { get; set; }
    public Tutorial.CyberspaceProgress CyberspaceProgression { get; set; }
    public Tutorial.MeatspaceProgress MeatspaceProgression { get; set; }
    public Tutorial.WorkstationProgress WorkstationProgression { get; set; }
    public Tutorial.HackingProgress HackingProgression { get; set; }
    public bool HideTutorial { get; set; }

    public static string DefaultSaveGameName = "New Game";
    public static SaveGame GetNewDefault(string name)
    {
        //validation: if you try and use warpeddrive, you could overwrite the configuration file
        //so let's tack _game onto it
        if (name == "warpeddrive")
            name = "warpeddrive_game";

        return new SaveGame()
        {
            Name = (name == "") ? DefaultSaveGameName : name,
            LastSaved = DateTime.MinValue,
            Cyberspace = CyberspaceEnvironment.GetDefault(),
            Meatspace = StarshipEnvironment.GetDefault()
        };
    }
}
