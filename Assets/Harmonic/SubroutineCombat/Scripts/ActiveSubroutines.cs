using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class ActiveSubroutines {

	public static List<Subroutine> List = new List<Subroutine>();
	public static List<IMalware> MalwareList = new List<IMalware>();

	public delegate void ChangeHandler();
	public static event ChangeHandler OnMalwareListChange;
	
	public static void Add(Subroutine newlyActive)
	{
		if (!List.Contains(newlyActive))
		{
			List.Add(newlyActive);

			//notify viruses of this
			foreach(IMalware iMal in MalwareList)
			{
				if (iMal is VirusAI)
				{
					(iMal as VirusAI).OnSubroutineActive(newlyActive);
				}
			}
		}
	}

	public static void Remove(Subroutine newlyInactive)
	{
		if (List.Contains(newlyInactive))
		{
			List.Remove(newlyInactive);

			//notify viruses of this
			foreach(IMalware iMal in MalwareList)
			{
				if (iMal is VirusAI)
				{
					(iMal as VirusAI).OnSubroutineInactive(newlyInactive);
				}
			}
		}
	}

	public static void AddVirus(IMalware newVirus)
	{
		MalwareList.Add(newVirus);
		if (OnMalwareListChange != null)
			OnMalwareListChange();
	}

	public static void RemoveVirus(IMalware oldVirus)
	{
		MalwareList.Remove(oldVirus);
		if (OnMalwareListChange != null)
			OnMalwareListChange();
	}
}
