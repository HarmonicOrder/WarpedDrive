using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;
using Prime31.TransitionKit;
using System;

public class BasicWASD : MonoBehaviour {

    public Animator animator;
    public Camera TerminalCamera;
    public UnityStandardAssets.ImageEffects.BlurOptimized BlurEffect;

    private bool IsUsingTerminal = false;

    // Use this for initialization
    void Start () {
		Cursor.visible = false;
        Radio.Instance.SetSoundtrack(Radio.Soundtrack.Spacey);
        BlurEffect.enabled = false;
        TerminalCamera.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyUp(KeyCode.E))
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

        Vector3 vec = new Vector3(h, 0f, v);
		vec.Normalize();
		vec.x *= Time.deltaTime * 5f;
		vec.z *= Time.deltaTime * 5f;
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
	}

    private void ToggleTerminal()
    {
        IsUsingTerminal = !IsUsingTerminal;

        BlurEffect.enabled = IsUsingTerminal;
        TerminalCamera.enabled = IsUsingTerminal;
    }
}
