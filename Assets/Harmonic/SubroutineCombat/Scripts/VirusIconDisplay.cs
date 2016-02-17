using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class VirusIconDisplay : MonoBehaviour {

    public static VirusIconDisplay Instance { get; set; }
    public UnityEngine.UI.Text VirusAmount;
    public UnityEngine.UI.Text RansomwareAmount;
    public UnityEngine.UI.Text BombAmount;
    public UnityEngine.UI.Text WabbitAmount;
    public UnityEngine.UI.Text SpawnerAmount;
    public UnityEngine.UI.Text TankAmount;
    public UnityEngine.UI.Text TrojanAmount;
    public UnityEngine.UI.Text StealthAmount;

    // Use this for initialization
    void Awake ()
    {
        DisableIcons();
        Instance = this;
	}

    internal void UpdateIcon(IMalware second, int first)
    {
        switch(second.Type)
        {
            case VirusAI.VirusType.Virus:
                UpdateIcon(VirusAmount, first);
                break;
            case VirusAI.VirusType.Wabbit:
                UpdateIcon(WabbitAmount, first);
                break;
            case VirusAI.VirusType.Trojan:
                UpdateIcon(TrojanAmount, first);
                break;
            case VirusAI.VirusType.Tank:
                UpdateIcon(TankAmount, first);
                break;
            case VirusAI.VirusType.Spawner:
                UpdateIcon(SpawnerAmount, first);
                break;
            case VirusAI.VirusType.Ransomware:
                UpdateIcon(RansomwareAmount, first);
                break;
            case VirusAI.VirusType.Bomb:
                UpdateIcon(BombAmount, first);
                break;
            case VirusAI.VirusType.Stealth:
                UpdateIcon(StealthAmount, first);
                break;
        }
    }

    private void UpdateIcon(Text virusAmount, int amountOfVirus)
    {
        if (amountOfVirus == 0)
        {
            virusAmount.rectTransform.parent.gameObject.SetActive(false);
        }
        else
        {
            virusAmount.rectTransform.parent.gameObject.SetActive(true);
            virusAmount.text = amountOfVirus.ToString();
        }
    }

    internal void DisableIcons()
    {
        VirusAmount.rectTransform.parent.gameObject.SetActive(false);
        WabbitAmount.rectTransform.parent.gameObject.SetActive(false);
        TrojanAmount.rectTransform.parent.gameObject.SetActive(false);
        TankAmount.rectTransform.parent.gameObject.SetActive(false);
        SpawnerAmount.rectTransform.parent.gameObject.SetActive(false);
        RansomwareAmount.rectTransform.parent.gameObject.SetActive(false);
        BombAmount.rectTransform.parent.gameObject.SetActive(false);
        StealthAmount.rectTransform.parent.gameObject.SetActive(false);
    }
}
