using UnityEngine;
using System.Collections;

public class Scaler : MonoBehaviour {

	public bool scaleUp = false;
	public float duration = 1f;
	public float scaleTo = 1f;
	public Vector3 ScaleAxis = Vector3.forward;

	private float scaleTime = 0f;

	void Start()
	{
		this.transform.localScale = Vector3.forward;
	}

	// Update is called once per frame
	void Update () {
		if (scaleUp){
			scaleTime += Time.deltaTime;

			if (scaleTime > duration)
				scaleUp = false;
			else 
				this.transform.localScale = Vector3.Lerp(ScaleAxis, new Vector3(scaleTo, scaleTo, scaleTo), Mathf.Min(1, scaleTime / duration));
		}
	}
}
