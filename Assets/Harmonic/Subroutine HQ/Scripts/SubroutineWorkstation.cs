using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Prime31.TransitionKit;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SubroutineWorkstation : MonoBehaviour {

    public float rotateTime = .5f, moveTime = .5f;
    public Transform subroutineVisualization;
    public RectTransform subroutineListPanel, addNewSubroutineBtn;
    public Text SummaryText, CoreText, RAMText, HPText, DMGText, FreeRAMText;

    public string currentFunctionName = "";
    public string currentMovementName = "";

    public GameObject UpgradeRoot, UpgradeLines;
    public Color EquippedButtonColor;

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
    private Button CurrentSubroutineButton;
    private Button CurrentFunctionButton;
    private Button CurrentMovementButton;

    private Dictionary<RectTransform, SubroutineInfo> ButtonInfoCache = new Dictionary<RectTransform, SubroutineInfo>();

    public SubroutineInfo CurrentlyModifyingSubroutine { get; private set; }

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
        InitializeSubroutineList();
        UpdateFreeRAM();
    }

    private void InitializeSubroutineList()
    {
        int i = 0;
        float height = 0, offsetY = 0;
        foreach (RectTransform r in subroutineListPanel)
        {
            SubroutineInfo si = CyberspaceEnvironment.Instance.Subroutines.Find((s) => (s.ID == r.name));
            if (r.name == "new")
            {
            }
            else if (si == null)
            { 
                r.gameObject.SetActive(false);
            }
            else
            {
                height = r.sizeDelta.y;
                i++;
                r.GetComponentInChildren<Text>().text = si.CompositeName;
                ButtonInfoCache.Add(r, si);

                if (si.ID == "a1")
                {
                    offsetY = r.anchoredPosition.y;

                    SetSubroutine(si, r.GetComponent<Button>());
                }
            }
        }

        addNewSubroutineBtn.anchoredPosition = new Vector2(addNewSubroutineBtn.anchoredPosition.x, offsetY - (i * height) - (height / 2));
    }

    private void SetSubroutine(SubroutineInfo si, Button button)
    {
        CurrentlyModifyingSubroutine = si;

        SetMovementVisualization(si.MovementName);
        SetFunctionVisualization(si.FunctionName);

        ShowSubroutineSummary(si);

        CurrentSubroutineButton = SetEquipButtonColor(CurrentSubroutineButton, button);
    }

    private void ShowSubroutineSummary(SubroutineInfo si)
    {
        SummaryText.text = string.Format("{0}\n", si.CompositeName);
        CoreText.text = string.Format(" {0}  ¢", si.CoreCost);
        RAMText.text = string.Format(" {0} RAM", si.RAMCost);
        //HPText.text = string.Format("{0} HP", si.);
        //DMGText.text = string.Format("{0} DMG", si.d);
    }

    private void UpdateFreeRAM()
    {
        FreeRAMText.text = string.Format("{0} TB Free RAM", CyberspaceEnvironment.Instance.MaximumRAM - CyberspaceEnvironment.Instance.CurrentRAMUsed);
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
        CurrentlyModifyingSubroutine.FunctionName = name;
        SetFunctionVisualization(name);
        UpdateUpgrades();
    }

    private string oldFunctionName = "";
    private void SetFunctionVisualization(string name)
    {
        if (name != oldFunctionName)
        {
            if (functions.ContainsKey(oldFunctionName))
                functions[oldFunctionName].gameObject.SetActive(false);

            functions[name].gameObject.SetActive(true);

            oldFunctionName = name;

            CurrentFunctionButton = SetEquipButtonColor(CurrentFunctionButton, GameObject.Find(name + "Button").GetComponent<Button>());
        }
    }

    private Button SetEquipButtonColor(Button oldButton, Button newButton)
    {
        if (oldButton != null)
            oldButton.colors = ChangeColors(oldButton.colors, newButton.colors.normalColor);

        newButton.colors = ChangeColors(newButton.colors, EquippedButtonColor);

        return newButton;
    }

    private ColorBlock ChangeColors(ColorBlock colors, Color newNormalColor)
    {
        return new ColorBlock()
        {
            colorMultiplier = colors.colorMultiplier,
            disabledColor = colors.disabledColor,
            fadeDuration = colors.fadeDuration,
            highlightedColor = colors.highlightedColor,
            pressedColor = colors.pressedColor,
            normalColor = newNormalColor
        };
    }

    private void UpdateUpgrades()
    {
        Type functionType = Type.GetType(currentFunctionName);
        if (Upgrade.FunctionUpgrades.ContainsKey(functionType))
        {
            Upgrade.WorkstationMapping mapping = Upgrade.FunctionUpgrades[functionType];
            SetUpgradeButton(FunctionBaseButton, mapping.Base);
            SetUpgradeButton(FunctionLeftButton, mapping.Left);
            SetUpgradeButton(FunctionRightButton, mapping.Right);
        }
    }

    //TODO: set color on button based on "equipped"
    private void SetUpgradeButton(GameObject gameObject, Upgrade u)
    {
        //TODO : if you haven't discovered it, set name and description to ???
        gameObject.transform.FindChild("Name").GetComponent<Text>().text = u.Name;
        gameObject.transform.FindChild("Description").GetComponent<Text>().text = u.Description;
    }

    public void SetMovement(string name)
    {
        SetMovement(name, false);
    }

    public void SetMovement(string name, bool skipRAMCheck = false)
    {
        if (CurrentlyModifyingSubroutine.ValidRAMUse(name) || skipRAMCheck)
        {
            CurrentlyModifyingSubroutine.MovementName = name;
            SetMovementVisualization(name);
            UpdateUpgrades();
            UpdateFreeRAM();
        }
        else
        {
            print("not enough ram");
        }
    }

    private string oldMovementName = "";
    private void SetMovementVisualization(string name)
    {
        if (oldMovementName != name)
        {
            if (movement.ContainsKey(oldMovementName))
                movement[oldMovementName].gameObject.SetActive(false);

            movement[name].gameObject.SetActive(true);
            oldMovementName = name;
            
            CurrentMovementButton = SetEquipButtonColor(CurrentMovementButton, GameObject.Find(name + "Button").GetComponent<Button>());
        }
    }

    //actually pointer click now
    public void PointerEnter(BaseEventData data)
    {
        if (data is PointerEventData)
        {
            RectTransform rt = (data as PointerEventData).pointerEnter.transform.parent.GetComponent<RectTransform>();
            if (rt != null)
            {
                if (ButtonInfoCache.ContainsKey(rt))
                {
                    SubroutineInfo si = ButtonInfoCache[rt];

                    SetSubroutine(si, rt.GetComponent<Button>());
                }
            }
        }
    }
}
