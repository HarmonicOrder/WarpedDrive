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
				"InfoSec", 
				new NetworkLocation(){
					Name = "InfoSec",
					sceneIndex = 3,
                    Children = new List<Location>
                    {
                        new NetworkLocation(){
                            Name = "MeatSec",
                            sceneIndex = 10,
                            IsInfected = true,
                            Machines = new List<Machine>()
                            {
                                new Machine()
                                {
                                    Name = "GatewayMachine",
                                    CPUCores = 4,
                                    IsInfected = false
                                },
                                new Machine()
                                {
                                    Name = "DBMachine",
                                    CPUCores = 2,
                                    IsInfected = true
                                }
                            }
                        }
                    }
				}
			},
            {
                "Archives",
                new NetworkLocation()
                {
                    Name = "Archives",
                    sceneIndex = 3,
                    Children = new List<Location>() {
                        new NetworkLocation(){
                            Name = "colonyBlueprints",
                            sceneIndex = 8,
                            IsInfected = true,
                            Machines = new List<Machine>()
                            {
                                new Machine()
                                {
                                    Name = "GatewayMachine",
                                    CPUCores = 4,
                                    IsInfected = false
                                },
                                new Machine()
                                {
                                    Name = "ServerMachine",
                                    CPUCores = 4,
                                    IsInfected = true
                                },
                                new Machine()
                                {
                                    Name = "KeystoreMachine",
                                    CPUCores = 2,
                                    IsInfected = true
                                },
                                new Machine()
                                {
                                    Name = "DBMachine",
                                    CPUCores = 2,
                                    IsInfected = true,
                                    IsAccessible = false
                                }
                            }
                        },
                        new NetworkLocation(){
                            Name = "crossH",
                            sceneIndex = 14,
                            IsInfected = true,
                            Machines = new List<Machine>()
                            {
                                new Machine()
                                {
                                    Name = "GatewayMachine",
                                    CPUCores = 4,
                                    IsInfected = false
                                },
                                new Machine()
                                {
                                    Name = "KeystoreMachine1",
                                    CPUCores = 4,
                                    IsInfected = true
                                },
                                new Machine()
                                {
                                    Name = "KeystoreMachine2",
                                    CPUCores = 2,
                                    IsInfected = true
                                },
                                new Machine()
                                {
                                    Name = "DBMachine1",
                                    CPUCores = 2,
                                    IsInfected = true,
                                    IsAccessible = false
                                },
                                new Machine()
                                {
                                    Name = "DBMachine2",
                                    CPUCores = 2,
                                    IsInfected = true,
                                    IsAccessible = false
                                }
                            }
                        },
                        new NetworkLocation(){
                            Name = "colonistDossiers",
                            sceneIndex = 9,
                            IsInfected = true,
                            Machines = new List<Machine>()
                            {
                                new Machine()
                                {
                                    Name = "GatewayMachine",
                                    CPUCores = 4,
                                    IsInfected = false
                                },
                                new Machine()
                                {
                                    Name = "DBMachine",
                                    CPUCores = 2,
                                    IsInfected = true
                                },
                                new Machine()
                                {
                                    Name = "DBMachine2",
                                    CPUCores = 6,
                                    IsInfected = true
                                }
                            }
                        },
                        new NetworkLocation(){
                            Name = "fortress",
                            sceneIndex = 11,
                            IsInfected = true,
                            Machines = new List<Machine>()
                            {
                                new Machine()
                                {
                                    Name = "GatewayMachine",
                                    CPUCores = 2,
                                    IsInfected = false
                                },
                                new Machine()
                                {
                                    Name = "SecurityMachine",
                                    CPUCores = 4,
                                    IsInfected = true
                                },
                                new Machine()
                                {
                                    Name = "ArchiveMachine",
                                    CPUCores = 2,
                                    IsInfected = true
                                },
                                new Machine()
                                {
                                    Name = "ServerMachine",
                                    CPUCores = 2,
                                    IsInfected = true
                                }
                            }
                        },
                        new NetworkLocation(){
                            Name = "ring",
                            sceneIndex = 13,
                            IsInfected = true,
                            Machines = new List<Machine>()
                            {
                                new Machine()
                                {
                                    Name = "GatewayMachine",
                                    CPUCores = 2,
                                    IsInfected = false
                                },
                                new Machine()
                                {
                                    Name = "SecurityMachine",
                                    CPUCores = 4,
                                    IsInfected = true
                                },
                                new Machine()
                                {
                                    Name = "ArchiveMachine",
                                    CPUCores = 2,
                                    IsInfected = true
                                },
                                new Machine()
                                {
                                    Name = "ServerMachine",
                                    CPUCores = 2,
                                    IsInfected = true
                                }
                            }
                        },
                        new NetworkLocation(){
                            Name = "stealthIntro",
                            sceneIndex = 12,
                            IsInfected = true,
                            Machines = new List<Machine>()
                            {
                                new Machine()
                                {
                                    Name = "GatewayMachine",
                                    CPUCores = 2,
                                    IsInfected = false
                                },
                                new Machine()
                                {
                                    Name = "SecurityMachine",
                                    CPUCores = 4,
                                    IsInfected = true
                                },
                                new Machine()
                                {
                                    Name = "ArchiveMachine",
                                    CPUCores = 2,
                                    IsInfected = true
                                },
                                new Machine()
                                {
                                    Name = "ServerMachine",
                                    CPUCores = 2,
                                    IsInfected = true
                                }
                            }
                        },
                        new NetworkLocation(){
                            Name = "wifi",
                            sceneIndex = 15,
                            IsInfected = true,
                            Machines = new List<Machine>()
                            {
                                new Machine()
                                {
                                    Name = "GatewayMachine",
                                    CPUCores = 2,
                                    IsInfected = false
                                },
                                new Machine()
                                {
                                    Name = "SecurityMachine",
                                    CPUCores = 4,
                                    IsInfected = true
                                },
                                new Machine()
                                {
                                    Name = "ArchiveMachine",
                                    CPUCores = 2,
                                    IsInfected = true
                                },
                                new Machine()
                                {
                                    Name = "ServerMachine",
                                    CPUCores = 2,
                                    IsInfected = true
                                }
                            }
                        }
                    }
                }
            },
			{
				"Engineering", 
				new NetworkLocation(){
					Name = "Engineering",
					sceneIndex = 3,
					Children = new List<Location>(){
						new NetworkLocation(){
							Name = "Hydroponics",
							sceneIndex = 4,
							IsInfected = true,
							Machines = new List<Machine>()
							{
								new Machine()
								{
									Name = "GatewayMachine",
									CPUCores = 4,
									IsInfected = false
								},
								new Machine()
								{
									Name = "DBMachine",
									CPUCores = 2,
									IsInfected = true
								}
							}
						},
						new NetworkLocation(){
							Name = "Reactor Control",
							sceneIndex = 5,
							IsInfected = true,
							Machines = new List<Machine>()
							{
								new Machine()
								{
									Name = "GatewayMachine",
									CPUCores = 4,
									IsInfected = false
								},
								new Machine()
								{
									Name = "ServerMachine",
									CPUCores = 2,
									IsInfected = true
								}
							}
						},
						new NetworkLocation(){
							Name = "Propulsion",
							sceneIndex = 6,
							IsInfected = true,
							Machines = new List<Machine>()
							{
								new Machine()
								{
									Name = "GatewayMachine",
									CPUCores = 4,
									IsInfected = false
								},
								new Machine()
								{
									Name = "ServerMachine",
									CPUCores = 2,
									IsInfected = true
								},
                                new Machine()
                                {
                                    Name = "SecurityMachine",
                                    CPUCores = 2,
                                    IsInfected = true
                                },
                                new Machine()
                                {
                                    Name = "ArchiveMachine",
                                    CPUCores = 2,
                                    IsInfected = true
                                }
                            }
						},
						new NetworkLocation(){
							Name = "3DPrintshop",
							sceneIndex = 7,
							IsInfected = true,
							Machines = new List<Machine>()
							{
								new Machine()
								{
									Name = "GatewayMachine",
									CPUCores = 4,
									IsInfected = false
								},
								new Machine()
								{
									Name = "DBMachine",
									CPUCores = 2,
									IsInfected = true
								},
								new Machine()
								{
									Name = "DBMachine2",
									CPUCores = 6,
									IsInfected = true
								}
							}
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
			candidate = (NetworkLocation)net.FindByName(name);
			if (candidate != null)
				return candidate;
		}

		return null;
	}

	public static NetworkLocation GetLocationByCurrentScene()
	{		
		IEnumerable<Location> coll = ConvertToList(RootSubnets.Values);
		if (coll != null)
			return LocByCurrentScene(coll);
		else 
			return null;
	}

	private static NetworkLocation LocByCurrentScene(IEnumerable<Location> collection)
	{
		Location candidate = null;
		foreach(Location net in collection)
		{
			if ((net as NetworkLocation).sceneIndex == UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex)
				candidate = net;
			else if (net.Children != null)
				candidate = LocByCurrentScene(net.Children);

			if (candidate != null)
				return (NetworkLocation)candidate;
		}

		return null;
	}

	private static IEnumerable<Location> ConvertToList(Dictionary<string, NetworkLocation>.ValueCollection values)
	{
		//it has to be downcast
		List<Location> list = new List<Location>();
		foreach(NetworkLocation netLoc in values)
		{
			//i would do addRange but it can't downcast while doing that
			//Debug.Log("adding");
			list.Add(netLoc);
		}
		return list;
	}
}
