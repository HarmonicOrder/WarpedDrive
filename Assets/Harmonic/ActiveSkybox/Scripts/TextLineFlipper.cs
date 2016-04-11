using UnityEngine;
using System.Collections;
using System;

public class TextLineFlipper : MonoBehaviour {

    private TextMesh myText;
    private string[] lines;
    private int currentLine, width;
    private string wipeString;
    private float wipeTime = .1f;
	// Use this for initialization
	void Start () {
        this.myText = this.GetComponent<TextMesh>();
        lines = this.myText.text.Split('\n');
        width = lines[0].Length;

        for (int i = 0; i < width; i++)
        {
            wipeString += "#";
        }

        StartCoroutine(FlipLines());
	}

    private IEnumerator FlipLines()
    {
        while(isActiveAndEnabled)
        {
            yield return new WaitForSeconds(UnityEngine.Random.Range(1, 5));
            currentLine = 0;
            string lastLine;
            for (int i = 0; i < lines.Length; i++)
            {
                lastLine = lines[i];
                lines[i] = wipeString;
                this.myText.text = string.Join("\n", lines);
                yield return new WaitForSeconds(wipeTime);
                lines[i] = lastLine;
            }

            this.myText.text = string.Join("\n", lines);
            yield return new WaitForSeconds(wipeTime);
        }
    }

    // Update is called once per frame
    void Update () {
	
	}
}
