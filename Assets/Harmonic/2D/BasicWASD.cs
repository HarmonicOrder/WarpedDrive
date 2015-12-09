using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;
using Prime31.TransitionKit;

public class BasicWASD : MonoBehaviour {

    public Animator animator;

	// Use this for initialization
	void Start () {
		Cursor.visible = false;
	}
	
	// Update is called once per frame
	void Update () {
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
}
