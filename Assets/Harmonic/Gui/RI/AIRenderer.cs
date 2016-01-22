using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using System.Text.RegularExpressions;

//idea: inherit this for a VirusAIRenderer, something that talks back
//how to sync between them? I have no idea
//maybe a coroutine trade off
// AIRenderer.Instance.Output("you stink")
// return new waitforseconds
// VirusAIRenderer.Instance.Output("no you stink")
[RequireComponent(typeof(Image))]
public class AIRenderer : MonoBehaviour {
    public Sprite TalkSprite, ThinkSprite, IdeaSprite, SearchingSprite, AlertSprite, TimerAlertSprite;
    public Color NormalColor;
    public Color AlertColor;
    public Text OutputText;
    public RIState State;
    public Animator AnimationAnimator;
    public Image BadgeBackground, BadgeSymbol;
    public RectTransform OutputPanel;
    public bool HideOnIdle = false;

    public static AIRenderer Instance { get; set; }

    //TODO: replace tuples with struct? class?
    //with: bool for 'sticky' instructions
    //enum for 'set tutorial enum after you display this'
    private Queue<AILine> Queue = new Queue<AILine>();
    private Queue<AILine> PriorityQueue = new Queue<AILine>();
    private Image behaviorImage;
    private bool OuputIsOpen = false;
    // Use this for initialization
    void Awake () {
        Instance = this;
        behaviorImage = GetComponent<Image>();
	}
    
    void Start()
    {
        OutputPanel.localScale = new Vector3(1, 0, 1);
        IdleBehavior();
        StartCoroutine(PollQueues());
    }

    private string[] characters;
    private int charIndex;
    private Regex isBreak = new Regex(@"(\.|\?|\!)");
    private float waitTime = .05f;
    private AILine CurrentOutput;

    private IEnumerator PollQueues()
    {
        while(this.isActiveAndEnabled)
        {
            bool hasOutput = false;

            //pull priority queue
            if (PriorityQueue.Count > 0)
            {
                CurrentOutput = PriorityQueue.Dequeue();
                hasOutput = true;
            }
            else if (Queue.Count > 0)
            {
                CurrentOutput = Queue.Dequeue();
                hasOutput = true;
            }

            //if not in idle and has priority
            //apologize (this just in...uh sorry but....oh....new data incoming....)
            //show priority
            //reentry (anyway...so anyways...as I was saying...)
            //else
            if (hasOutput)
            {
                BadgeBackground.color = HarmonicUtils.ColorWithAlpha(BadgeBackground.color, 1f);
                BadgeSymbol.color = HarmonicUtils.ColorWithAlpha(BadgeSymbol.color, 1f);

                if (!OuputIsOpen)
                    yield return StartCoroutine(ToggleBackground(true));

                //set sprite to state
                behaviorImage.sprite = GetSprite(CurrentOutput.State);
                behaviorImage.color = GetColor(CurrentOutput.State);
                //set animator to corresponding animation state
                AnimationAnimator.SetInteger("CurrentState", (int)CurrentOutput.State);
                AnimationAnimator.SetFloat("CurrentSpeed", GetSpeed(CurrentOutput.State));

                OutputText.text = "";
                //set text (one char at a time)
                for (charIndex = 0; charIndex < CurrentOutput.Text.Length; charIndex++)
                {
                    OutputText.text += CurrentOutput.Text[charIndex];

                    if (isBreak.IsMatch(CurrentOutput.Text[charIndex].ToString()))
                    {
                        waitTime = .5f;
                    }
                    else
                    {
                        waitTime = .05f;
                    }
                    yield return new WaitForSeconds(waitTime);
                }
                //give the user a bit to process this
                yield return new WaitForSeconds(1f);
            }
            else
            {
                if (OuputIsOpen && !CurrentOutput.Sticky)
                {
                    yield return StartCoroutine(ToggleBackground(false));
                    IdleBehavior();
                }
                else
                    yield return new WaitForSeconds(.5f);
            }
        }
    }

