using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Toaster : MonoBehaviour {
    public Text Output;
    public RectTransform OutputPanel;

    private bool isDisplayed = false;
    private const float ToastTime = 3f;

	// Use this for initialization
	void Start () {
        OutputPanel.localScale = new Vector3(1, 0, 1);
        ToastLog.OnToast += ToastLog_OnToast;
	}

    void ToastLog_OnToast(string message)
    {
        Output.text = message;
        StartCoroutine(GrowIn());
    }

    private IEnumerator GrowIn()
    {
        while(OutputPanel.localScale.y < 1)
        {
            OutputPanel.localScale = new Vector3(1, OutputPanel.localScale.y + Time.deltaTime * 4, 1);
            yield return null;
        }
        OutputPanel.localScale = Vector3.one;
        yield return new WaitForSeconds(ToastTime);
        StartCoroutine(ShrinkOut());
    }

    private IEnumerator ShrinkOut()
    {
        while (OutputPanel.localScale.y > 0)
        {
            OutputPanel.localScale = new Vector3(1, OutputPanel.localScale.y - Time.deltaTime * 4, 1);
            yield return null;
        }
        OutputPanel.localScale = new Vector3(1, 0, 1);
    }

    void Destroy()
    {
        ToastLog.OnToast -= ToastLog_OnToast;
    }
	
}
