using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NetworkLocation {

	public string Name { get; set; }
	public string sceneName {get;set;}
	public bool IsInfected {get;set;}
	public NetworkLocation Parent {get;set;}
	public IEnumerable<NetworkLocation> Children {get;set;}
}
