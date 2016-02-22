using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class AbilityManager : MonoBehaviour {

    public Button SuspendButton;
    private float SuspendCountdown = 0f;
    private float SuspendCooldown = 30f;
    private Image SuspendImage;

    void Awake()
    {
        this.SuspendImage = SuspendButton.GetComponent<Image>();
    }

    public void ActivateSuspend()
    {
        if (SuspendCountdown <= 0 && CyberspaceDroneInput.CurrentLock != null && CyberspaceDroneInput.CurrentLock is VirusAI)
        {
            SuspendCountdown = SuspendCooldown;
            SuspendButton.interactable = false;
            DoSuspend();
        }
    }

    private void DoSuspend()
    {
        VirusAI v = CyberspaceDroneInput.CurrentLock as VirusAI;
    }

    void Update()
    {
        if (SuspendCountdown > 0)
        {
            SuspendCountdown -= InterruptTime.deltaTime;
            SuspendImage.fillAmount = (SuspendCooldown - SuspendCountdown) / SuspendCooldown;

            if (SuspendCountdown <= 0)
            {
                SuspendCountdown = 0;
                SuspendImage.fillAmount = 1f;
                SuspendButton.interactable = true;
            }
        }
    }
}