    private Color GetColor(RIState first)
    {
        switch (first)
        {
            case RIState.Alerting:
                return AlertColor;
            default:
                return NormalColor;
        }
    }

    private void IdleBehavior()
    {
        AnimationAnimator.SetInteger("CurrentState", 0);
        behaviorImage.color = new Color(0, 0, 0, 0);
        OutputText.text = "";
        BadgeBackground.color = HarmonicUtils.ColorWithAlpha(BadgeBackground.color, .5f);
        BadgeSymbol.color = HarmonicUtils.ColorWithAlpha(BadgeSymbol.color, .5f);
    }

    private IEnumerator ToggleBackground(bool doOpen)
    {
        if (doOpen)
        {
            while(OutputPanel.localScale.y < 1)
            {
                OutputPanel.localScale = new Vector3(1, OutputPanel.localScale.y + .1f, 1);
                yield return null;
            }
            OuputIsOpen = true;
        }
        else
        {
            while (OutputPanel.localScale.y > 0)
            {
                OutputPanel.localScale = new Vector3(1, OutputPanel.localScale.y - .1f, 1);
                yield return null;
            }
            OutputPanel.localScale = new Vector3(1, 0, 1);
            OuputIsOpen = false;
        }
    }

    private float GetSpeed(RIState first)
    {
        switch(first)
        {
            case RIState.Alerting:
                return 1;
            case RIState.Idea:
                return 1;
            case RIState.Searching:
                return 2;
            case RIState.Talking:
                return 1;
            case RIState.Thinking:
                return -2;
            case RIState.Timing:
                return .5f;
            default:
                return 1;
        }
    }

#if UNITY_EDITOR
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Alpha0))
        {
            PriorityQueue.Enqueue(new AILine(RIState.Alerting, "There's a snake in my boot!"));
        }
        else if (Input.GetKeyUp(KeyCode.Alpha4))
        {
            Queue.Enqueue(new AILine(RIState.Idea, "Eureka! I think I've cracked it, good sport."));
        }
        else if (Input.GetKeyUp(KeyCode.Alpha3))
        {
            Queue.Enqueue(new AILine(RIState.Searching, "Looking around...can't say I see much. Oh! Nope, I was wrong."));
        }
        else if (Input.GetKeyUp(KeyCode.Alpha1))
        {
            Queue.Enqueue(new AILine(RIState.Talking, "Here we can see the programmer in his native habitat. Filled with Mountain Dew. And video games."));
        }
        else if (Input.GetKeyUp(KeyCode.Alpha2))
        {
            Queue.Enqueue(new AILine(RIState.Thinking, "Hmmmm. Hmmmmmmmm. Hhhhmmmmm..."));
        }
        else if (Input.GetKeyUp(KeyCode.Alpha5))
        {
            Queue.Enqueue(new AILine(RIState.Timing, "My oh my look at the time! You're running low on oxygen..."));
        }
    }
#endif

    private Sprite GetSprite(RIState first)
    {
        switch(first)
        {
            case RIState.Alerting:
                return AlertSprite;
            case RIState.Idea:
                return IdeaSprite;
            case RIState.Searching:
                return SearchingSprite;
            case RIState.Talking:
                return TalkSprite;
            case RIState.Thinking:
                return ThinkSprite;
            case RIState.Timing:
                return TimerAlertSprite;
            default:
                return null;
        }
    }

    //for conversations it might make sense to return how long the duration for saying the text is?
    //could just be a lookup
    public void Output(RIState state, string text, bool isSticky = false, bool isPriority = false)
    {
        if (isPriority)
        {
            PriorityQueue.Enqueue(new AILine(state, text, isSticky));
        }
        else
        {
            Queue.Enqueue(new AILine(state, text, isSticky));
        }
    }    

    public enum RIState { Idle = 0, Talking, Thinking, Searching, Idea, Timing, Alerting }
}
