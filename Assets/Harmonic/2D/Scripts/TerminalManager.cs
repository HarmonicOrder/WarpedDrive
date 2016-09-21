using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class TerminalManager : MonoBehaviour {

    public static bool IsNextToTerminal { get; set; }
    public static bool IsNextToGenerator { get; set; }
    public static string GeneratorName { get; set; }
    public static Terminal.TerminalType CurrentTerminalType = Terminal.TerminalType.networkaccess;

    public float TerminalDistance = 2f;
    public Transform Character;

    private TextMesh currentLabel;

    Coroutine check, showLabel;


    // Use this for initialization
    void Start()
    {
        check = StartCoroutine(CheckTerminals());
        showLabel = StartCoroutine(ShowLabel());
        foreach (Transform t in this.transform)
        {
            TextMesh text = t.GetChild(0).GetComponent<TextMesh>();
            text.color = HarmonicUtils.ColorWithAlpha(text.color, 0);
        }
    }

    private IEnumerator ShowLabel()
    {
        float increment = .2f;
        while(enabled)
        {
            if (currentLabel && IsNextToTerminal)
            {
                currentLabel.color = HarmonicUtils.ColorWithAlpha(currentLabel.color, Mathf.Min(currentLabel.color.a + increment, 1f));
            }
            else if (currentLabel && !IsNextToTerminal)
            {
                currentLabel.color = HarmonicUtils.ColorWithAlpha(currentLabel.color, Mathf.Max(currentLabel.color.a - increment, 0f));
            }

            yield return new WaitForSeconds(.1f);
        }
    }

    private IEnumerator CheckTerminals()
    {
        while (enabled)
        {
            IsNextToGenerator = false;
            IsNextToTerminal = false;
            foreach (Transform t in this.transform)
            {
                //print("distance of " + Vector3.Distance(Character.position, t.position));
                if (Vector3.Distance(Character.position, t.position) < TerminalDistance)
                {
                    if (t.name.ToLower().StartsWith("o2gen"))
                    {
                        IsNextToGenerator = true;
                        GeneratorName = t.name;
                        break;
                    }
                    else
                    {
                        IsNextToTerminal = true;
                        currentLabel = t.GetChild(0).GetComponent<TextMesh>();

                        EnumExtensions.TryParse<Terminal.TerminalType>(CurrentTerminalType, t.name.ToLower(), out CurrentTerminalType);
                        break;
                    }
                }
            }

            yield return new WaitForSeconds(.5f);
        }
    }
    
    void OnDestroy() {
        StopCoroutine(check);
        StopCoroutine(showLabel);
    }
}
