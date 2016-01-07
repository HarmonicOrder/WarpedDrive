using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class TerminalManager : MonoBehaviour {

    public static bool IsNextToTerminal = false;
    public static Terminal.TerminalType CurrentTerminalType = Terminal.TerminalType.networkaccess;
    public float TerminalDistance = 2f;
    public Transform Character;

    Coroutine check;

    // Use this for initialization
    void Start () {
        check = StartCoroutine(CheckTerminals());
	}

    private IEnumerator CheckTerminals()
    {
        while (enabled)
        {
            IsNextToTerminal = false;
            foreach (Transform t in this.transform)
            {
                //print("distance of " + Vector3.Distance(Character.position, t.position));
                if (Vector3.Distance(Character.position, t.position) < TerminalDistance)
                {
                    IsNextToTerminal = true;

                    CurrentTerminalType.TryParse<Terminal.TerminalType>(t.name.ToLower(), out CurrentTerminalType);
                    break;
                }
            }

            yield return new WaitForSeconds(.5f);
        }
    }
    
    void OnDestroy() {
        StopCoroutine(check);
	}
}
