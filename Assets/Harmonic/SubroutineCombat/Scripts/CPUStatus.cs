using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class CPUStatus : MonoBehaviour {
	public float MaxWidth = 500f;
	public Text CPUTextLine;
	public RectTransform CPUBar, InUse, NotInUse, Occupied, Infected;
    public Image CPUUsedImage;
    public Image CPUUsedByVirusImage;

    private List<RectTransform> InUses, NotInUses, Occupieds, Infecteds;

	// Use this for initialization
	void Start () {
		CyberspaceBattlefield.Current.OnCoreChange += OnCPUChange;
		OnCPUChange();
        InUses = new List<RectTransform>();
        NotInUses = new List<RectTransform>();
        Occupieds = new List<RectTransform>();
        Infecteds = new List<RectTransform>();
    }

	public void OnCPUChange()
	{
        string newCPUString = string.Format("{0} / {1} / {2} CPU ¢ores", CyberspaceBattlefield.Current.UsedCores, CyberspaceBattlefield.Current.CurrentCores, CyberspaceBattlefield.Current.TotalCores);
		CPUTextLine.text = newCPUString;
        //float denom = Mathf.Max(CyberspaceBattlefield.Current.CurrentCores, 1f);
        //SetBarByPercentage(CyberspaceBattlefield.Current.UsedCores / denom);
        //SetCPUBarImageFilled();
        RefreshCPURectangles();
	}

    private void RefreshCPURectangles()
    {
        print("Refreshing cpus");
        foreach(RectTransform t in this.transform)
        {
            
        }
    }

    private void SetCPUBarImageFilled()
    {
        float denom = Mathf.Max(CyberspaceBattlefield.Current.CurrentCores, 1f);
        CPUUsedImage.fillAmount = (CyberspaceBattlefield.Current.UsedCores - CyberspaceBattlefield.Current.StolenCores) / denom;

        if (CyberspaceBattlefield.Current.StolenCores > 0)
        {
            CPUUsedByVirusImage.fillAmount = (CyberspaceBattlefield.Current.UsedCores) / denom;
        }
        else
        {
            CPUUsedByVirusImage.fillAmount = 0;
        }
    }

    private void SetBarByPercentage(float percentage){
		float newWidth = MaxWidth * percentage;
		CPUBar.anchoredPosition = new Vector2(newWidth / 2, 0);
		CPUBar.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, newWidth);
	}

    void OnDestroy()
    {
        CyberspaceBattlefield.Current.OnCoreChange -= OnCPUChange;
    }
}
