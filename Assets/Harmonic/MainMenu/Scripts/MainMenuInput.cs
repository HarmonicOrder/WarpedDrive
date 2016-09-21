using UnityEngine;
using System.Collections;
using Prime31.TransitionKit;
using UnityEngine.UI;

public class MainMenuInput : MonoBehaviour
{
    public Texture2D cursorTexture;
    public Button LoadGameButton, LoadOtherGameButton;
    public RectTransform NewGameButton;
    public Canvas MainmenuCanvas;
    public Canvas SignupCanvas;
    public TextMesh TitleText;
    public InputField NewGameName;
    public Transform LoadGameButtons, SettingsPanel, GamePanel;
    public Sprite SpeakerOnSprite, SpeakerOffSprite;
    public Image MusicImage, EffectsImage;
    public Slider MusicSlider, EffectsSlider;
    
    // Use this for initialization
    void Start()
    {
        Cursor.SetCursor(cursorTexture, new Vector2(cursorTexture.width / 2, cursorTexture.height / 2), CursorMode.Auto);
        Cursor.visible = true;
        LoadGameButton.gameObject.SetActive(HarmonicSerialization.Instance.HasContinueGame);
        LoadOtherGameButton.gameObject.SetActive(HarmonicSerialization.Instance.HasContinueGame);
        SettingsPanel.gameObject.SetActive(false);

        RefreshMusicEffectsUI("Music", MusicImage, MusicSlider);
        RefreshMusicEffectsUI("SoundFX", EffectsImage, EffectsSlider);

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
    void Update()
    {

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
    public void ToggleSettings()
    {
        bool newSettingsPanelVisibility = !SettingsPanel.gameObject.activeSelf;

        SettingsPanel.gameObject.SetActive(newSettingsPanelVisibility);
        GamePanel.gameObject.SetActive(!newSettingsPanelVisibility);
    }

    private void ToggleMusicEffectsPreference(string key, Image icon, Slider slider)
    {
        float newValue = 0f;

        if (PlayerPrefs.HasKey(key))
        {
            if (PlayerPrefs.GetFloat(key) > 0)
            {
                newValue = 0f;
            }
            else
            {
                newValue = 1f;
            }
        }

        PlayerPrefs.SetFloat(key, newValue);

        RefreshMusicEffectsUI(key, icon, slider);
    }

    private void RefreshMusicEffectsUI(string key, Image icon, Slider slider)
    {
        float value = GetValueWithDefault(key);

        if (value > 0f)
        {
            icon.sprite = SpeakerOnSprite;
        }
        else
        {
            icon.sprite = SpeakerOffSprite;
        }

        if (slider != null)
            slider.value = value;
    }

    private static float GetValueWithDefault(string key)
    {
        float value = 1f; //default to 1
        if (PlayerPrefs.HasKey(key))
        {
            value = PlayerPrefs.GetFloat(key);
        }

        return value;
    }

    public void ToggleMusic()
    {
        ToggleMusicEffectsPreference("Music", MusicImage, MusicSlider);
    }

    public void ToggleEffects()
    {
        //probably should name this Effects, but oh well
        ToggleMusicEffectsPreference("SoundFX", EffectsImage, EffectsSlider);
    }

    private void SliderValueChange(string key, Slider slider, Image image, float value)
    {
        PlayerPrefs.SetFloat(key, value);
        RefreshMusicEffectsUI(key, image, null);
        Radio.Instance.RefreshVolume();
    }

    public void MusicSliderValueChange(float value)
    {
        SliderValueChange("Music", MusicSlider, MusicImage, value);
    }

    public void EffectsSliderValueChange(float value)
    {
        SliderValueChange("SoundFX", EffectsSlider, EffectsImage, value);
    }
}
