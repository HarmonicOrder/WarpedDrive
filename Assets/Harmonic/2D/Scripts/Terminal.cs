using UnityEngine;
using System.Collections;
using System;

public class Terminal : MonoBehaviour {

    public TextMesh TerminalText;

    private Coroutine StatusRendering;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	}

    public void ShowStatus()
    {
        StatusRendering = StartCoroutine(PrintLoop());
    }

    public void StopStatus()
    {
        StopCoroutine(StatusRendering);
    }

    private IEnumerator PrintLoop()
    {
        while (enabled)
        { 
            PrintStatus();
            yield return new WaitForSeconds(1);
        }
    }

    public void PrintStatus()
    {
        //Adnascentia, n, plural, root-like branches that sprout into the earth from a plant's stem
        TerminalText.text = string.Format(
            "UNITED NATIONS COLONY SHIP <i>Adnascentia</i>\n"+
            "======================================\n"+
            "Life Support Report\n"+
            "Oxygen Storage Space: {0}\nOxygen Level:         {1}\nOxygen Depletion in:{2}",
            HarmonicUtils.HumanizeKilograms(StarshipEnvironment.Instance.OxygenStorage), 
            HarmonicUtils.HumanizeKilograms(StarshipEnvironment.Instance.OxygenLevel), 
            HarmonicUtils.HumanizeTimespan(TimeSpan.FromSeconds(StarshipEnvironment.Instance.SecondsTilOxygenRunsOut)));
    }
}
