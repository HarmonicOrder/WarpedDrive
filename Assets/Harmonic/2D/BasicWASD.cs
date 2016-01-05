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

    public MeshRenderer topSlit, bottomSlit;

    public bool TestNewGame;

    private bool IsUsingTerminal = false;

    void Awake()
    {
        if (TestNewGame)
            HarmonicSerialization.Instance.IsNewGame = true;

        if (HarmonicSerialization.Instance.IsNewGame)
        {
            topSlit.enabled = true;
            bottomSlit.enabled = true;
            StartCoroutine(MoveSlits());
        }
        else
        {
            DestroySlits();
        }
    }

    private IEnumerator MoveSlits()
    {
        yield return new WaitForSeconds(2f);
        float openTime = 0f;
        while (openTime < .5f)
        {
            topSlit.transform.Translate(Vector3.up * openTime * openTime, Space.World);
            bottomSlit.transform.Translate(Vector3.down * openTime * openTime, Space.World);
            openTime += Time.deltaTime;
            yield return null;
        }
        DestroySlits();
    }

    private void DestroySlits()
    {
        GameObject.Destroy(topSlit.gameObject);
        GameObject.Destroy(bottomSlit.gameObject);
    }

    // Use this for initialization
    void Start () {
		Cursor.visible = false;
        Radio.Instance.SetSoundtrack(Radio.Soundtrack.Spacey);
        BlurEffect.enabled = false;
        TerminalCamera.enabled = false;
        Autosave.Instance.On = true;
        //while in meatspace, consume oxygen
        OxygenConsumer.Instance.IsConsuming = true;
	}
	
	// Update is called once per frame
	void Update () {
        //cheat! tilde to get instant 5 mins of oxygen
        if (Input.GetKeyUp(KeyCode.BackQuote))
        {
            StarshipEnvironment.Instance.OxygenLevel += .0025f;
        }

        if (Input.GetKeyUp(KeyCode.E) && (IsUsingTerminal || TerminalManager.IsNextToTerminal))
        {
            ToggleTerminal();
        }
        else if (IsUsingTerminal && Input.GetKeyUp(KeyCode.Escape))
        {
            ToggleTerminal();
        }

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

		if (CrossPlatformInputManager.GetButtonDown("Jump")){
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
}
