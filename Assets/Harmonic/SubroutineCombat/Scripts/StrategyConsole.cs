using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StrategyConsole : MonoBehaviour {

	public static StrategyConsole Instance {get;set;}

	public string PinnedTextLine;
	public Text consoleOut;
	public string LineStartString = "> ";

	// Use this for initialization
	void Start () {
		Instance = this;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void WriteLines(string lines)
	{
		consoleOut.text = LineStartString + PinnedTextLine + '\n'+ lines;
	}
}
