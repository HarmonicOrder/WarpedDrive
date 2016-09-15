using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Radio : MonoBehaviour {

    private static Radio _instance;
    public static Radio Instance {
    get {
            if (_instance == null)
            {
                _instance = (Radio)new GameObject("defaultRadioCreated").AddComponent(typeof(Radio));
                _instance.Primary = _instance.gameObject.AddComponent<AudioSource>();
                _instance.Secondary = _instance.gameObject.AddComponent<AudioSource>();
                _instance.Soundtracks = new List<AudioClip>();
            }
            return _instance;
        }
    }

    public AudioSource Primary;
    public AudioSource Secondary;
    public float CrossFadeTime = 1f;
    public List<AudioClip> Soundtracks;

    private bool isCrossFading = false;
    private float currentCrossFadeTime = 0f;

	// Use this for initialization
	void Awake () {
        Radio._instance = this;
        GameObject.DontDestroyOnLoad(this.gameObject);
        RefreshVolume();
        this.Primary.Play();
    }
	
	// Update is called once per frame
	void Update () {
        if (this.isCrossFading)
        {
            this.Primary.volume = Mathf.Lerp(1, 0, this.currentCrossFadeTime / this.CrossFadeTime);
            this.Secondary.volume = Mathf.Lerp(0, 1, this.currentCrossFadeTime / this.CrossFadeTime);

            this.currentCrossFadeTime += Time.deltaTime;

            if (this.currentCrossFadeTime > this.CrossFadeTime)
            {
                AudioSource oldPrimary = this.Primary;
                this.Primary = this.Secondary;
                this.Secondary = oldPrimary;

                this.Primary.volume = 1f;
                this.Secondary.volume = 0f;

                this.isCrossFading = false;
            }
        }
	}

    public void SetSoundtrack(Soundtrack s)
    {
        AudioClip current = Primary.clip;
        AudioClip next = null;
        RefreshVolume();

        if ((current == null) || (current.name.ToLower() != s.ToString().ToLower()))
        {
            foreach (AudioClip clip in Soundtracks)
            {
                if (clip.name.ToLower() == s.ToString().ToLower())
                {
                    next = clip;
                    break;
                }
            }

            if (next != null)
            {
                this.Secondary.clip = next;
                this.Secondary.volume = 0f;
                this.Secondary.Play();
                this.isCrossFading = true;
                this.currentCrossFadeTime = 0f;
            }
        }
    }

    public void RefreshVolume()
    {
        float value = 1f;
        if (PlayerPrefs.HasKey("Music"))
        {
            value = PlayerPrefs.GetFloat("Music");
        }

        Primary.volume = Secondary.volume = value;
    }

    public enum Soundtrack
    {
        Spacey,
        FranticTechs,
        DigitalEnvironment,
        SubtleElectronica,
        MethodicalAntivirus
    }
}
