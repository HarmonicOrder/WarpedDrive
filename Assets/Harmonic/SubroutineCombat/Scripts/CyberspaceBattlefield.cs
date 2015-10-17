using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class CyberspaceBattlefield {

	public static string[,] Phonetic = new string[4,6]{
		{"alpha", "bravo", "charlie", "delta", "echo", "foxtrot"},
		{"ack", "bar", "car", "data", "end", "foo"},
		{"amp", "buzz", "crypto", "droid", "epoch", "fizz"},
		{"admin", "blit", "cube", "demo", "engine", "farm"}
	};

	public static CyberspaceBattlefield Current {get;set;}

	public string SubnetAddress;
	public string HumanSubnetAddress;
	public int TotalCores { get {
			return 0;
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
		SetRandomSubnetAddress();
	}

	public bool CanUseCores(int amount)
	{
		return UsedCores + amount <= CurrentCores;
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
		System.Random r = new System.Random();
		System.Text.StringBuilder result = new System.Text.StringBuilder(4);
		result.Append("::");
		for (int i = 0; i < 4; i++) {
			result.Append(hexChars[r.Next(hexChars.Length)]);
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
				
				result.Append(Phonetic[i, charIndex]);
			}
			else
			{
				result.Append(address[i]);
			}
		}
		return result.ToString();
	}
}
