using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.ComponentModel;

public class Toaster : MonoBehaviour {
    public Text Output;
    public RectTransform OutputPanel;
    public Image ProgressBar;

    private bool isDisplayed = false;
    private const float ToastTime = 3f;
    private bool readyForToast = true;
    private Guid? stickyID = null;

	// Use this for initialization
	void Start () {
        OutputPanel.localScale = new Vector3(1, 0, 1);
        ToastLog.OnToast += ToastLog_OnToast;
        ProgressBar.fillAmount = 0f;
	}

    void ToastLog_OnToast(object sender, EventArgs e)
    {
        var tea = (e as ToastLog.ToastEventArgs);
        if (!tea.Handled)
        {
            if (stickyID.HasValue && tea.ID == stickyID)
            {
                UpdateDisplay(tea);
                tea.Handled = true;
                
                if (tea.Progess < 0)
                {
                    StartCoroutine(ShrinkOut());
                }
            }
            else if (readyForToast)
            {
                UpdateDisplay(tea);
                tea.Handled = true;
                StartCoroutine(GrowIn());
            }
        }
    }

    private void UpdateDisplay(ToastLog.ToastEventArgs tea)
    {
        Output.text = tea.Message;
        ProgressBar.fillAmount = tea.Progess / 100f;
        stickyID = tea.ID;
    }

    private IEnumerator GrowIn()
    {
        readyForToast = false;
        while(OutputPanel.localScale.y < 1)
        {
            OutputPanel.localScale = new Vector3(1, OutputPanel.localScale.y + Time.deltaTime * 4, 1);
            yield return null;
        }
        OutputPanel.localScale = Vector3.one;
        yield return new WaitForSeconds(ToastTime);

        if (!stickyID.HasValue)
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
        readyForToast = true;
    }

    void OnDestroy()
    {
        ToastLog.OnToast -= ToastLog_OnToast;
    }
	
}
