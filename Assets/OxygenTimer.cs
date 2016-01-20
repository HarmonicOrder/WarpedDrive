using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class OxygenTimer : MonoBehaviour {
	public Text OxygenText;
    
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		OxygenText.text = string.Format("O2 {0}", HarmonicUtils.ClockFormatWithDecisecond((float)StarshipEnvironment.Instance.SecondsTilOxygenRunsOut));
	}
    
}
