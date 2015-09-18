using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


public class playerHealth : MonoBehaviour {

	public float RecoveryTime = 2f;
	public float MaximumHealth = 10f;
	public float HealthAmount = 1f;
	public float GlitchFade = 1f;
	public float InvulnerabilityTime = 1f;
	public List<Image> GlitchSpots = new List<Image>();

	private float CurrentHealth;
	private float CurrentAlpha = 0;
	private float DestinationAlpha = 1;
	private bool AlphaMoving = false;
	private float CurrentGlitchFade;

	// Use this for initialization
	void Start () {
		CurrentHealth = MaximumHealth;
	}
	
	private float timeSinceLastDamage = 999f;
	void LateUpdate () {
		if (CurrentHealth < MaximumHealth)
		{
			timeSinceLastDamage += Time.deltaTime;
			if (timeSinceLastDamage > RecoveryTime)
			{
				HealDamage(HealthAmount);
				timeSinceLastDamage = 999f;
			}
		}
		if (AlphaMoving)
		{
			//print (CurrentAlpha.ToString());
			SetAlpha(Mathf.Lerp(CurrentAlpha, DestinationAlpha, CurrentGlitchFade / GlitchFade));
			CurrentGlitchFade += Time.deltaTime;
			if (CurrentGlitchFade > GlitchFade){
				AlphaMoving = false;
			}
		}
	}


	public void TakeDamage(float damage)
	{
		if (timeSinceLastDamage > InvulnerabilityTime)
		{
			CurrentHealth -= damage;
			if (CurrentHealth < 0f)
			{
				//game over!
			}
			else
			{
				timeSinceLastDamage = 0f;
			}
			DestinationAlpha = 1 - CurrentHealth / MaximumHealth;
			AlphaMoving = true;
			CurrentGlitchFade = 0;
		}
	}

	public void HealDamage(float healAmount)
	{
		CurrentHealth += healAmount;

		CurrentHealth = Mathf.Min(CurrentHealth, MaximumHealth);
		DestinationAlpha = 1 - CurrentHealth / MaximumHealth;
		AlphaMoving = true;
		CurrentGlitchFade = 0;
	}



	private void SetAlpha(float alpha)
	{
		CurrentAlpha = alpha;
		foreach(Image i in GlitchSpots)
		{
			i.color = new Color(i.color.r, i.color.g, i.color.b, alpha);
		}
	}
}
