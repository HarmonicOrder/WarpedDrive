using System;
using System.Collections;

public class SaveGame {
    public string Name { get; set; }
    public DateTime LastSaved { get; set; }

    public static string DefaultSaveGameName = "New Game";
    public static SaveGame GetNewDefault()
    {
        return new SaveGame()
        {
            Name = DefaultSaveGameName,
            LastSaved = DateTime.MinValue
        };
    }
}
