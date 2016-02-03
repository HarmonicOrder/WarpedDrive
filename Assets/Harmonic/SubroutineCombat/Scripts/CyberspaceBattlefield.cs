using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class CyberspaceBattlefield {

	public static CyberspaceBattlefield Current {get;set;}

	private int totalCores;
	public int TotalCores { get {
			return totalCores;
		}
	}
	private int currCores = 0;
	public int CurrentCores {
		get{ return currCores; }
	}
	private int usedCores = 0;
	public int UsedCores {
		get { return usedCores; }
	}

    public int StolenCores;

	public delegate void OnCoreChangeEvent();
	public OnCoreChangeEvent OnCoreChange;

	public NetworkLocation CurrentNetwork {get;set;}
    public bool Abdicate { get; set; }

    public CyberspaceBattlefield() {
		CurrentNetwork = NetworkMap.GetLocationByCurrentScene();
		if (CurrentNetwork != null)
		{
			totalCores = CurrentNetwork.Machines.Sum( m => m.CPUCores);
		}

		currCores = CurrentNetwork.Machines.Where( m => !m.IsInfected).Sum( m => m.CPUCores);
	}

	public bool CanUseCores(int amount)
	{
		return UsedCores + amount <= CurrentCores;
	}

	public bool ProvisionCores(int amount, bool isStolen = false)
	{
		if (CanUseCores(amount))
		{
			usedCores += amount;

            if (isStolen)
                StolenCores += amount;
			
			FireCoreChange();
			return true;
		}
		else
		{
			return false;
		}
	}

	public bool ReclaimCores(int amount, bool isStolen = false)
	{
		usedCores -= amount;

        if (isStolen)
            StolenCores -= amount;

		FireCoreChange();

		return true;
	}

	public void AddCores(int amount)
	{
		currCores += amount;

		FireCoreChange();
	}

	private void FireCoreChange()
	{	
		if (OnCoreChange != null)
			OnCoreChange();
	}

	public Machine FindByName(string name)
	{
		foreach(Machine m in CurrentNetwork.Machines)
		{
			Machine candidate;
			if (m.Name.ToLower() == name.ToLower())
			{
				candidate = m;
			}
			else
			{
				candidate = (Machine)m.FindByName(name);
			}

			if (candidate != null)
				return candidate;
		}

		return null;
	}
}
