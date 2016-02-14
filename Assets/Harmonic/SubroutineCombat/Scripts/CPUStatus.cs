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

        InUses = new List<RectTransform>() { InUse };
        NotInUses = new List<RectTransform>() { NotInUse };
        Occupieds = new List<RectTransform>() { Occupied };
        Infecteds = new List<RectTransform>() { Infected };

		OnCPUChange();
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
        int usedByPlayerCoreNumbers = CyberspaceBattlefield.Current.UsedCores - CyberspaceBattlefield.Current.StolenCores;
        RefreshCPURectangleType(InUses, InUse, usedByPlayerCoreNumbers);
        RefreshCPURectangleType(NotInUses, NotInUse, CyberspaceBattlefield.Current.CurrentCores - CyberspaceBattlefield.Current.UsedCores);
        RefreshCPURectangleType(Occupieds, Occupied, CyberspaceBattlefield.Current.StolenCores);
        RefreshCPURectangleType(Infecteds, Infected, CyberspaceBattlefield.Current.TotalCores - CyberspaceBattlefield.Current.CurrentCores);
    }

    private void RefreshCPURectangleType(List<RectTransform> existingTransforms, RectTransform prototype, int numOfCores)
    {
        for (int i = 0; i < numOfCores + existingTransforms.Count; i++)
        {
            bool cpuStateIsOn = i < numOfCores;
            bool transformExists = i < existingTransforms.Count;

            if (transformExists)
            {
                existingTransforms[i].gameObject.SetActive(cpuStateIsOn);
            }
            else if (cpuStateIsOn)
            {
                existingTransforms.Add(GameObject.Instantiate<RectTransform>(prototype));
                existingTransforms[i].SetParent(this.transform);
                existingTransforms[i].localScale = Vector3.one;
                existingTransforms[i].localRotation = Quaternion.identity;
                existingTransforms[i].localPosition = Vector3.zero;
                existingTransforms[i].gameObject.SetActive(true);
                existingTransforms[i].SetSiblingIndex(prototype.GetSiblingIndex() + 1);
            }
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

    public CPUState[] GetStateArray()
    {
        CPUState[] result = new CPUState[CyberspaceBattlefield.Current.TotalCores];
        
        int usedByPlayerCoreNumbers = CyberspaceBattlefield.Current.UsedCores - CyberspaceBattlefield.Current.StolenCores;
        for (int i = 0; i < result.Length; i++)
        {
            if (i < usedByPlayerCoreNumbers)
            {
                result[i] = CPUState.Provisioned;
            }
            else if (i < usedByPlayerCoreNumbers + CyberspaceBattlefield.Current.CurrentCores)
            {
                result[i] = CPUState.Available;
            }
            else if (i < usedByPlayerCoreNumbers + CyberspaceBattlefield.Current.CurrentCores + CyberspaceBattlefield.Current.StolenCores)
            {
                result[i] = CPUState.Occupied;
            }
            else
            {
                result[i] = CPUState.Infected;
            }
        }

        return result;
    }

    public enum CPUState { Provisioned, Available, Occupied, Infected }
}
