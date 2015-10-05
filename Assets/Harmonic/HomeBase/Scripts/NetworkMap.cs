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

	public static NetworkLocation CurrentLocation;

	//static constructor!
	static NetworkMap()
	{
		RootSubnets = new Dictionary<string, NetworkLocation>()
		{
			{
				"Infocom", 
				new NetworkLocation(){
					Name = "Infocom",
					sceneIndex = 2
				}
			},
			{
				"Engineering", 
				new NetworkLocation(){
					Name = "Engineering",
					sceneIndex = 2,
					Children = new List<NetworkLocation>(){
						new NetworkLocation(){
							Name = "Hydroponics",
							sceneIndex = 3,
							IsInfected = true
						},
						new NetworkLocation(){
							Name = "Reactor Control",
							sceneIndex = 4,
							IsInfected = true
						},
						new NetworkLocation(){
							Name = "Propulsion",
							sceneIndex = 5,
							IsInfected = true
						}
					}
				}
			}
		};
		SetParents(RootSubnets.Values, null);
	}

	private static void SetParents(IEnumerable children, NetworkLocation parent)
	{
		if (children != null)
		{
			foreach(NetworkLocation net in children)
			{
				net.Parent = parent;
				SetParents(net.Children, net);
			}
		}
	}

	//case insensitive
	public static NetworkLocation GetLocationByLocationName(string name){
		NetworkLocation candidate;

		foreach(NetworkLocation net in RootSubnets.Values)
		{
			candidate = net.FindByName(name);
			if (candidate != null)
				return candidate;
		}

		return null;
	}

}
