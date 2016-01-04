using UnityEngine;
using System.Collections;
using System;

public class Autosave : MonoBehaviour {

    private static Autosave _instance;
    public static Autosave Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = (Autosave)new GameObject("autosave").AddComponent(typeof(Autosave));
            }

            return _instance;
        }
    }

    public bool On
    {
        set
        {
            if (value)
            {
                AutosaveCoroutine = StartCoroutine(TryAutosave());                
            }
            else
            {
                StopCoroutine(AutosaveCoroutine);
            }
        }
    }

    public Coroutine AutosaveCoroutine { get; private set; }

    // Use this for initialization
    void Start () {
        GameObject.DontDestroyOnLoad(this.gameObject);
	}

    private IEnumerator TryAutosave()
    {
        while (this.enabled)
        {
            yield return new WaitForSeconds(30f);

            if (HarmonicSerialization.Instance.CurrentSave == null)
            {
                print("No savegame to autosave, skipping");
            }
            else
            {
                try
                {
                    print("Autosaving...");
                    HarmonicSerialization.Instance.SaveCurrentGame();
                    print("Autosave complete");
                }
                catch (Exception e)
                {
                    print("Failed to autosave! " + e.ToString());
                }
            }
        }
    }
}
