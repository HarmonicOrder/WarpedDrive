using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

public class ScriptedNewGame : MonoBehaviour {
    
    public MeshRenderer topSlit, bottomSlit;
    public Light GlobalLight;
    public Canvas ClockCanvas;
    public bool TestNewGame;
    public GameObject EmergencyLight;

    private BlurOptimized blurrer;

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
            StartCoroutine(MoveSlits());
        }
        else
        {
            DestroySlits();
            this.enabled = false;
            GameObject.Destroy(EmergencyLight);
        }
    }

    private IEnumerator MoveSlits()
    {
        yield return new WaitForSeconds(2f);
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

        GameObject.Destroy(EmergencyLight);
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

    private void DestroySlits()
    {
        topSlit.enabled = false;
        bottomSlit.enabled = false;
    }
}
