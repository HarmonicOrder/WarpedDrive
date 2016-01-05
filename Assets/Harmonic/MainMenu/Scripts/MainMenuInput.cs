using UnityEngine;
using System.Collections;
using Prime31.TransitionKit;
using UnityEngine.UI;

public class MainMenuInput : MonoBehaviour {
	public Texture2D cursorTexture;
    public Button LoadGameButton;
    public Canvas MainmenuCanvas;
    public Canvas SignupCanvas;
    public TextMesh TitleText;
    public InputField NewGameName;

	// Use this for initialization
	void Start () {
		Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);
        Cursor.visible = true;
        LoadGameButton.gameObject.SetActive(HarmonicSerialization.Instance.HasContinueGame);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void NewGame()
	{
        //StartGame();
        ShowSignupForm();
	}

    public void ContinueGame()
    {
        HarmonicSerialization.Instance.ContinueOnLastSavedGame();
        StartGame();
    }

    public void ShowSignupForm()
    {
        TitleText.gameObject.SetActive(false);
        MainmenuCanvas.enabled = false;
        SignupCanvas.enabled = true;
    }

    public void CompleteSignupForm()
    {
        HarmonicSerialization.Instance.CreateNewGame(NewGameName.text);
        StartGame();
    }

    public void StartGame()
    {
        var pixelater = new PixelateTransition()
        {
            finalScaleEffect = PixelateTransition.PixelateFinalScaleEffect.ToPoint,
            duration = 1.0f
        };
        pixelater.nextScene = 1;
        TransitionKit.instance.transitionWithDelegate(pixelater);
        return;
    }
}
