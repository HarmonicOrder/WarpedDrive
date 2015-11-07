using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CPUStatus : MonoBehaviour {
	public float MaxWidth = 500f;
	public Text CPUTextLine;
	public RectTransform CPUBar;

	// Use this for initialization
	void Start () {
		CyberspaceBattlefield.Current.OnCoreChange += OnCPUChange;
		OnCPUChange();
	}

	public void OnCPUChange()
	{
        string newCPUString = string.Format("{0} / {1} / {2} CPU ¢ores", CyberspaceBattlefield.Current.UsedCores, CyberspaceBattlefield.Current.CurrentCores, CyberspaceBattlefield.Current.TotalCores);
		float denom = Mathf.Max(CyberspaceBattlefield.Current.CurrentCores, 1f);
		SetBarByPercentage(CyberspaceBattlefield.Current.UsedCores / denom);
		CPUTextLine.text = newCPUString;
	}
	
	
	private void SetBarByPercentage(float percentage){
		float newWidth = MaxWidth * percentage;
		CPUBar.anchoredPosition = new Vector2(newWidth / 2, 0);
		CPUBar.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, newWidth);
	}
}
