using UnityEngine;
using System.Collections;
using System;

public struct AILine {

    public AIRenderer.RIState State;
    public bool Sticky;
    public string Text;
    public Enum TutorialFlag;

    public AILine(AIRenderer.RIState state, string text, bool isSticky = false) : this()
    {
        State = state;
        Text = text;
        Sticky = isSticky;
    }
}
