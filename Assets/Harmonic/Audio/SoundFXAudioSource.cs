using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class SoundFXAudioSource : MonoBehaviour {

    public string PreferenceName = "SoundFX";

    void Awake () {
        this.GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat(this.PreferenceName);
        Destroy(this);
	}
}
