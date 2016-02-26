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
        Abilities.Add("suspend", new AbilityViewModel(Suspend, Ability.ByName("suspend"),
@"
<color=grey>1.5m refresh</color>
<color=green>75% Suspend for 5s</color>
<color=#a00>25% Block 100% for 5s</color>"));
        Abilities.Add("overclock", new AbilityViewModel(Overclock, Ability.ByName("overclock"),
@"
<color=grey>3m refresh</color>
<color=green>65% +1 Core for 1m</color>
<color=#a00>35% -1 Core for 1m</color>"));
        Abilities.Add("fork", new AbilityViewModel(Fork, Ability.ByName("fork"),
@"
<color=grey>2m refresh</color>
<color=green>75% Clone Subroutine</color>
<color=#a00>25% Spawn Virus</color>"));
        Abilities.Add("multithread", new AbilityViewModel(Multithread, Ability.ByName("multithread"),
@"
<color=grey>1m refresh</color>
<color=green>66% Rate +50% for 5s</color>
<color=#a00>33% Frozen for 5s</color>"));
    }

    public void Activate(string name)
    {
        print(CyberspaceDroneInput.CurrentLock);

        AbilityViewModel avm = Abilities[name];
        if (avm.Countdown <= 0 && avm.Model.CanActivate(CyberspaceDroneInput.CurrentLock))
        {
            avm.Countdown = avm.Cooldown;
            avm.Button.interactable = false;
            avm.Model.Activate(CyberspaceDroneInput.CurrentLock);
            avm.CurrentEffect = StartCoroutine(CountdownToEffectEnd(avm));
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

    private IEnumerator CountdownToEffectEnd(AbilityViewModel avm)
    {
        yield return new WaitForSecondsInterruptTime(avm.Model.Duration);
        avm.Model.Deactivate();
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

    void OnDestroy()
    {
        foreach(AbilityViewModel avm in Abilities.Values)
        {
            if (avm.CurrentEffect != null)
                StopCoroutine(avm.CurrentEffect);
        }
    }

    private class AbilityViewModel
    {
        public float Cooldown;
        public float Countdown = 0f;
        public Image Image;
        public Button Button;
        public string HoverText;
        public string HoverTitle;
        public Ability Model;
        public Coroutine CurrentEffect;

        public AbilityViewModel(Button b, Ability a, string hoverBody)
        {
            this.Model = a;
            this.Cooldown = a.Cooldown;
            this.Button = b;
            this.Image = b.GetComponent<Image>();
            this.HoverText = hoverBody;
            this.HoverTitle = String.Format(@"<color=green>{0}</color>", a.Name.ToUpper());
        }
    }

}
