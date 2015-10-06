using UnityEngine;
using System.Collections;

public class Scaler : MonoBehaviour {

	public bool scaleUp = false;
	public float duration = 1f;
	private float scaleTime = 0f;
	
	// Update is called once per frame
	void Update () {
		if (scaleUp){
			scaleTime += Time.deltaTime;

			if (scaleTime > duration)
				scaleUp = false;
			else 
				this.transform.localScale = Vector3.Lerp(Vector3.forward, new Vector3(1f, 1f, 1f), Mathf.Min(1, scaleTime / duration));
		}
	}
}
