using UnityEngine;
using System.Collections;

public class ScriptedNewGame : MonoBehaviour {
    
    public MeshRenderer topSlit, bottomSlit;
    public Light GlobalLight;
    public Canvas ClockCanvas;
    public bool TestNewGame;
    public GameObject EmergencyLight;

    // Use this for initialization
    void Start ()
    {
        if (TestNewGame)
            HarmonicSerialization.Instance.IsNewGame = true;

        if (HarmonicSerialization.Instance.IsNewGame)
        {
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
        float openTime = 0f;
        while (openTime < .5f)
        {
            topSlit.transform.Translate(Vector3.up * openTime * openTime, Space.World);
            bottomSlit.transform.Translate(Vector3.down * openTime * openTime, Space.World);
            openTime += Time.deltaTime;
            yield return null;
        }
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
        GameObject.Destroy(topSlit.gameObject);
        GameObject.Destroy(bottomSlit.gameObject);
    }
}
