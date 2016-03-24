using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class Machine : Location {

	public static string[,] Phonetic = new string[4,6]{
		{"alpha", "bravo", "charlie", "delta", "echo", "foxtrot"},
		{"ack", "bar", "car", "data", "end", "foo"},
		{"amp", "buzz", "crypto", "droid", "epoch", "fizz"},
		{"admin", "blit", "cube", "demo", "engine", "farm"}
	};
	public string SubnetAddress;
	public string HumanSubnetAddress;
	public int CPUCores;
	public bool IsInfected;
    public bool IsAccessible = true;
    public bool HasActiveAV { get; set; }
    public List<IMalware> ActiveMalware = new List<IMalware>();
    public List<IMalware> LurkingMalware = new List<IMalware>();

    internal Transform AVBattleship { get; set; }
    internal Transform AVBattleshipTracerHangar { get; set; }
    public bool IsBeingReinfected { get; private set; }
    internal Transform AVCastle { get; set; }
    internal Transform AVCastleTracerHanger { get; set; }
    public Hardpoint[] Hardpoints { get; set; }

    public delegate void SystemCleanEvent();
	public SystemCleanEvent OnMachineClean;
    public SystemCleanEvent OnMachineReInfectionStart;
    public SystemCleanEvent OnMachineReInfectionFailure;
    public SystemCleanEvent OnMachineReInfectionSuccess;

    public Machine()
	{
		SetRandomSubnetAddress();
	}

	private void SetRandomSubnetAddress()
	{
		SubnetAddress = GetFourRandomHexcodes();
		HumanSubnetAddress = ReplaceHexWithPhonetic(SubnetAddress);
	}
	
	private static List<char> hexAlpha = new List<char>() {'a', 'b', 'c', 'd', 'e', 'f'};
	private const string hexChars = "0123456789abcdef";


    private static string GetFourRandomHexcodes()
	{
		System.Text.StringBuilder result = new System.Text.StringBuilder(4);
		result.Append("::");
		for (int i = 0; i < 4; i++) {
			result.Append(hexChars[UnityEngine.Random.Range(0, hexChars.Length)]);
		}
		return result.ToString();
	}
	
	private static string ReplaceHexWithPhonetic(string address)
	{
		System.Text.StringBuilder result = new System.Text.StringBuilder(4);
		for (int i = 0; i < address.Length; i++) {
			if ( hexAlpha.Any( c => c == address[i]))
			{
				int charIndex = hexAlpha.IndexOf(address[i]);
				
				result.Append(Phonetic[i-2, charIndex]);
			}
			else
			{
				result.Append(address[i]);
			}
		}
		return result.ToString();
	}

    public void DoOnMachineClean()
    {
        if (this.AVBattleship)
        {
            //doing this while one of the hardpoints is selected can cause problems
#warning fixme: do something smarter than this to fix hardpoints disappearing
            CyberspaceDroneInput.CurrentLock = null;

            if (CyberspaceDroneInput.Instance.CurrentFocus == this.AVBattleship)
                CyberspaceDroneInput.Instance.ResetFocusToMachine();

            GameObject.Destroy(this.AVBattleship.gameObject);
            this.AVBattleshipTracerHangar = null;
        }

        if (this.AVCastle != null)
        {
            this.AVCastle.gameObject.SetActive(true);
            this.Hardpoints = this.AVCastle.GetComponentsInChildren<Hardpoint>();
        }

        IsInfected = false;
        CyberspaceBattlefield.Current.AddCores(CPUCores);

        if (OnMachineClean != null)
        {
            OnMachineClean();
        }
    }

    public void DoOnMachineReinfectionComplete()
    {
        if (this.AVCastle != null)
        {
            this.AVCastle.gameObject.SetActive(false);
        }

        this.IsInfected = true;
        CyberspaceBattlefield.Current.RemoveCores(CPUCores);

        if (OnMachineReInfectionSuccess != null)
        {
            OnMachineReInfectionSuccess();
        }
    }

    internal void DoOnMachineReinfectionStopped()
    {
        this.IsBeingReinfected = false;

        if (OnMachineReInfectionFailure != null)
        {
            OnMachineReInfectionFailure();
        }
    }

    internal void StartReinfection()
    {
        this.IsBeingReinfected = true;

        if (OnMachineReInfectionStart != null)
        {
            OnMachineReInfectionStart();
        }
    }

    internal Transform StartAntivirus(Transform aVBattleshipPrefab, MachineStrategyAnchor currentAnchor)
    {
        this.HasActiveAV = true;

        Transform av = GameObject.Instantiate<Transform>(aVBattleshipPrefab);
        av.SetParent(currentAnchor.transform);
        av.localPosition = Vector3.forward * 140f;
        av.GetComponent<OrbitAround>().OrbitAnchor = currentAnchor.transform;
        this.Hardpoints = av.GetComponentsInChildren<Hardpoint>();

        return av;
    }

    internal Hardpoint CandidateHardpoint()
    {
        for (int i = 0; i < Hardpoints.Count(); i++)
        {
            UnityEngine.Debug.Log("evaluating " + Hardpoints[i].name);
            if (!Hardpoints[i].Occupied)
                return Hardpoints[i];
        }

        return null;
    }
}
