﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class SubnetworkLocation : Location {

	public static string[,] Phonetic = new string[4,6]{
		{"alpha", "bravo", "charlie", "delta", "echo", "foxtrot"},
		{"ack", "bar", "car", "data", "end", "foo"},
		{"amp", "buzz", "crypto", "droid", "epoch", "fizz"},
		{"admin", "blit", "cube", "demo", "engine", "farm"}
	};
	public string SubnetAddress;
	public string HumanSubnetAddress;

	public SubnetworkLocation()
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
		System.Random r = new System.Random();
		System.Text.StringBuilder result = new System.Text.StringBuilder(4);
		result.Append("::");
		for (int i = 0; i < 4; i++) {
			result.Append(hexChars[r.Next(hexChars.Length)]);
		}
		Debug.Log(result.ToString());
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
}