using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Prime31.TransitionKit;
using System;
using UnityEngine.UI;

public class SubroutineWorkstation : MonoBehaviour {

    public float rotateTime = .5f, moveTime = .5f;
    public Transform subroutineVisualization;

    public string currentFunctionName = "";
    public string currentMovementName = "Tracer";

    public GameObject UpgradeRoot, UpgradeLines;

    private Dictionary<string, Transform> functions = new Dictionary<string, Transform>();
    private Dictionary<string, Transform> movement = new Dictionary<string, Transform>();

    bool isRotating = false, isMoving = false;
    float currentRotateTime = 0, currentMoveTime = 0;
    Quaternion from, to;
    Vector3 camFrom, camTo;
    Transform subParent;
    Vector3 subFrom, subTo;
    private GameObject FunctionBaseButton;
    private GameObject FunctionLeftButton;
    private GameObject FunctionRightButton;

    // Use this for initialization
    void Start () {
        Radio.Instance.SetSoundtrack(Radio.Soundtrack.DigitalEnvironment);

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
        FunctionBaseButton = GameObject.Find("FunctionBase");
        FunctionLeftButton = GameObject.Find("FunctionLeft");
        FunctionRightButton = GameObject.Find("FunctionRight");
        UpgradeRoot.SetActive(false);
        UpgradeLines.SetActive(false);
        SetFunction("Delete");
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
        if (isMoving)
        {
            this.transform.position = Vector3.Lerp(camFrom, camTo, currentMoveTime/ moveTime);
            subroutineVisualization.position = Vector3.Lerp(subFrom, subTo, currentMoveTime / moveTime);
            if (currentMoveTime >= moveTime)
            {
                this.transform.position = camTo;
                subroutineVisualization.position = subTo;
                subroutineVisualization.SetParent(subParent);
                isMoving = false;
            }
            currentMoveTime += Time.deltaTime;
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
        currentMoveTime = 0f;
        camFrom = this.transform.position;
        camTo = GameObject.Find("upgradeHarness").transform.FindChild("cameraMount").position;
        subFrom = GameObject.Find("assemblyHarness").transform.FindChild("harness").position;
        subParent = GameObject.Find("upgradeHarness").transform.FindChild("harness");
        subTo = subParent.position;
        subroutineVisualization.SetParent(null);
        isMoving = true;
        UpgradeRoot.SetActive(true);
        UpgradeLines.SetActive(true);
    }
    public void upgradeToSubroutine()
    {
        currentMoveTime = 0f;
        camFrom = this.transform.position;
        camTo = Vector3.zero;
        subFrom = GameObject.Find("upgradeHarness").transform.FindChild("harness").position;
        subParent = GameObject.Find("assemblyHarness").transform.FindChild("harness");
        subTo = subParent.position;
        subroutineVisualization.SetParent(null);
        isMoving = true;
        UpgradeRoot.SetActive(false);
        UpgradeLines.SetActive(false);
    }

    public void SetFunction(string name)
    {
        if (currentFunctionName != name)
        {
            functions[currentFunctionName].gameObject.SetActive(false);
            functions[name].gameObject.SetActive(true);

            currentFunctionName = name;
        }

        UpdateUpgrades();
    }

    private void UpdateUpgrades()
    {
        Upgrade.WorkstationMapping mapping = Upgrade.FunctionUpgrades[Type.GetType(currentFunctionName)];
        SetUpgradeButton(FunctionBaseButton, mapping.Base);
        SetUpgradeButton(FunctionLeftButton, mapping.Left);
        SetUpgradeButton(FunctionRightButton, mapping.Right);
    }

    private void SetUpgradeButton(GameObject gameObject, Upgrade u)
    {
        gameObject.transform.FindChild("Name").GetComponent<Text>().text = u.Name;
        gameObject.transform.FindChild("Description").GetComponent<Text>().text = u.Description;
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
