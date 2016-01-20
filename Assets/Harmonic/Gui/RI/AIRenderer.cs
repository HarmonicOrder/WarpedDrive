using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

//idea: inherit this for a VirusAIRenderer, something that talks back
//how to sync between them? I have no idea
//maybe a coroutine trade off
// AIRenderer.Instance.Output("you stink")
// return new waitforseconds
// VirusAIRenderer.Instance.Output("no you stink")
public class AIRenderer : MonoBehaviour {
    public Sprite TalkSprite, ThinkSprite, IdeaSprite, SearchingSprite, AlertSprite, TimerAlertSprite;
    public Color NormalColor;
    public Color AlertColor;
    public Text OutputText;
    public RIState State;
    public Animator AnimationAnimator;

    public static AIRenderer Instance { get; set; }

    private Queue<Tuple<RIState, string>> Queue = new Queue<Tuple<RIState, string>>();
    private Queue<Tuple<RIState, string>> PriorityQueue = new Queue<Tuple<RIState, string>>();

    // Use this for initialization
    void Awake () {
        Instance = this;
	}

    //todo: maybe make coroutines instead
    void Update()
    {
        //pull priority queue
        //if not in idle and has priority
        //apologize (this just in...uh sorry but....oh....new data incoming....)
        //show priority
        //reentry (anyway...so anyways...as I was saying...)
        //else
        //if finished with last state
        //set sprite to state
        //set animator to corresponding animation state
        //set text (one sentence at a time?)
        //wait
        //  might even could wait based on how many characters are in the output string
        //repeat

        //maybe include debug input (1-6) to set states
    }
	
    //for conversations it might make sense to return how long the duration for saying the text is?
    //could just be a lookup
    public void Output(RIState state, string text, bool isPriority = false)
    {
        if (isPriority)
        {
            PriorityQueue.Enqueue(new Tuple<RIState, string>(state, text));
        }
        else
        {
            Queue.Enqueue(new Tuple<RIState, string>(state, text));
        }
    }    

    public enum RIState { Idle, Talking, Thinking, Searching, Idea, Timing, Alerting }
}
