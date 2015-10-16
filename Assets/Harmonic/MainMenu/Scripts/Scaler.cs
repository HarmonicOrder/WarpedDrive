using UnityEngine;
using System.Collections;

public class Scaler : MonoBehaviour {

	public bool scaleUp  = false;
	public float duration = 1f;
	public Vector3 scaleFrom = Vector3.forward;
	public Vector3 scaleTo = Vector3.one;

	private float scaleTime = 0f;

	void Start()
	{
		this.transform.localScale = scaleFrom;
	}

	public void ScaleAgain()
	{
		this.transform.localScale = scaleFrom;
		scaleUp = true;
		scaleTime = 0f;
	}


	// Update is called once per frame
	void Update () {
		if (scaleUp){
			scaleTime += Time.deltaTime;

			if (scaleTime > duration)
				scaleUp = false;
			else 
				this.transform.localScale = Vector3.Lerp(scaleFrom, scaleTo, Mathf.Min(1, scaleTime / duration));
		}
	}
}
