using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

public class ScriptedNewGame : MonoBehaviour {
    
    public MeshRenderer topSlit, bottomSlit;
    public Light GlobalLight;
    public Canvas ClockCanvas;
    public bool TestNewGame;
    public bool TestTutorial;
    public GameObject EmergencyLight;
    public AudioClip WakeyWakey;

    public uint FinalFrequency = 8000;

    private BlurOptimized blurrer;
    private AudioSource audio;
    private AudioLowPassFilter filter;


    // Use this for initialization
    void Start ()
    {
        if (TestNewGame)
            HarmonicSerialization.Instance.IsNewGame = true;

        if (HarmonicSerialization.Instance.IsNewGame)
        {
            blurrer = Camera.main.GetComponent<BlurOptimized>();
            blurrer.enabled = true;
            blurrer.blurSize = 5;
            topSlit.enabled = true;
            bottomSlit.enabled = true;
            ClockCanvas.enabled = false;
            GlobalLight.intensity = 0;
            audio = this.gameObject.AddComponent<AudioSource>();
            audio.clip = WakeyWakey;
            audio.playOnAwake = false;
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

            if (TestTutorial)
                StartTutorialScript();
        }
    }

    private static void StartTutorialScript()
    {
        new GameObject("Tutorial").AddComponent<GameTutorial>();
    }

    private IEnumerator MoveSlits()
    {
        yield return new WaitForSeconds(2f);
        audio.Play(1000);
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
        GlobalLight.intensity = 1.25f;
        yield return new WaitForSeconds(.2f);
        GlobalLight.intensity = 0;

        yield return new WaitForSeconds(4f);
        yield return StartCoroutine(FlickerLights());

        yield return new WaitForSeconds(4f);
        yield return StartCoroutine(FlickerLights());

        yield return new WaitForSeconds(.2f);
        while(GlobalLight.intensity < 1.25f)
        {
            GlobalLight.intensity += .2f;
            yield return null;
        }
        GlobalLight.intensity = 1.25f;
        yield return new WaitForSeconds(2f);

        StartTutorialScript();
        GameObject.Destroy(EmergencyLight);
        Radio.Instance.Primary.volume = 1f;
        HarmonicSerialization.Instance.IsNewGame = false;
    }

    private IEnumerator FlickerLights()
    {
        GlobalLight.intensity = 1.25f;
        yield return new WaitForSeconds(.2f);
        GlobalLight.intensity = 0;
        yield return new WaitForSeconds(.2f);
        GlobalLight.intensity = 1.25f;
        yield return new WaitForSeconds(.4f);
        GlobalLight.intensity = 0;
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
