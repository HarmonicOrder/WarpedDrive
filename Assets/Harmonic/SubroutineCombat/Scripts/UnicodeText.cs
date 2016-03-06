using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System;

[RequireComponent(typeof(TextMesh))]
public class UnicodeText : MonoBehaviour {
    public int Blocks = 1;
    public int LinesPerBlock = 6;
    public int LineLength = 12;
    public bool FlipRandomly = false;

    private TextMesh myMesh;

    public static List<HarmonicUtils.Range> UnicodeRanges = new List<HarmonicUtils.Range>() {
        //latin
        new HarmonicUtils.Range(0x0020, 0x007F),
        //cyrillic
        new HarmonicUtils.Range(0x0400, 0x04FF),
        //hebrew
        new HarmonicUtils.Range(0x0590, 0x05FF),
        //arabic
        new HarmonicUtils.Range(0x0600, 0x06FF),
        //devanagari
        new HarmonicUtils.Range(0x0900, 0x097F),
        //katakana
        new HarmonicUtils.Range(0x30A0, 0x30FF),
        //hiragana
        new HarmonicUtils.Range(0x3040, 0x309F),
        //hangul
        new HarmonicUtils.Range(0x1100, 0x11FF),
        //cjk (large!)
        new HarmonicUtils.Range(0x4E00, 0x9FFF)
    };

    // Use this for initialization
    void Start () {
        myMesh = GetComponent<TextMesh>();
        StringBuilder sb = new StringBuilder();

        for (int b = 0; b < Blocks; b++)
        {
            if (b != 0)
                sb.Append("\r\n\r\n");

            for (int l = 0; l < LinesPerBlock; l++)
            {
                sb.Append(HarmonicUtils.GetRandomUnicode(LineLength, UnicodeRanges));

                if (l != LinesPerBlock - 1)
                    sb.Append("\r\n");
            }
        }

        myMesh.text = sb.ToString();

        if (FlipRandomly)
            StartCoroutine(DoFlipRandomly());
        else
            this.enabled = false;
	}

    string originalText;
    private IEnumerator DoFlipRandomly()
    {
        while(enabled)
        {
            myMesh.text = InsertAtWithoutNewline(myMesh.text, UnityEngine.Random.Range(0, myMesh.text.Length), HarmonicUtils.GetRandomUnicode(1, UnicodeRanges)[0]);

            yield return new WaitForSecondsInterruptTime(.05f);
        }
    }

    private string InsertAtWithoutNewline(string original, int index, char insert)
    {
        if (original[index] != '\r' && original[index] != '\n')
        {
            return InsertAt(original, index, insert);
        }
        //increment by 2 to pass up \r\ns
        //if we can't go up the index
        else if (index+2 > original.Length-1)
        {
            //go down the index
            return InsertAtWithoutNewline(original, index - 2, insert);
        }
        else
        {
            return InsertAtWithoutNewline(original, index + 2, insert);
        }
    }


    private string InsertAt(string original, int index, char insert)
    {
        char[] chars = original.ToCharArray();
        chars[index] = insert;
        return new string(chars);
    }
}
