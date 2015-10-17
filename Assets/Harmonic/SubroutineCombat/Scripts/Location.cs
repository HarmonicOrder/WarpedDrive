using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Location{

	public string Name { get; set; }
	public Location Parent {get;set;}
	public IEnumerable<Location> Children {get;set;}
	
	public Location FindByName(string name)
	{
		if (this.Name.ToUpper() ==  name.ToUpper())
			return this;
		
		if (this.Children == null)
			return null;
		
		Location childLoc = null;
		foreach(Location net in Children)
		{
			childLoc = net.FindByName(name);
			if (childLoc != null)
				return childLoc;
		}
		
		return null;
	}
}
