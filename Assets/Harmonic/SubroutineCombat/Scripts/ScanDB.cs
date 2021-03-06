﻿using UnityEngine;
using System.Collections;
using System;

public class ScanDB : NetworkLocationButton, IActivatable
{
    public Transform ScanTransform;
    public float scanHeight;
    public CyberspaceDroneInput Input;

    public string UnlockFilename;
    public Sprite UnlockImage;
    public string UnlockText;
    public string UnlockFunction;
    public string UnlockUpgrade;

    private Machine myMachine { get; set; }

    // Use this for initialization
    void Start()
    {
        this.myMachine = CyberspaceBattlefield.Current.FindByName(this.transform.root.name);
        this.myMachine.OnMachineClean += OnMachineClean;
        this.transform.localScale = Vector3.zero;
        ScanTransform.gameObject.SetActive(false);
    }

    private void OnMachineClean()
    {
        if (isActiveAndEnabled)
            StartCoroutine(this.Open());
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Activate()
    {
        StartCoroutine(Scan());
        StartCoroutine(Close());
    }

    public IEnumerator Scan()
    {
        ScanTransform.gameObject.SetActive(true);

        float currentSweep = 0, coefficient = -1, current = 0, duration = 1f;
        Vector3 from, to;

        from = ScanTransform.position;
        to = ScanTransform.position + (Vector3.up * scanHeight * coefficient);

        while (currentSweep < 3)
        {
            if (current / duration > 1)
            {
                currentSweep++;
                coefficient = coefficient * -1;
                from = ScanTransform.position;
                to = ScanTransform.position + (Vector3.up * scanHeight * coefficient);
                current = 0;
            }
            ScanTransform.position = Vector3.Lerp(from, to, current / duration);
            current += InterruptTime.deltaTime;
            yield return null;
        }

        ScanTransform.gameObject.SetActive(false);
        Unlock();
    }

    private void Unlock()
    {
        if (UnlockImage != null)
        {
            Input.ShowImage(UnlockFilename, UnlockImage);
        }
        else if (!string.IsNullOrEmpty(UnlockText))
        {
            Input.ShowText(UnlockFilename, UnlockText);
        }
        else if (!string.IsNullOrEmpty(UnlockFunction))
        {
            CyberspaceEnvironment.Instance.UnlockedFunctions.Add(UnlockFunction);
            ToastLog.Toast(string.Format("{0}\nUnlocked!", UnlockFunction));
        }
        else if (!string.IsNullOrEmpty(UnlockUpgrade))
        {

        }
    }

    void OnDestroy()
    {
        this.myMachine.OnMachineClean -= OnMachineClean;
    }
}
