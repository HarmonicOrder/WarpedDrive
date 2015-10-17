using UnityEngine;
using System.Collections;

public class CPUStatus : MonoBehaviour {
	public float MaxWidth = 500f;
	public RectTransform CPUBar;

	// Use this for initialization
	void Start () {
		CyberspaceBattlefield.Current.OnCoreChange += OnCPUChange;
	}

	public void OnCPUChange()
	{
		string newCPUString = string.Format("{0} / {1} / {2} CPU Cores", CyberspaceBattlefield.Current.UsedCores, CyberspaceBattlefield.Current.CurrentCores, CyberspaceBattlefield.Current.TotalCores);
		SetBarByPercentage(CyberspaceBattlefield.Current.UsedCores / (float)CyberspaceBattlefield.Current.CurrentCores);
	}
	
	
	private void SetBarByPercentage(float percentage){
		float newWidth = MaxWidth * percentage;
		CPUBar.anchoredPosition = new Vector2(newWidth / 2, 0);
		CPUBar.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, newWidth);
	}
}
