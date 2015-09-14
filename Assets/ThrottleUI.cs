using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ThrottleUI : MonoBehaviour {
	public Text ThrottleText;
	public CyberspaceShellInput Shell;
	public int numBars = 20;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		ThrottleText.text = GetThrottleText();
	}

	private string GetThrottleText()
	{
		int currentBars = (int)Mathf.Floor( 0); //Shell.CurrentThrottle * numBars );
		return string.Format("{0}", "".PadLeft(currentBars, (char)9608));
	}
}

