using UnityEngine;
using System.Collections;

public class MachineStrategyAnchor : MonoBehaviour {
    public MachineStrategyAnchor Left, Right, Forward, Backward;
    public Machine myMachine;
    public Transform AntivirusCastle;

    void Awake()
    {
        myMachine.AVCastle = this.AntivirusCastle;

        if (this.AntivirusCastle != null && myMachine.IsInfected) //this.transform.name.ToLower() != "gatewaymachine"
        {
            if (myMachine.IsInfected)
            {
                this.AntivirusCastle.gameObject.SetActive(false);
            }

            myMachine.AVCastleTracerHanger = this.AntivirusCastle.Find("TracerSpawn").transform;
        }
    }
}
