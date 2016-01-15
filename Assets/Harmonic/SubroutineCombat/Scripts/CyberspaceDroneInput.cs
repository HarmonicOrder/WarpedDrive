using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.UI;
using Prime31.TransitionKit;
using System;
using System.Collections.Generic;

public class CyberspaceDroneInput : MonoBehaviour {

	public static ILockTarget CurrentLock;

	public float smoothing = 5f;
	public Transform strategyYawSphere;
	public Transform strategyPitchSphere;
	public float xSensitivity = 2f,
				 ySensitivity = 2f,
				 zSensitivity = 2f;
	public float moveSpeed = 1f;
	public bool invertY = true;
	public Transform AlphaSubPrefab;
	public Transform BetaSubPrefab;
	public SubroutineStatus AlphaStatus;
	public SubroutineStatus BetaStatus;
	public SubroutineStatus GammaStatus;
	public Text TargetGuiText;
	public Transform PivotTransform;
	public MeshRenderer HitCrosshair;
	public MeshRenderer Crosshair;
	public RectTransform Menu;
	public Text consoleText;
	public Transform TracerStartPosition;
    public Transform SubroutineHarnessPrefab;
	private Quaternion currentLookRotation;
	private bool showingMainMenu;

	private bool lerpToMachine = false, isControllingSubroutine = false;
	private float currentLerpTime = 0f;
	private float lerpToTime = .5f;
	private Vector3 lerpFrom, lerpTo;

    public Camera ControlCamera { get; private set; }

    void Awake() {
		CyberspaceBattlefield.Current = new CyberspaceBattlefield();
		StrategyConsole.Initialize(consoleText);

		AlphaSubPrefab.GetComponent<Subroutine>().Status = AlphaStatus;
		BetaSubPrefab.GetComponent<Subroutine>().Status = BetaStatus;
	}

	// Use this for initialization
	void Start () {
        Radio.Instance.SetSoundtrack(Radio.Soundtrack.MethodicalAntivirus);
        
		currentLookRotation = PivotTransform.localRotation;
		Cursor.lockState = CursorLockMode.Confined;
		Cursor.visible = false;
		HitCrosshair.enabled = false;
        CurrentLock = null;
	}

