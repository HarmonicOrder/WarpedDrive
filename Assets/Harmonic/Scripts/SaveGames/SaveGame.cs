using System;
using System.Collections;

public class SaveGame {
    public string Name { get; set; }
    public DateTime LastSaved { get; set; }
    public CyberspaceEnvironment Cyberspace { get; set; }
    public StarshipEnvironment Meatspace { get; set; }

    public static string DefaultSaveGameName = "New Game";
    public static SaveGame GetNewDefault(string name)
    {
        return new SaveGame()
        {
            Name = (name == "") ? DefaultSaveGameName : name,
            LastSaved = DateTime.MinValue,
            Cyberspace = CyberspaceEnvironment.GetDefault(),
            Meatspace = StarshipEnvironment.GetDefault()
        };
    }
}
