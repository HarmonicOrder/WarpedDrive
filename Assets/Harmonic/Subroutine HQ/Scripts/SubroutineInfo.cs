using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SubroutineInfo {

    public string ID = "a1";
    public int Hotkey = 1;
    public string FunctionName = "Delete";
    public string MovementName = "Tracer";
    public uint RAMCost = 1;
    public uint CoreCost = 1;

    public List<string> FunctionUpgrades;
    public List<string> MovementUpgrades;

    public string CompositeName
    {
        get
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(FunctionName);
            sb.Append(" ");
            sb.Append(MovementName);
            return sb.ToString();
        }
    }
}
