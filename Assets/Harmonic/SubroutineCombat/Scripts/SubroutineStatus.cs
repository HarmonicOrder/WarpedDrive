using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SubroutineStatus : MonoBehaviour {
	public enum SubroutineSlot { Alpha, Beta, Gamma }

	public Image Status;
	public Sprite DisabledStatusSprite;
	public Sprite EnabledStatusSprite;
	public Sprite DeadStatusSprite;
	public Image RechargeBackground;
	public Image RechargeForeground;
	public SubroutineSlot Slot;

	public float MaxWidth = 196f;

	void Start(){
		RechargeBackground.enabled = false;
		RechargeForeground.enabled = false;
		Status.sprite = DisabledStatusSprite;
	}

	public void OnSubroutineActive()
	{
		RechargeBackground.enabled = true;
		RechargeForeground.enabled = true;
		Status.sprite = EnabledStatusSprite;
		SetBarByPercentage(1f);
	}

	public void OnSubroutineDead()
	{
		RechargeBackground.enabled = false;
		RechargeForeground.enabled = false;
		Status.sprite = DeadStatusSprite;
	}

	public void OnSubroutineTakeDamage(float currentHitpoints, float maxHitpoints)
	{
		SetBarByPercentage(currentHitpoints / maxHitpoints);
	}

	private void SetBarByPercentage(float percentage){
		float newWidth = MaxWidth * percentage;
		RechargeForeground.rectTransform.anchoredPosition = new Vector2(newWidth / 2, 0);
		RechargeForeground.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, newWidth);
	}
}
