using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Upgrade{
    
    public string Name { get; set; }
    public int CoreCost { get; set; }
    public string Description { get; set; }

    public class WorkstationMapping
    {
        public Upgrade Base { get; set; }
        public Upgrade Left { get; set; }
        public Upgrade Right { get; set; }
    }

    public static Dictionary<Type, WorkstationMapping> FunctionUpgrades = new Dictionary<Type, WorkstationMapping>()
    {
        {
            typeof(Delete), new WorkstationMapping()
            {
                Base = new Upgrade()
                {
                    Name = "Memory Scrub",
                    CoreCost = 1,
                    Description = "+1 DMG"
                },
                Left = new Upgrade()
                {
                    Name = "Reformatter",
                    CoreCost = 1,
                    Description = "+2 DMG"
                },
                Right = new Upgrade()
                {
                    Name = "Multi-Trace",
                    CoreCost = 1,
                    Description = "x3 shots"
                }
            }
        },
        {
            typeof(Terminate), new WorkstationMapping()
            {
                Base = new Upgrade()
                {
                    Name = "Thread Suspend",
                    CoreCost = 1,
                    Description = "1s Freeze"
                },
                Left = new Upgrade()
                {
                    Name = "SUDO",
                    CoreCost = 1,
                    Description = "+2 DMG"
                },
                Right = new Upgrade()
                {
                    Name = "???",
                    CoreCost = 1,
                    Description = "Bypass shields"
                }
            }
        }
    };
}
