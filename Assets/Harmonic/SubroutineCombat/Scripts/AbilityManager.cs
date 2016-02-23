using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class AbilityManager : MonoBehaviour {

    public Button Suspend, Overclock;
    public Text HoverText;
    private Dictionary<string, AbilityViewModel> Abilities = new Dictionary<string, AbilityViewModel>();

    void Awake()
    {
        HoverText.enabled = false;
        Abilities.Add("suspend", new AbilityViewModel(Suspend));
        Abilities.Add("overclock", new AbilityViewModel(Overclock));
    }

    public void Activate(string name)
    {
        print(CyberspaceDroneInput.CurrentLock);

        AbilityViewModel avm = Abilities[name];
        if (avm.Countdown <= 0 && CyberspaceDroneInput.CurrentLock != null && CyberspaceDroneInput.CurrentLock is VirusAI)
        {
            avm.Countdown = avm.Cooldown;
            avm.Button.interactable = false;
            DoSuspend();
        }
    }

    public void OnButtonHover(string name)
    {
        HoverText.enabled = true;
    }

    public void OnButtonHoverOff(string name)
    {
        HoverText.enabled = false;

    }

    private void DoSuspend()
    {
        VirusAI v = CyberspaceDroneInput.CurrentLock as VirusAI;
    }

    void Update()
    {
        foreach (AbilityViewModel avm in Abilities.Values)
        {
            if (avm.Countdown > 0)
            {
                avm.Countdown -= InterruptTime.deltaTime;
                avm.Image.fillAmount = (avm.Cooldown - avm.Countdown) / avm.Cooldown;

                if (avm.Countdown <= 0)
                {
                    avm.Countdown = 0;
                    avm.Image.fillAmount = 1f;
                    avm.Button.interactable = true;
                }
            }
        }
    }

    private class AbilityViewModel
    {
        public float Cooldown = 30f;
        public float Countdown = 0f;
        public Image Image;
        public Button Button;
        public AbilityViewModel(Button b)
        {
            this.Button = b;
            this.Image = b.GetComponent<Image>();
        }
    }

}
