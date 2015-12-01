using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Prime31.TransitionKit;

public class SubroutineWorkstation : MonoBehaviour {

    public float rotateTime = .5f;
    public Transform subroutineVisualization;

    public string currentFunctionName = "Delete";
    public string currentMovementName = "Tracer";

    private Dictionary<string, Transform> functions = new Dictionary<string, Transform>();
    private Dictionary<string, Transform> movement = new Dictionary<string, Transform>();

    bool isRotating = false;
    float currentRotateTime = 0;
    Quaternion from, to;
    Transform subParent;
    Vector3 subFrom, subTo;

	// Use this for initialization
	void Start () {
	    foreach (Transform t in GameObject.Find("FunctionRoot").transform)
        {
            functions.Add(t.name, t);
            if (t.name != currentFunctionName)
            {
                t.gameObject.SetActive(false);
            }
        }
        foreach (Transform t in GameObject.Find("MovementRoot").transform)
        {
            movement.Add(t.name, t);
            if (t.name != currentMovementName)
            {
                t.gameObject.SetActive(false);
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (isRotating)
        {
            this.transform.localRotation = Quaternion.Slerp(from, to, currentRotateTime / rotateTime);
            subroutineVisualization.position = Vector3.Lerp(subFrom, subTo, currentRotateTime / rotateTime);
            if (currentRotateTime >= rotateTime)
            {
                this.transform.localRotation = to;
                subroutineVisualization.position = subTo;
                subroutineVisualization.SetParent(subParent);
                isRotating = false;
            }
            currentRotateTime += Time.deltaTime;
        }

        if (Input.GetKeyUp(KeyCode.Escape))
        {
            var pixelater = new SquaresTransition()
            {
                nextScene = 2,

                //finalScaleEffect = PixelateTransition.PixelateFinalScaleEffect.ToPoint,
                duration = .33f
            };
            TransitionKit.instance.transitionWithDelegate(pixelater);
        }
	}

    public void listingToSubroutine()
    {
        currentRotateTime = 0f;
        from = this.transform.localRotation;
        to = Quaternion.Euler(Vector3.up * 90f);
        subFrom = GameObject.Find("listHarness").transform.FindChild("harness").position;
        subParent = GameObject.Find("assemblyHarness").transform.FindChild("harness");
        subTo = subParent.position;
        subroutineVisualization.SetParent(null);
        isRotating = true;
    }

    public void subroutineToListing()
    {
        currentRotateTime = 0f;
        from = this.transform.localRotation;
        to = Quaternion.Euler(Vector3.up);
        subFrom = GameObject.Find("assemblyHarness").transform.FindChild("harness").position;
        subParent = GameObject.Find("listHarness").transform.FindChild("harness");
        subTo = subParent.position;
        subroutineVisualization.SetParent(null);
        isRotating = true;
    }

    public void subroutineToUpgrade()
    {
        currentRotateTime = 0f;
        from = this.transform.localRotation;
        to = Quaternion.Euler(Vector3.up * 180f);
        subFrom = GameObject.Find("assemblyHarness").transform.FindChild("harness").position;
        subParent = GameObject.Find("assemblyHarness").transform.FindChild("harness");
        subTo = subParent.position;
        subroutineVisualization.SetParent(null);
        isRotating = true;
    }
    public void upgradeToSubroutine()
    {
        currentRotateTime = 0f;
        from = this.transform.localRotation;
        to = Quaternion.Euler(Vector3.up * 90f);
        subFrom = GameObject.Find("assemblyHarness").transform.FindChild("harness").position;
        subParent = GameObject.Find("assemblyHarness").transform.FindChild("harness");
        subTo = subParent.position;
        subroutineVisualization.SetParent(null);
        isRotating = true;
    }

    public void SetFunction(string name)
    {
        if (currentFunctionName != name)
        {
            functions[currentFunctionName].gameObject.SetActive(false);
            functions[name].gameObject.SetActive(true);

            currentFunctionName = name;
        }
    }

    public void SetMovement(string name)
    {
        if (currentMovementName != name)
        {
            movement[currentMovementName].gameObject.SetActive(false);
            movement[name].gameObject.SetActive(true);

            currentMovementName = name;
        }
    }


}
