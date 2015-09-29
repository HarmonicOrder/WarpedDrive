using UnityEngine;
using System.Collections;

public class ExplosionScalerAndFader : MonoBehaviour {
	public float SelfDestructTime = 5f;
	public Transform ScaleTransform;
	public float ScaleCoeff = 1f;
	private float TimeSpentScaling = 0f;
	public SpriteRenderer FadeSprite;
	public float FadeCoeff = 1f;
	private float TimeSpentFading = 0f;

	// Use this for initialization
	void Start () {
		StartCoroutine(SelfDestruct());
	}
	
	// Update is called once per frame
	void Update () {
		if (ScaleTransform != null)
		{
			TimeSpentScaling += Time.deltaTime;
			float scaleFactor = Mathf.Pow( ScaleCoeff, TimeSpentScaling );
			ScaleTransform.localScale = new Vector3( scaleFactor, scaleFactor, scaleFactor);
			print (scaleFactor);
		}

		if(FadeSprite != null)
		{
			TimeSpentFading += Time.deltaTime;
			FadeSprite.color = new Color(
				FadeSprite.color.r,
				FadeSprite.color.g,
				FadeSprite.color.b,
				Mathf.Max(1 * TimeSpentFading * TimeSpentFading * FadeCoeff, 255f)
				);
		}	
	}

	public IEnumerator SelfDestruct()
	{
		yield return new WaitForSeconds(this.SelfDestructTime);
		GameObject.Destroy(this.gameObject);
	}
}
