using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;
using Prime31.TransitionKit;
using System;

public class BasicWASD : MonoBehaviour {

    public Animator animator, headlightAnimator;
    public Camera TerminalCamera;
    public UnityStandardAssets.ImageEffects.BlurOptimized BlurEffect;
    public Terminal Term;
    public Generator Gen;
    public UnityEngine.UI.Text ClockText;
    public UnityEngine.UI.Image ClockFill;
    public ParticleSystem Immature;
    public RectTransform AreYouSurePanel;
    public RectTransform QuitButton;
    public Canvas UICanvas;

    private bool IsUsingTerminal = false;
    private bool IsShowingMenu = false;
    private FocusState CurrentFocus;
    private enum FocusState { None, Terminal, Generator }

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
        Gen.transform.parent.gameObject.SetActive(false);
        Term.transform.parent.gameObject.SetActive(false);
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
        if (Input.GetKeyUp(KeyCode.E))
        {
            if (TerminalManager.IsNextToTerminal)
            {
                SetFocus(FocusState.Terminal);
            }
            else if (TerminalManager.IsNextToGenerator)
            {
                SetFocus(FocusState.Generator);
            }
        }
        else if (CurrentFocus != FocusState.None && escapeUp)
        {
            SetFocus(FocusState.None);
        }
        else if (escapeUp)
        {
            ToggleMenu();
        }

        if (CurrentFocus == FocusState.None)
        {
		    float h, v;
		    h = CrossPlatformInputManager.GetAxis("Horizontal");
		    v = CrossPlatformInputManager.GetAxis("Vertical")  ;
            //print("h:" + h + " v:" + v);
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
            headlightAnimator.SetFloat("BlendX", Mathf.Clamp(h, -1f, 1f));
            headlightAnimator.SetFloat("BlendY", Mathf.Clamp(v, -1f, 1f));
            animator.SetBool("isWalking", (h != 0) || (v != 0));

            Vector3 vec = new Vector3(h, v, 0f);
		    vec.Normalize();
		    vec.x *= Time.deltaTime * 5f;
		    vec.y *= Time.deltaTime * 5f;
		    this.transform.Translate(vec);
        }

		if (CrossPlatformInputManager.GetButtonDown("Jump") && !HarmonicSerialization.Instance.IsNewGame){
            UICanvas.enabled = false;
			var pixelater = new PixelateTransition()
			{
				nextScene = 2,
				finalScaleEffect = PixelateTransition.PixelateFinalScaleEffect.ToPoint,
				duration = 1.0f
			};
			TransitionKit.instance.transitionWithDelegate( pixelater );
		}

        ClockText.text = HarmonicUtils.ClockFormat((float)StarshipEnvironment.Instance.SecondsTilOxygenRunsOut);
        ClockFill.fillAmount = (float)(StarshipEnvironment.Instance.OxygenLevel / StarshipEnvironment.Instance.OxygenStorage);
	}

    private IEnumerator BeImmature()
    {
        Immature.Play();
        yield return new WaitForSeconds(1f);
        Immature.Stop();
    }

    private void SetFocus(FocusState contextual)
    {
        if (CurrentFocus == contextual)
        {
            CurrentFocus = FocusState.None;
        }
        else
        {
            CurrentFocus = contextual;
        }

        SetTerminalState(CurrentFocus);
    }

    private void SetTerminalState(FocusState st)
    {
        BlurEffect.enabled = st != FocusState.None;
        TerminalCamera.enabled = st != FocusState.None;

        if (st == FocusState.Terminal)
        {
            Term.transform.parent.gameObject.SetActive(true);
            Term.ShowStatus();
        }
        else if (st == FocusState.Generator)
        {
            Gen.transform.parent.gameObject.SetActive(true);

        }
        else
        {
            Gen.transform.parent.gameObject.SetActive(false);
            Term.transform.parent.gameObject.SetActive(false);
            Term.StopStatus();
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
