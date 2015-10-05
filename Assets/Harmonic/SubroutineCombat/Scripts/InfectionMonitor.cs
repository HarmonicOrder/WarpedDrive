using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class InfectionMonitor : MonoBehaviour {

	// Use this for initialization
	void Start () {
		ActiveSubroutines.OnMalwareListChange += OnMalwareListChange;
		ScanAndPrint();
	}

	private void OnMalwareListChange()
	{
		if (ActiveSubroutines.MalwareList.Count == 0)
		{
			OnWin();
		}
		else
		{
			ScanAndPrint();
		}
	}

	private void ScanAndPrint()
	{
		Dictionary<string, Tuple<int, IMalware>> malwareTypes = new Dictionary<string, Tuple<int, IMalware>>();
		foreach(IMalware imal in ActiveSubroutines.MalwareList)
		{
			if (malwareTypes.ContainsKey(imal.GetType().Name))
			{
				print ("incrementing " + imal.GetType().Name);
				Tuple<int, IMalware> thisRecord = malwareTypes[imal.GetType().Name];
				malwareTypes[imal.GetType().Name] = new Tuple<int, IMalware>(thisRecord.First + 1, thisRecord.Second);
			}
			else
			{
				print ("adding " + imal.GetType().Name);
				malwareTypes.Add(imal.GetType().Name, new Tuple<int, IMalware>(1, imal));
			}
		}

		string output = StrategyConsole.Instance.LineStartString + "INFECTION SCAN :\r\n";
		foreach(Tuple<int, IMalware> rec in malwareTypes.Values)
		{
			output += StrategyConsole.Instance.LineStartString + rec.First.ToString()+" ";
			if (rec.First > 1)
			{
				output +=  rec.Second.DisplayNamePlural;
			}
			else
			{
				output +=  rec.Second.DisplayNameSingular;
			}
			output += "\r\n";
		}

		StrategyConsole.Instance.WriteLines(output);
	}

	private void OnWin()
	{
		StrategyConsole.Instance.WriteLines(StrategyConsole.Instance.LineStartString+"SYSTEM CLEAN!\r\n> Esc to return to menu");
		if (NetworkMap.CurrentLocation != null)
			NetworkMap.CurrentLocation.IsInfected = false;
	}

}
