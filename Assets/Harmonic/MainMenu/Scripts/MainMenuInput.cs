using UnityEngine;
using System.Collections;
using Prime31.TransitionKit;
using UnityEngine.UI;

public class MainMenuInput : MonoBehaviour {
	public Texture2D cursorTexture;
    public Button LoadGameButton, LoadOtherGameButton;
    public RectTransform NewGameButton;
    public Canvas MainmenuCanvas;
    public Canvas SignupCanvas;
    public TextMesh TitleText;
    public InputField NewGameName;
    public Transform LoadGameButtons;

    // Use this for initialization
    void Start() {
        Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);
        Cursor.visible = true;
        LoadGameButton.gameObject.SetActive(HarmonicSerialization.Instance.HasContinueGame);
        LoadOtherGameButton.gameObject.SetActive(HarmonicSerialization.Instance.HasContinueGame);

        if (HarmonicSerialization.Instance.HasContinueGame)
        {
            //put the name on the button so the user knows which game they're loading
            LoadGameButton.GetComponentInChildren<Text>().text = "Continue as " + HarmonicSerialization.Instance.ContinueSave.Name;
        }
        else
        {
            NewGameButton.anchoredPosition = new Vector2(0f, -100f);
        }

        for (int i = 0; i < 8; i++)
        {
            Button b = GameObject.Find("Game" + i).GetComponent<Button>();
            if (i >= HarmonicSerialization.Instance.configuration.SavedGames.Count)
            {
                b.gameObject.SetActive(false);
            }
        }
        LoadGameButtons.gameObject.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
	
	}

	public void NewGame()
	{
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

    public void Exit()
    {
        Application.Quit();
    }

    public void OpenLoadButtons()
    {
        LoadGameButtons.gameObject.SetActive(true);
        for (int i = 0; i < HarmonicSerialization.Instance.configuration.SavedGames.Count; i++)
        {
            Button b = GameObject.Find("Game" + i).GetComponent<Button>();
            Text t = b.gameObject.GetComponentInChildren<Text>();
            t.text = HarmonicSerialization.Instance.configuration.SavedGames[i];
            b.onClick.AddListener(() =>
            {
                string loadedGame = t.text;
                print("loading game: " + loadedGame);
                HarmonicSerialization.Instance.LoadSaveGame(loadedGame);
                StartGame();
            });
        }
    }
}
