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
	private int currCores;
	public int CurrentCores {
		get{ return currCores; }
		set{
			currCores = value;
			if (OnCoreChange != null)
				OnCoreChange();
		}
	}
	private int usedCores;
	public int UsedCores {
		get { return usedCores; }
		set {
			usedCores = value;
			
			if (OnCoreChange != null)
				OnCoreChange();
		}
	}

	public delegate void OnCoreChangeEvent();
	public OnCoreChangeEvent OnCoreChange;

	public CyberspaceBattlefield() {
	}

	public bool CanUseCores(int amount)
	{
		return UsedCores + amount <= CurrentCores;
	}

}
