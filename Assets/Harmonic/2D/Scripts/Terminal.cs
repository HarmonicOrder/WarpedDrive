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

    //Adnascentia, n, plural, root-like branches that sprout into the earth from a plant's stem
    public const string Header = "UNITED NATIONS STAR SHIP <i>Adnascentia</i>\n";
    public const string EqualLine = "======================================\n";

    public void PrintStatus()
    {
        switch(TerminalManager.CurrentTerminalType)
        {
            case TerminalType.oxygen:
                PrintStatus(string.Format(
                    "Life Support Report\n"+
                    "Oxygen Storage Space: {0}\nOxygen Level:         {1}\nOxygen Depletion in:{2}",
                    HarmonicUtils.HumanizeKilograms(StarshipEnvironment.Instance.OxygenStorage), 
                    HarmonicUtils.HumanizeKilograms(StarshipEnvironment.Instance.OxygenLevel), 
                    HarmonicUtils.HumanizeTimespan(TimeSpan.FromSeconds(StarshipEnvironment.Instance.SecondsTilOxygenRunsOut))));
                break;
            case TerminalType.mainframe:
                PrintStatus(string.Format(
                    "Infosec Report\n" +
                    "Maximum RAM: {0} TBs\nUsed RAM:    {1} TBs\n                       ",
                    CyberspaceEnvironment.Instance.MaximumRAM,
                    CyberspaceEnvironment.Instance.CurrentRAMUsed
                    ));
                break;
            case TerminalType.starship:
                PrintStatus(string.Format(
                    "Starship Report\n" +
                    "Date: {0}\nVoyage Time:\n{1}\n                       ",
                    GetLoreCurrentDateTime().ToString(),
                    HarmonicUtils.HumanizeTimespan(GetLoreCurrentDateTime() - GetLoreVoyageStartTime())
                    ));
                break;
        }        
    }

    public void PrintStatus(string content)
    {
        TerminalText.text =
            Header +
            EqualLine +
            content;
    }

    public const int YearOfDeparture = 2288;
    public const int YearOfIncident = 2293;
    public static DateTime GetLoreVoyageStartTime()
    {
        return new DateTime(YearOfDeparture, 1, 3);
    }

    public static DateTime GetLoreCurrentDateTime()
    {
        return new DateTime(DateTime.Now.Ticks).AddYears(YearOfIncident - StarshipEnvironment.Instance.GameStartTime.Year);
    }

    public static TimeSpan GetPlaytime()
    {
        return DateTime.Now - StarshipEnvironment.Instance.GameStartTime;
    }

    public enum TerminalType
    {
        oxygen,
        mainframe,
        infection,
        starship,
        networkaccess,
    }
}
