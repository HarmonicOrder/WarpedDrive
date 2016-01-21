using UnityEngine;
using System.Collections;

public class MeatspaceTutorial : MonoBehaviour {
    
    void Start ()
    {
        if (!Tutorial.HasDone(Tutorial.MeatspaceProgress.PickedUpOxygen))
        {
            AIRenderer.Instance.Output(AIRenderer.RIState.Talking, "You're awake! Thank the programmers you weren't locked in there.", false, true);
            AIRenderer.Instance.Output(AIRenderer.RIState.Talking, "Oxygen levels are low...hurry, jack in to your netvis rig [Spacebar].  Meet you in cyberspace.", true, true);
        }
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            if (!Tutorial.HasDone(Tutorial.MeatspaceProgress.PickedUpOxygen))
            {
                Tutorial.JustDid(Tutorial.MeatspaceProgress.PickedUpOxygen);
            }
        }
    }
}
