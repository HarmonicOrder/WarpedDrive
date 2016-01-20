using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;
using Prime31.TransitionKit;
using System;

public class BasicWASD : MonoBehaviour {

    public Animator animator;
    public Camera TerminalCamera;
    public UnityStandardAssets.ImageEffects.BlurOptimized BlurEffect;
    public Terminal T;
    public UnityEngine.UI.Text ClockText;
    public UnityEngine.UI.Image ClockPanel;
    public ParticleSystem Immature;
    public RectTransform AreYouSurePanel;
    public RectTransform QuitButton;

    private bool IsUsingTerminal = false;
    private bool IsShowingMenu = false;

    // Use this for initialization
    void Start () {
		Cursor.visible = false;
        Radio.Instance.SetSoundtrack(Radio.Soundtrack.Spacey);
        BlurEffect.enabled = false;
        TerminalCamera.enabled = false;
        Autosave.Instance.On = true;
        //while in meatspace, consume oxygen
        OxygenConsumer.Instance.IsConsuming = true;
        OxygenConsumer.Instance.IsConsumingSlowly = false;
        QuitButton.gameObject.SetActive(false);
        AreYouSurePanel.gameObject.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
        //cheat! tilde to get instant 5 mins of oxygen
        if (Input.GetKeyUp(KeyCode.BackQuote))
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                StarshipEnvironment.Instance.OxygenLevel -= .0025f;
            }
            else
            {
                StarshipEnvironment.Instance.OxygenLevel += .0025f;
            }
        }

        //hypersleep
        if (Input.GetKeyUp(KeyCode.C))
        {

        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            StartCoroutine(BeImmature());
        }

        bool escapeUp = Input.GetKeyUp(KeyCode.Escape);
        if (Input.GetKeyUp(KeyCode.E) && (IsUsingTerminal || TerminalManager.IsNextToTerminal))
        {
            ToggleTerminal();
        }
        else if (IsUsingTerminal && escapeUp)
        {
            ToggleTerminal();
        }
        else if (escapeUp)
        {
            ToggleMenu();
        }

        if (!IsUsingTerminal)
        {
		    float h, v;
		    h = CrossPlatformInputManager.GetAxis("Horizontal");
		    v = CrossPlatformInputManager.GetAxis("Vertical")  ;

            if (v > 0)
            {
                animator.SetInteger("Direction", 2);
            }
            else if (v < 0)
            {
                animator.SetInteger("Direction", 0);
            }
            else if (h > 0)
            {
                animator.SetInteger("Direction", 3);
            }
            else if (h < 0)
            {
                animator.SetInteger("Direction", 1);
            }
            animator.SetBool("isWalking", (h != 0) || (v != 0));

            Vector3 vec = new Vector3(h, v, 0f);
		    vec.Normalize();
		    vec.x *= Time.deltaTime * 5f;
		    vec.y *= Time.deltaTime * 5f;
		    this.transform.Translate(vec);
        }

		if (CrossPlatformInputManager.GetButtonDown("Jump") && !HarmonicSerialization.Instance.IsNewGame){
			var pixelater = new PixelateTransition()
			{
				nextScene = 2,
				finalScaleEffect = PixelateTransition.PixelateFinalScaleEffect.ToPoint,
				duration = 1.0f
			};
			TransitionKit.instance.transitionWithDelegate( pixelater );
		}

        ClockText.text = HarmonicUtils.ClockFormat((float)StarshipEnvironment.Instance.SecondsTilOxygenRunsOut);
	}

    private IEnumerator BeImmature()
    {
        Immature.Play();
        yield return new WaitForSeconds(1f);
        Immature.Stop();
    }

    private void ToggleTerminal()
    {
        IsUsingTerminal = !IsUsingTerminal;

        SetTerminalState(IsUsingTerminal);
    }

    private void SetTerminalState(bool usingTerminal)
    {
        BlurEffect.enabled = usingTerminal;
        TerminalCamera.enabled = usingTerminal;
        ClockPanel.enabled = !usingTerminal;

        if (usingTerminal)
        {
            T.ShowStatus();
        }
        else
        {
            T.StopStatus();
        }
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        print("colliding!");
    }

    public void QuitClick()
    {
        AreYouSurePanel.gameObject.SetActive(true);
    }

    public void AreYouSureQuit()
    {
        Application.Quit();
    }

    public void CancelQuit()
    {
        AreYouSurePanel.gameObject.SetActive(false);
        ToggleMenu();
    }

    private void ToggleMenu()
    {
        IsShowingMenu = !IsShowingMenu;
        Cursor.visible = IsShowingMenu;
        QuitButton.gameObject.SetActive(IsShowingMenu);

        OxygenConsumer.Instance.IsConsuming = !IsShowingMenu;
    }
}
