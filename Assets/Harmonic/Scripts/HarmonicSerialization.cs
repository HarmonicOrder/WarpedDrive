using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using LitJson;
using System;

public class HarmonicSerialization : MonoBehaviour {

    private static HarmonicSerialization _instance;
    public static HarmonicSerialization Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = (HarmonicSerialization)new GameObject("io").AddComponent(typeof(HarmonicSerialization));
                _instance.LoadConfiguration();
            }

            return _instance;
        }
    }

    private const string GlobalFileName = "warpeddrive.json";
    internal const string ErrorLogFilename = "error.txt";
    private static string WindowsPersistentDataPath = Application.persistentDataPath.Replace(@"/", @"\");
    internal static string GetPath(string filename)
    {
        return Path.Combine(
#if UNITY_STANDALONE_WIN
            WindowsPersistentDataPath,
#elif UNITY_EDITOR_WIN
            WindowsPersistentDataPath,
#else
            Application.persistentDataPath, 
#endif
            filename);
    }

    public Dictionary<string, SaveGame> Games = new Dictionary<string, SaveGame>();
    internal SaveGame CurrentSave { get; set; }
    internal SaveGame ContinueSave { get; set; }
    public bool HasContinueGame
    {
        get
        {
            return (this.configuration != null) && (!String.IsNullOrEmpty(this.configuration.LastSavedGameName)) && (this.ContinueSave != null);
        }
    }

    public WarpedDriveConfiguration configuration { get; set; }
    
    private void LoadConfiguration()
    {
        print(string.Format("Trying to load configuration from {0}", GetPath(GlobalFileName)));
        if (File.Exists(GetPath(GlobalFileName)))
        {
            print("Loading configuration");
            try
            {
                configuration = ReadObjectFromFile<WarpedDriveConfiguration>(GetPath(GlobalFileName));
                LoadSaveGames();
            }
            catch (Exception e)
            {
                WriteToErrorLog("Error loading configuration", e);
            }
        }
        else
        {
            print("Creating default configuration");
            configuration = new WarpedDriveConfiguration()
            {
                SavedGames = new List<string>()
            };
        }
    }

    static T ReadObjectFromFile<T>(string filename)
    {
        string g = File.ReadAllText(filename);
        return JsonMapper.ToObject<T>(g);
    }

    private static void WriteToErrorLog(string message, Exception e)
    {
        print(message);
        File.WriteAllText(GetPath(ErrorLogFilename), string.Format("{0} : {1}", message, e.ToString()));
    }

    private void LoadSaveGames()
    {
        if ((configuration != null) && (configuration.SavedGames != null))
        {
            try
            {
                print(string.Format("Found {0} saved games", configuration.SavedGames.Count));
                foreach (string saveGameName in configuration.SavedGames)
                {
                    print(string.Format("Found save game '{0}'", saveGameName));
                    Games[saveGameName] = ReadObjectFromFile<SaveGame>(GetPath(saveGameName+".json"));
                }

                print(string.Format("Last saved game was '{0}'", this.configuration.LastSavedGameName));
                if (Games.ContainsKey(this.configuration.LastSavedGameName))
                {
                    this.ContinueSave = Games[this.configuration.LastSavedGameName];
                }
            }
            catch (Exception e)
            {
                WriteToErrorLog("Error loading save game", e);
            }
        }
        else
        {
            print(string.Format("Configuration is {0}null, has no saved games list", configuration == null ? "" : "not "));
        }
    }

    public void CreateNewGame(string name)
    {
        this.CurrentSave = SaveGame.GetNewDefault(name);
        this.configuration.SavedGames.Add(this.CurrentSave.Name);
        this.SaveCurrentGame();
    }

    public void ContinueOnLastSavedGame()
    {
        this.CurrentSave = this.ContinueSave;
        StarshipEnvironment.SetInstance(this.CurrentSave.Meatspace);
        CyberspaceEnvironment.SetInstance(this.CurrentSave.Cyberspace);
    }

    public void LoadSaveGame(string name)
    {
        if (Games.ContainsKey(name))
        {
            this.CurrentSave = Games[name];
            StarshipEnvironment.SetInstance(this.CurrentSave.Meatspace);
            CyberspaceEnvironment.SetInstance(this.CurrentSave.Cyberspace);
        }
    }

    public void SaveCurrentGame()
    {
        //save the game first so we can set the configuration's lastSavedGameName
        if (this.CurrentSave != null)
        {
            this.CurrentSave.LastSaved = DateTime.Now;
            File.WriteAllText(GetPath(this.CurrentSave.Name+".json"), JsonMapper.ToJson(this.CurrentSave));
            this.configuration.LastSavedGameName = this.CurrentSave.Name;
        }

        File.WriteAllText(GetPath(GlobalFileName), JsonMapper.ToJson(this.configuration));
    }
    
    void Start () {
        GameObject.DontDestroyOnLoad(this.gameObject);
	}

    public class WarpedDriveConfiguration
    {
        public List<string> SavedGames { get; set; }
        public string LastSavedGameName { get; set; }
    }
}
