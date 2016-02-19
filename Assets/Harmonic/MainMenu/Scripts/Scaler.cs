using UnityEngine;
using System.Collections;

public class Scaler : MonoBehaviour {

	public bool scaleUp  = false;
    public bool loop = false;
	public float duration = 1f;
	public Vector3 scaleFrom = Vector3.forward;
	public Vector3 scaleTo = Vector3.one;

	private float scaleTime = 0f;
    public float scaleTimeOffset = 0f;

	void Start()
	{
        this.scaleTime = this.scaleTimeOffset;
        this.transform.localScale = Vector3.Lerp(scaleFrom, scaleTo, Mathf.Min(1, scaleTime / duration));
        //this.transform.localScale = scaleFrom;
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
            scaleTime += InterruptTime.deltaTime;

			if (scaleTime > duration)
            {
                if (loop)
                {
                    this.transform.localScale = scaleFrom;
                    scaleTime = 0f;
                }
                else
                {
				    scaleUp = false;
                }
            }
			else 
				this.transform.localScale = Vector3.Lerp(scaleFrom, scaleTo, Mathf.Min(1, scaleTime / duration));
		}
	}
}
