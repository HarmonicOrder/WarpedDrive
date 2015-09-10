using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class OxygenTimer : MonoBehaviour {
	public Text OxygenText;

	public float secondsLeft = 88f; //just some arbitrary constant

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		OxygenText.text = string.Format("O2 {0}", GetTimeString());
		secondsLeft -= Time.deltaTime;
	}

	private string GetTimeString(){
		return string.Format("{0}:{1}", Mathf.Floor(secondsLeft / 60).ToString("00"), (Mathf.Floor(secondsLeft % 60)).ToString("00"));
	}
}
