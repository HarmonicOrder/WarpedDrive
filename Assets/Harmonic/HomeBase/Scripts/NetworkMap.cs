using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class NetworkMap {

	public static Dictionary<string, NetworkLocation> RootSubnets;
	public static IEnumerable<string> RootSubnetNames{
		get{
			return RootSubnets.Keys;
		}
	}

	//static constructor!
	static NetworkMap()
	{
		RootSubnets = new Dictionary<string, NetworkLocation>()
		{
			{
				"Infocom", 
				new NetworkLocation(){
					Name = "Infocom",
					sceneName = null
				}
			},
			{
				"Engineering", 
				new NetworkLocation(){
					Name = "engineering",
					sceneName = null
				}
			}
		};
	}

	private static void SetParents()
	{
	}

}
