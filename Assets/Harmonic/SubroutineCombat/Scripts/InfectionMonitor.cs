using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class InfectionMonitor : MonoBehaviour {

	// Use this for initialization
	void Start () {
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
		print ("scanning");
		Dictionary<Type, Tuple<int, IMalware>> malwareTypes = new Dictionary<Type, Tuple<int, IMalware>>();
		foreach(IMalware imal in ActiveSubroutines.MalwareList)
		{
			if (malwareTypes.ContainsKey(imal.GetType()))
			{
				Tuple<int, IMalware> thisRecord = malwareTypes[imal.GetType()];
				thisRecord = new Tuple<int, IMalware>(thisRecord.First + 1, thisRecord.Second);
			}
			else
			{
				malwareTypes.Add(imal.GetType(), new Tuple<int, IMalware>(1, imal));
			}
		}

		string output = StrategyConsole.Instance.LineStartString + "INFECTION SCAN :";
		foreach(Tuple<int, IMalware> rec in malwareTypes.Values)
		{
			output += StrategyConsole.Instance.LineStartString + rec.First.ToString();
			if (rec.First > 1)
			{
				output +=  rec.Second.DisplayNamePlural;
			}
			else
			{
				output +=  rec.Second.DisplayNameSingular;
			}
			output += "\n";
		}

		StrategyConsole.Instance.WriteLines(output);
	}

	private void OnWin()
	{
		NetworkMap.CurrentLocation.IsInfected = false;
	}

}
