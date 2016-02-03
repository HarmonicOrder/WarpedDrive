using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class InfectionMonitor : MonoBehaviour {

	// Use this for initialization
	void Start () {
		ActiveSubroutines.OnMalwareListChange += OnMalwareListChange;
		ScanAndPrint(null);
	}

    void OnDestroy()
    {
        ActiveSubroutines.OnMalwareListChange -= OnMalwareListChange;
    }

	private void OnMalwareListChange(IMalware dead)
	{
		if (ActiveSubroutines.MalwareList.Count == 0)
		{
			OnWin();
		}
		else
		{
			ScanAndPrint(dead);
		}
	}

	private void ScanAndPrint(IMalware dead)
    {
        Dictionary<string, Tuple<int, IMalware>> malwareTypes = new Dictionary<string, Tuple<int, IMalware>>();
        foreach (IMalware imal in ActiveSubroutines.MalwareList)
        {
            if (malwareTypes.ContainsKey(imal.GetType().Name))
            {
                Tuple<int, IMalware> thisRecord = malwareTypes[imal.GetType().Name];
                malwareTypes[imal.GetType().Name] = new Tuple<int, IMalware>(thisRecord.First + 1, thisRecord.Second);
            }
            else
            {
                malwareTypes.Add(imal.GetType().Name, new Tuple<int, IMalware>(1, imal));
            }
        }
        PrintToIcons(malwareTypes, dead);
        //PrintToAI(malwareTypes);
        //PrintToConsole(malwareTypes);
    }

    private void PrintToIcons(Dictionary<string, Tuple<int, IMalware>> malwareTypes, IMalware dead)
    {
        if (VirusIconDisplay.Instance != null)
        {
            if (dead != null && !malwareTypes.ContainsKey(dead.GetType().Name))
            {
                malwareTypes.Add(dead.GetType().Name, new Tuple<int, IMalware>(0, dead));
            }

            foreach (Tuple<int, IMalware> rec in malwareTypes.Values)
            {
                VirusIconDisplay.Instance.UpdateIcon(rec.Second, rec.First);
            }
        }
    }

    private void PrintToAI(Dictionary<string, Tuple<int, IMalware>> malwareTypes)
    {
        string output = "Infection scan reveals: ";

        int count = 0;
        foreach (Tuple<int, IMalware> rec in malwareTypes.Values)
        {
            output += rec.First.ToString() + " ";
            if (rec.First > 1)
            {
                output += rec.Second.Type.ToString();
            }
            else
            {
                output += VirusAI.GetPluralVirusType(rec.Second.Type);
            }
            if (count < malwareTypes.Values.Count)
                output += ", ";
            count++;
        }
        recentOutput = output;

        //throttle to the last scan within some seconds
        if (printingToAI == null)
            printingToAI = StartCoroutine(PrintAfterDelay());
    }
    private Coroutine printingToAI;
    private string recentOutput;

    private IEnumerator PrintAfterDelay()
    {
        yield return new WaitForSeconds(1f);
        AIRenderer.Instance.Output(AIRenderer.RIState.Searching, recentOutput);
        printingToAI = null;
    }


    private void PrintToConsole(Dictionary<string, Tuple<int, IMalware>> malwareTypes)
    {
        string output = StrategyConsole.LineStartString + "INFECTION SCAN :\r\n";
        foreach (Tuple<int, IMalware> rec in malwareTypes.Values)
        {
            output += StrategyConsole.LineStartString + rec.First.ToString() + " ";
            if (rec.First > 1)
            {
                output += rec.Second.Type.ToString();
            }
            else
            {
                output += VirusAI.GetPluralVirusType(rec.Second.Type);
            }
            output += "\r\n";
        }

        StrategyConsole.WriteLines(output);
    }

    private void OnWin()
	{
        ClearIcons();
        if (!CyberspaceBattlefield.Current.Abdicate)
        {
            AIRenderer.Instance.Output(AIRenderer.RIState.Talking, "SYSTEM CLEAN!\r\n> [Esc] to return to menu.");
		    //StrategyConsole.WriteLines(StrategyConsole.LineStartString+"SYSTEM CLEAN!\r\n> Esc to return to menu");
		    if (NetworkMap.CurrentLocation != null)
			    NetworkMap.CurrentLocation.IsInfected = false;
        }
	}

    private void ClearIcons()
    {
        if (VirusIconDisplay.Instance != null)
        {
            VirusIconDisplay.Instance.DisableIcons();
        }
    }
}
