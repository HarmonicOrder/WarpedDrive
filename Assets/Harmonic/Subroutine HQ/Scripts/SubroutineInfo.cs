using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SubroutineInfo {

    public string ID = "a1";
    public int Hotkey = 1;
    public bool LoadedIntoRAM = false;
    public string FunctionName = "Delete";
    public string MovementName = "Tracer";
    public uint RAMCost
    {
        get
        {
            return SubroutineMovement.GetRAMCost(this.MovementName) +
                //get upgrade costs
                0;
        }
    }
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

    public bool ValidRAMUse(string newMovementName)
    {
        return CyberspaceEnvironment.Instance.MaximumRAM >= CyberspaceEnvironment.Instance.CurrentRAMUsed - this.RAMCost + SubroutineMovement.GetRAMCost(newMovementName);
    }
}
