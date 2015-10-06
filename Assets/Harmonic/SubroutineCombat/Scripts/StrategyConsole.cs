using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public static class StrategyConsole {

	public static string PinnedTextLine = "WASD to Pan, R-Click to Zoom";
	public static string LineStartString = "> ";

	private static Text consoleOut;

	// Use this for initialization
	public static void Initialize(Text currentUIElement) {
		consoleOut = currentUIElement;
	}


	public static void WriteLines(string lines)
	{
		if (consoleOut != null)
			consoleOut.text = LineStartString + PinnedTextLine + '\n'+ lines;
	}
}
