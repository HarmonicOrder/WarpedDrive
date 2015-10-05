using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NetworkLocation {

	public string Name { get; set; }
	public int sceneIndex {get;set;}
	public bool IsInfected {get;set;}
	public NetworkLocation Parent {get;set;}
	public IEnumerable<NetworkLocation> Children {get;set;}

	public NetworkLocation FindByName(string name)
	{
		if (this.Name.ToUpper() ==  name.ToUpper())
			return this;

		if (this.Children == null)
			return null;

		NetworkLocation childLoc = null;
		foreach(NetworkLocation net in Children)
		{
			childLoc = net.FindByName(name);
			if (childLoc != null)
				return childLoc;
		}

		return null;
	}
}