	// Update is called once per frame
	void Update () {		
		if (CrossPlatformInputManager.GetButtonDown("Cancel")){
			showingMainMenu = !showingMainMenu;

			ToggleMenu(showingMainMenu);
		}

		if (lerpToMachine)
		{
			if (currentLerpTime > lerpToTime)
			{
				this.transform.position = lerpTo;
				currentLerpTime = 0f;
				lerpToMachine = false;
			}
			else
			{
				this.transform.position = Vector3.Lerp(lerpFrom, lerpTo, currentLerpTime / lerpToTime);
				currentLerpTime += Time.deltaTime;
			}
		}

		if (showingMainMenu)
			return;

		bool LeftClick = CrossPlatformInputManager.GetButtonDown("Fire1");

		RaycastHit rayHit;
		if (Physics.Raycast(PivotTransform.position, PivotTransform.forward, out rayHit, Mathf.Infinity, 1 << LayerMask.NameToLayer("TargetRaycast")))
		{
			if (rayHit.collider != null)
			{
				//print (rayHit.collider.transform.parent.name);
				VirusAI v = (VirusAI)rayHit.collider.GetComponentInParent(typeof(VirusAI));
				if (v)
				{
					TargetGuiText.text = v.Info.GetTargetRichText();
					AssignLockTarget(LeftClick, v);
				}

				Hardpoint h = (Hardpoint)rayHit.collider.GetComponentInParent(typeof(Hardpoint));
				if (h)
				{
					AssignLockTarget(LeftClick, h);
				}

                IActivatable a = (IActivatable)rayHit.collider.GetComponentInParent(typeof(IActivatable));
				if ((a != null) && LeftClick)
				{
					a.Activate();
				}

                MachineLabel m = rayHit.collider.GetComponent<MachineLabel>();
				if ((m != null) && LeftClick)
				{
                    if (m.myMachine.IsAccessible)
                    {
					    //super hack
					    lerpTo = rayHit.collider.transform.parent.parent.position;
					    lerpFrom = this.transform.position;
					    lerpToMachine = true;
                    }
                    else
                    {
                        ToastLog.Toast("Machine\nInaccessible");
                    }
				}
			}
			
			HitCrosshair.enabled = true;
		}
		else 
		{
			if (LeftClick && CurrentLock != null)
			{
				CurrentLock.DisableLockedOnGui();
				CurrentLock = null;
			}
			HitCrosshair.enabled = false;
		}

#if UNITY_EDITOR
        if (Input.GetKey(KeyCode.LeftShift))
#else
        if (Input.GetKey(KeyCode.LeftControl))

#endif
        {
            CheckControlCamera(KeyCode.Alpha1, 1);
            CheckControlCamera(KeyCode.Alpha2, 2);
            CheckControlCamera(KeyCode.Alpha3, 3);
        }
        else if (CurrentLock != null)
        {
		    if (Input.GetKeyDown(KeyCode.Alpha1))
		    {
                PossiblyCreateSubroutine(GetSubroutineInfo(0));
		    }

		    if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                PossiblyCreateSubroutine(GetSubroutineInfo(1));
            }

            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                PossiblyCreateSubroutine(GetSubroutineInfo(2));
            }

            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                PossiblyCreateSubroutine(GetSubroutineInfo(3));
            }
        }

		float horz = CrossPlatformInputManager.GetAxis("Vertical") * ySensitivity;
		float vert = -CrossPlatformInputManager.GetAxis("Horizontal") * xSensitivity;

		if (invertY) 
		{
			vert = -vert;
		}

		float dX = 0f, dY = 0f;
		if (vert != 0f)
			dY = vert * moveSpeed;
		if (horz != 0f)
			dX = horz * moveSpeed;

		SlerpRotate(strategyPitchSphere, dX, 0, 91f);

		SlerpRotate(strategyYawSphere, 0, dY);

		float x = -CrossPlatformInputManager.GetAxis("Mouse Y") * xSensitivity;
		float y = CrossPlatformInputManager.GetAxis("Mouse X") * ySensitivity;
		
		currentLookRotation *= Quaternion.Euler(x, 
		                                   y, 
		                                   0);
        currentLookRotation = new Quaternion(currentLookRotation.x, currentLookRotation.y, 0, currentLookRotation.w);
		PivotTransform.localRotation = Quaternion.Slerp(PivotTransform.localRotation, currentLookRotation, smoothing * Time.deltaTime);
	}

    private void CheckControlCamera(KeyCode k, int hotkey)
    {
        if (Input.GetKeyUp(k))
        {
            //print("key down");
            if (isControllingSubroutine)
            {
                //print("already controlling");
                RevertToStrategyCamera();
            }
            else
            {
                //("looking for subroutines");
                Subroutine s = ActiveSubroutines.List.Find(sub => sub.SInfo.Hotkey == hotkey);
                if (s != null)
                {
                    //print("making camera");
                    isControllingSubroutine = true;
                    ControlCamera = new GameObject("ControlCamera").AddComponent<Camera>();
                    ControlCamera.transform.SetParent(s.FunctionRoot);
                    ControlCamera.cullingMask = Camera.main.cullingMask;
                    ControlCamera.depth = 1;
                    ControlCamera.clearFlags = Camera.main.clearFlags;
                    ControlCamera.transform.localPosition = Vector3.up;
                    ControlCamera.transform.localRotation = Quaternion.identity;
                    Camera.main.farClipPlane = 10;
                    //todo: auto-revert if subroutine dies
                }
                else
                {
                    //print("No subroutine to control!");
                }
            }
        }
        else
        {
            //print("no key up");
        }
    }

    private void RevertToStrategyCamera()
    {
        isControllingSubroutine = true;
        Camera.main.farClipPlane = 2000f;
        if (ControlCamera != null)
            GameObject.Destroy(ControlCamera.gameObject);
    }

    private void PossiblyCreateSubroutine(SubroutineInfo si)
    {
        if (si == null)
        {
            print("no subroutine info to create from");
            return;
        }

        if (ValidateTarget(si))
        {
            if (CyberspaceBattlefield.Current.ProvisionCores((int)si.CoreCost))
            {
                FireSubroutine(InstantiateHarness(si), si);
            }
            else
            {
                ToastLog.Toast("Insufficent\nCPU Cores");
            }
        }
        else
            print("invalid target");
    }

    private bool ValidateTarget(SubroutineInfo si)
    {
        if (si.MovementName == "Tracer")
        {
            return (CurrentLock is VirusAI);
        }

        return true;
    }

    private Transform InstantiateHarness(SubroutineInfo si)
    {
        //print("creating harness");
        if (si.MovementName == "Tracer")
        {
            return (Transform)Instantiate(SubroutineHarnessPrefab, TracerStartPosition.position, TracerStartPosition.rotation);
        }
        else
        {
            return (Transform)Instantiate(SubroutineHarnessPrefab, this.transform.position, this.transform.rotation);
        }
    }

    private SubroutineInfo GetSubroutineInfo(int index)
    {
        if (index < CyberspaceEnvironment.Instance.Subroutines.Count)
        {
            return CyberspaceEnvironment.Instance.Subroutines[index];
        }
        else
        {
            return null;
        }
    }

    private void ToggleMenu(bool showMenu)
	{
		Menu.gameObject.SetActive(showMenu);
		Crosshair.gameObject.SetActive(!showMenu);
		HitCrosshair.gameObject.SetActive(!showMenu);
		Cursor.visible = showMenu;
	}
	
	public void BackToNetwork()
	{
        CyberspaceBattlefield.Current.Abdicate = true;
		var pixelater = new PixelateTransition()
		{
			finalScaleEffect = PixelateTransition.PixelateFinalScaleEffect.ToPoint,
			duration = 1.0f
		};
		pixelater.nextScene = 2;
		TransitionKit.instance.transitionWithDelegate( pixelater );
	}
	
	public void BackToMeatspace()
    {
        CyberspaceBattlefield.Current.Abdicate = true;
        var pixelater = new PixelateTransition()
		{
			finalScaleEffect = PixelateTransition.PixelateFinalScaleEffect.ToPoint,
			duration = 1.0f
		};
		pixelater.nextScene = 1;
		TransitionKit.instance.transitionWithDelegate( pixelater );
	}

    private void FireSubroutine(Transform t, SubroutineInfo si)
	{
        //print("assigning subroutine harness");
        t.GetComponent<SubroutineHarness>().Assign(si);
        //print("activating subroutine");
		Subroutine s = t.GetComponent<Subroutine>();

        if (CurrentLock != null)
		    s.LockedTarget = CurrentLock.transform;

		s.Info.HitPoints = s.Info.MaxHitPoints;
		s.Activate(si);
	}
	
	private void AssignLockTarget(bool leftClick, ILockTarget newTargt)
	{
		if (leftClick)
		{
			if (CurrentLock != null)
			{
				CurrentLock.DisableLockedOnGui();
			}
			CurrentLock = newTargt;
			CurrentLock.EnableLockedOnGui();
		}
	}

	private void SlerpRotate(Transform target, float deltaX, float deltaY, float? xRange = null)
	{
		Quaternion newRotation = target.localRotation * Quaternion.Euler(deltaX, deltaY, 0);
		if (xRange.HasValue && (Quaternion.Angle(Quaternion.identity, newRotation) > xRange))
		{
			//try and get closer to the max
			if (target.localRotation.x < xRange)
				SlerpRotate(target, deltaX / 2, deltaY, xRange);
			return;
		}

		target.localRotation = Quaternion.Slerp(target.localRotation, newRotation, smoothing * Time.deltaTime);
	}
}
