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

	
	
	public static Transform FindClosestActiveSubroutine(Vector3 fromPosition, float range)
	{
		if (ActiveSubroutines.List.Count == 0)
		{
			return null;
		}
		
		//comparing range squared vs magnitude squared is a performance enhancement
		//it eliminates the expensive square root calculation
		float closest = range * range;
		Transform result = null;
		foreach( Subroutine mal in ActiveSubroutines.List)
		{
			float dist = (mal.transform.position - fromPosition).sqrMagnitude; 
			//if this has a higher priority than now
			//and the distance is closer
			if (dist < closest)
			{
				result = mal.transform;
				closest = dist;
			}
		}

		return result;
	}
}
