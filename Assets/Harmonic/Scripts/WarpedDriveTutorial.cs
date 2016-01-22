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
