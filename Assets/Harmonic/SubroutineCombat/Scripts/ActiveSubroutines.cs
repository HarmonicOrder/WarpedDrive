using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class ActiveSubroutines {

	public static List<Subroutine> List = new List<Subroutine>();
	public static List<VirusAI> VirusList = new List<VirusAI>();

	public static void Add(Subroutine newlyActive)
	{
		if (!List.Contains(newlyActive))
		{
			List.Add(newlyActive);

			//notify viruses of this
			foreach(VirusAI v in VirusList)
			{
				Debug.Log("alerting virus");
				v.OnSubroutineActive(newlyActive);
			}
		}
	}

	public static void Remove(Subroutine newlyInactive)
	{
		if (List.Contains(newlyInactive))
		{
			List.Remove(newlyInactive);

			//notify viruses of this
			foreach(VirusAI v in VirusList)
			{
				v.OnSubroutineInactive(newlyInactive);
			}
		}
	}
}
