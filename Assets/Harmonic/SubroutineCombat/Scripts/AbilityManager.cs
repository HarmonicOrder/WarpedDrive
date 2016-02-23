using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class AbilityManager : MonoBehaviour {

    public Button Suspend, Overclock, Fork, Multithread;
    public Text HoverText, HoverTitle;
    private Dictionary<string, AbilityViewModel> Abilities = new Dictionary<string, AbilityViewModel>();

    void Awake()
    {
        HoverText.enabled = false;
        HoverTitle.enabled = false;
        Abilities.Add("suspend", new AbilityViewModel(Suspend,"suspend",
@"
<color=grey>1.5m refresh</color>
<color=green>75% Suspend for 5s</color>
<color=#a00>25% Block 100% for 5s</color>"));
        Abilities.Add("overclock", new AbilityViewModel(Overclock,"overclock",
@"
<color=grey>3m refresh</color>
<color=green>65% +1 Core for 1m</color>
<color=#a00>35% -1 Core for 1m</color>"));
        Abilities.Add("fork", new AbilityViewModel(Fork,"fork",
@"
<color=grey>2m refresh</color>
<color=green>75% Clone Subroutine</color>
<color=#a00>25% Spawn Virus</color>"));
        Abilities.Add("multithread", new AbilityViewModel(Multithread,"multithread",
@"
<color=grey>1m refresh</color>
<color=green>66% Rate +50% for 5s</color>
<color=#a00>33% Frozen for 5s</color>"));
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
        HoverTitle.enabled = true;
        HoverText.text = Abilities[name].HoverText;
        HoverTitle.text = Abilities[name].HoverTitle;
    }

    public void OnButtonHoverOff(string name)
    {
        HoverText.enabled = false;
        HoverTitle.enabled = false;
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
        public string HoverText;
        public string HoverTitle;

        public AbilityViewModel(Button b, string name, string hoverBody)
        {
            this.Button = b;
            this.Image = b.GetComponent<Image>();
            this.HoverText = hoverBody;
            this.HoverTitle = String.Format(@"<color=green>{0}</color>", name.ToUpper());
        }
    }

}
