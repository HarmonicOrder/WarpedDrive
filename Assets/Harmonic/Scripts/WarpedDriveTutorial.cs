using UnityEngine;
using System.Collections;
using System;

public class GameTutorial : MonoBehaviour {

    private bool justEnteredCyberspace;

    void Awake()
    {
        GameObject.DontDestroyOnLoad(gameObject);
    }

    void Start ()
    {
        LoadOrStart();
    }

    void OnLevelWasLoaded()
    {
        LoadOrStart();
    }

    private void LoadOrStart()
    {
        if (Application.loadedLevel == 1)
        {
            MainStart();
        }
        else if (Application.loadedLevel == 2)
        {
            CyberspaceStart();
        }
        else if (Application.loadedLevel == 10)
        {
            SecurityLevelStart();
        }
    }

    private CyberspaceDroneInput InputScript;
    private bool isWaitingForMachineMove;
    private bool isWaitingForSubroutineInstantiation;

    private void SecurityLevelStart()
    {
        InputScript = GameObject.Find("StrategyMove").GetComponent<CyberspaceDroneInput>();
        AIRenderer.Instance.Output(AIRenderer.RIState.Talking, "This subnetwork has a maglock controller on it somewhere.", false, true);
        AIRenderer.Instance.Output(AIRenderer.RIState.Talking, "We'll have to remove the infection by starting some antivirus programs.", false, true);
        AIRenderer.Instance.Output(AIRenderer.RIState.Talking, "Let's move off of the gateway machine, and over to that other machine.", false, true);
        AIRenderer.Instance.Output(AIRenderer.RIState.Talking, "[Click] on the name of the other machine, the infected one.", true, true);
        isWaitingForMachineMove = true;
    }

    private void CyberspaceStart()
    {
        if (justEnteredCyberspace)
        {
            justEnteredCyberspace = false;
            AIRenderer.Instance.Output(AIRenderer.RIState.Talking, "OK, now we have more time to talk, even with your limited oxygen supply.", false, true);
            AIRenderer.Instance.Output(AIRenderer.RIState.Talking, "Thanks to your direct neural interface, you can experience the net at the speed of thought.", false, true);
            AIRenderer.Instance.Output(AIRenderer.RIState.Talking, "You may have noticed the entire network is infected with all sorts of viruses.", false, true);
            AIRenderer.Instance.Output(AIRenderer.RIState.Talking, "I woke you up because if we don't clean the system, we'll be stuck in space...forever.", false, true);
            AIRenderer.Instance.Output(AIRenderer.RIState.Talking, "First things first, let's find more oxygen for you. Let's look at our local network. [Z]", true, true);
        }
    }

    private void MainStart()
    {
        if (!Tutorial.HasDone(Tutorial.MeatspaceProgress.EnteredCyberspace))
        {
            AIRenderer.Instance.Output(AIRenderer.RIState.Talking, "You're awake! Thank the programmers you weren't locked in there.", false, true);
            AIRenderer.Instance.Output(AIRenderer.RIState.Talking, "Oxygen levels are low...let's talk in cyberspace. Hurry, jack in to the net. [Spacebar]", true, true);
        }
    }

    void Update()
    {
        if (Application.loadedLevel == 1)
        {
            MainUpdate();
        }
        else if (Application.loadedLevel == 2)
        {
            HomebaseUpdate();
        }
        else if (Application.loadedLevel == 10)
        {
            CorridorUnlockUpdate();
        }
    }

    private void CorridorUnlockUpdate()
    {
        if (isWaitingForMachineMove)
        {
            if (InputScript.CurrentMachine != null)
            {
                AIRenderer.Instance.Output(AIRenderer.RIState.Talking, "Look at that nasty red infection around the maglock firmware.", false, true);
                AIRenderer.Instance.Output(AIRenderer.RIState.Talking, "Let's remove the malicious code. We should have a Defender AV ready.", false, true);
                AIRenderer.Instance.Output(AIRenderer.RIState.Talking, "You can see that it's listed on the right side of your UI. The one with the shield.", false, true);
                AIRenderer.Instance.Output(AIRenderer.RIState.Talking, "Let's pick an entry point in the system for this Defender subroutine.", false, true);
                AIRenderer.Instance.Output(AIRenderer.RIState.Talking, "[Click] on a light blue box, then create a Defender virus [2].", true, true);
                isWaitingForMachineMove = false;
                isWaitingForSubroutineInstantiation = true;
                Tutorial.JustDid(Tutorial.HackingProgress.SwitchedMachines);
            }
        }
        if (isWaitingForSubroutineInstantiation)
        {
            if (Input.GetKeyUp(KeyCode.Alpha2))
            {
                AIRenderer.Instance.Output(AIRenderer.RIState.Talking, "Now the Defender should make short work of the infected program.", false, true);
                //Tutorial.JustDid(Tutorial.HackingProgress.TargetedMalware);
            }
        }
    }

    private void HomebaseUpdate()
    {
        if (Input.GetKeyUp(KeyCode.Z))
        {
            if (!Tutorial.HasDone(Tutorial.CyberspaceProgress.ZoomedOutOfCyberspace))
            {
                Tutorial.JustDid(Tutorial.CyberspaceProgress.ZoomedOutOfCyberspace);
                AIRenderer.Instance.Output(AIRenderer.RIState.Talking, "Let me find where some oxygen canisters are hidden.", false, true);
                AIRenderer.Instance.Output(AIRenderer.RIState.Searching, "......", false, true);
                AIRenderer.Instance.Output(AIRenderer.RIState.Talking, "OK I found some in a secure locker. Click on that server labeled 'Physical Security'.", true, true);
            }
        }
    }

    private void MainUpdate()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            if (!Tutorial.HasDone(Tutorial.MeatspaceProgress.EnteredCyberspace))
            {
                Tutorial.JustDid(Tutorial.MeatspaceProgress.EnteredCyberspace);
                justEnteredCyberspace = true;
            }
        }
    }
}
