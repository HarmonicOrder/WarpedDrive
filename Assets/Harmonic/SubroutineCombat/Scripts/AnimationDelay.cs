using UnityEngine;
using System.Collections;
using System;

public class AnimationDelay : MonoBehaviour {

    public float Delay = 1f;

	// Use this for initialization
	void Start () {
        this.GetComponent<Animation>().playAutomatically = false;
        StartCoroutine(WaitThenPlay());
	}

    private IEnumerator WaitThenPlay()
    {
        yield return new WaitForSeconds(Delay);
        this.GetComponent<Animation>().Play();
        this.enabled = false;
    }
}
