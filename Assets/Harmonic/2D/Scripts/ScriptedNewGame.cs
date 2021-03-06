﻿using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

public class ScriptedNewGame : MonoBehaviour {
    
    public MeshRenderer topSlit, bottomSlit;
    public Light Spotlight1, Spotlight2;
    public Canvas ClockCanvas;
    public bool TestNewGame;
    public bool TestTutorial;
    public GameObject EmergencyLight;
    public AudioClip WakeyWakey;

    public uint FinalFrequency = 8000;

    private BlurOptimized blurrer;
    private AudioSource computerVoiceAudio;
    private AudioLowPassFilter filter;


    // Use this for initialization
    void Start ()
    {
#if UNITY_EDITOR
        if (TestNewGame)
            HarmonicSerialization.Instance.IsNewGame = true;
#endif

        if (HarmonicSerialization.Instance.IsNewGame)
        {
            blurrer = Camera.main.GetComponent<BlurOptimized>();
            blurrer.enabled = true;
            blurrer.blurSize = 5;
            topSlit.enabled = true;
            bottomSlit.enabled = true;
            ClockCanvas.enabled = false;
            Spotlight1.intensity = Spotlight2.intensity = 0;
            computerVoiceAudio = this.gameObject.AddComponent<AudioSource>();
            computerVoiceAudio.clip = WakeyWakey;
            computerVoiceAudio.playOnAwake = false;
            filter = this.gameObject.AddComponent<AudioLowPassFilter>();
            filter.cutoffFrequency = 800f;
            Radio.Instance.Primary.volume = .25f;
            StartCoroutine(MoveSlits());
        }
        else
        {
            DestroySlits();
            this.enabled = false;
            GameObject.Destroy(EmergencyLight);

            if (TestTutorial && GameObject.Find("Tutorial") == null)
            {
                HarmonicSerialization.Instance.CurrentSave = SaveGame.GetNewDefault("DEBUG");
                StartTutorialScript();
            }
        }
    }

    private static void StartTutorialScript()
    {
        new GameObject("Tutorial").AddComponent<GameTutorial>();
    }

    private IEnumerator MoveSlits()
    {
        yield return new WaitForSeconds(2f);
        computerVoiceAudio.Play(1000);
        yield return new WaitForSeconds(2f);
        StartCoroutine(UnMuffle());
        while (blurrer.blurSize > 0f)
        {
            blurrer.blurSize -= .2f;
            yield return null;
        }
        blurrer.enabled = false;
        blurrer.blurSize = 3f;
        DestroySlits();
        ClockCanvas.enabled = true;

        yield return new WaitForSeconds(2f);
        Spotlight1.intensity = Spotlight2.intensity = 1.25f;
        yield return new WaitForSeconds(.2f);
        Spotlight1.intensity = Spotlight2.intensity = 0;

        yield return new WaitForSeconds(4f);
        yield return StartCoroutine(FlickerLights());

        yield return new WaitForSeconds(4f);
        yield return StartCoroutine(FlickerLights());

        yield return new WaitForSeconds(.2f);
        while(Spotlight1.intensity < 1.25f)
        {
            Spotlight1.intensity = Spotlight2.intensity += .2f;
            yield return null;
        }
        Spotlight1.intensity = Spotlight2.intensity = 1.25f;
        yield return new WaitForSeconds(2f);

        StartTutorialScript();
        GameObject.Destroy(EmergencyLight);
        Radio.Instance.Primary.volume = 1f;
        HarmonicSerialization.Instance.IsNewGame = false;
    }

    private IEnumerator FlickerLights()
    {
        Spotlight1.intensity = Spotlight2.intensity = 1.25f;
        yield return new WaitForSeconds(.2f);
        Spotlight1.intensity = Spotlight2.intensity = 0;
        yield return new WaitForSeconds(.2f);
        Spotlight1.intensity = Spotlight2.intensity = 1.25f;
        yield return new WaitForSeconds(.4f);
        Spotlight1.intensity = Spotlight2.intensity = 0;
    }

    private IEnumerator UnMuffle()
    {
        while(filter.cutoffFrequency < this.FinalFrequency)
        {
            filter.cutoffFrequency += 100f;
            yield return null;
        }
    }

    private void DestroySlits()
    {
        topSlit.enabled = false;
        bottomSlit.enabled = false;
    }
}
