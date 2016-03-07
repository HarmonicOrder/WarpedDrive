using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class ActiveSubroutines {

	public static List<Subroutine> List = new List<Subroutine>();
    /// <summary>
    /// canonical list of all malware currently in the subnet.
    /// Also includes any (stealth) malware that are currently lurking
    /// </summary>
	public static List<IMalware> MalwareList = new List<IMalware>();

	public delegate void ChangeHandler(IMalware dead);
	public static event ChangeHandler OnMalwareListChange;
	
	public static void Add(Subroutine newlyActive)
	{
		if (!List.Contains(newlyActive))
		{
			List.Add(newlyActive);

			//notify viruses of this
			foreach(IMalware iMal in MalwareList)
			{
                //todo: check for ISubroutineListener instead
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
                //todo: check for ISubroutineListener instead
                if (iMal is VirusAI)
				{
					(iMal as VirusAI).OnSubroutineInactive(newlyInactive);
				}
			}
		}
	}

	public static void AddVirus(IMalware newVirus)
	{
		Machine m = CyberspaceBattlefield.Current.FindByName(newVirus.transform.root.name);
		if (m == null)
        {
            UnityEngine.Debug.LogWarning("No machine found to add virus to!");
        }
        else
        {
		    MalwareList.Add(newVirus);

            if (newVirus.IsLurking())
            {
                m.LurkingMalware.Add(newVirus);
            }
            else
            {
                if (!m.IsInfected)
                {
                    m.StartReinfection();
                }
			    m.ActiveMalware.Add(newVirus);
            }

            if (OnMalwareListChange != null)
                OnMalwareListChange(null);
        }
	}

	public static void RemoveVirus(IMalware oldVirus)
	{
		Machine m = CyberspaceBattlefield.Current.FindByName(oldVirus.transform.root.name);
		if (m != null)
		{
			m.ActiveMalware.Remove(oldVirus);
			if (m.ActiveMalware.Count == 0){

                if (CyberspaceBattlefield.Current.Abdicate)
                {
                    //noop
                }
                else if (m.IsInfected)
                {
                    if (m.LurkingMalware.Count > 0)
                    {
                        foreach(ILurker l in m.LurkingMalware)
                        {
                            l.OnMachineClean();
                        }
                    }
                    
                    m.DoOnMachineClean();
                }
                else if (m.IsBeingReinfected)
                {
                    m.DoOnMachineReinfectionStopped();
                }
			}
		}
        else
        {
            Debug.LogWarning("cant find machine to remove virus from activemalware list");
        }

        if (!MalwareList.Remove(oldVirus))
            Debug.LogWarning("Could not remove virus!");

		if (OnMalwareListChange != null)
			OnMalwareListChange(oldVirus);

        //UnityEngine.Debug.Log("still: " + (MalwareList.Count) + " malware");
        //MalwareList.ForEach(a => UnityEngine.Debug.Log("still here: "+a.transform.name));
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
		foreach( Subroutine sub in ActiveSubroutines.List)
		{
			if ((sub != null) && (sub.transform != null))
			{
				float dist = (sub.transform.position - fromPosition).sqrMagnitude; 
				//if this has a higher priority than now
				//and the distance is closer
				if (dist < closest)
				{
					result = sub.transform;
					closest = dist;
				}
			}
		}

		return result;
	}

	
	public static IMalware FindClosestMalware(Vector3 fromPosition, float Range)
	{
		if (ActiveSubroutines.MalwareList.Count == 0)
		{
			return null;
		}
		
		//comparing range squared vs magnitude squared is a performance enhancement
		//it eliminates the expensive square root calculation
		float closest = Range * Range;

		IMalware closeIMal = null;

		foreach( IMalware mal in ActiveSubroutines.MalwareList)
		{
            if (mal.IsLurking())
                continue;

			float dist = (mal.transform.position - fromPosition).sqrMagnitude / mal.AttackPriority; 
			//if this has a higher priority than now
			//and the distance is closer
			if (dist < closest)
			{
				closeIMal = mal;
                closest = dist;
			}
		}

		return closeIMal;
	}
}
