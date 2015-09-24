using UnityEngine;
using System.Collections;
using Prime31.TransitionKit;

public class MainMenuInput : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void NewGame()
	{
		var pixelater = new PixelateTransition()
		{
			finalScaleEffect = PixelateTransition.PixelateFinalScaleEffect.ToPoint,
			duration = 1.0f
		};
		pixelater.nextScene = 1;
		TransitionKit.instance.transitionWithDelegate( pixelater );
		return;
	}
}
