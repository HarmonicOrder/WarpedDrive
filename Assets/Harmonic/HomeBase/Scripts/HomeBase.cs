﻿using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;
using Prime31.TransitionKit;
using System;
using UnityEngine.UI;
using System.Collections.Generic;

public class HomeBase : MonoBehaviour {
	public Camera zoomCamera, mainCamera;
	public Texture2D cursorTexture;
	public Transform homeFiberGate;
	public Transform WarpBubblePrefab;
	public Color _highlightColor;
	public static Color SubnetTextHighlightColor;
	public Camera RenderExitCamera;
    public Canvas UICanvas;
    public Text zoomHint, ZoomCurrentSubnet;
    public Button ZoomTop, ZoomLeft, ZoomRight, ZoomBottom;
    public Transform EngineeringZoomPoint, InfosecZoomPoint, ArchivesZoomPoint, MedicalZoomPoint, PayloadZoomPoint, BridgeZoomPoint;

    private CameraZoomToZoom zoomScript;
    private SubnetState CurrentSubnetState;
	// Use this for initialization
	void Start () {
        SeedLayout();
        Radio.Instance.SetSoundtrack(Radio.Soundtrack.SubtleElectronica);
		SubnetTextHighlightColor = this._highlightColor;
		Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);
		zoomScript = this.transform.GetComponent<CameraZoomToZoom>();
		Cursor.visible = true;
        OxygenConsumer.Instance.IsConsumingSlowly = true;
        StartCoroutine(ShowCanvas());
        zoomHint.enabled = false;

        CurrentSubnetState = SubnetState.Infosec;
        RefreshButtonLayout();
        RefreshZoomButtonVisibility();
    }

    private IEnumerator ShowCanvas()
    {
        yield return new WaitForSeconds(1f);
        UICanvas.enabled = true;
    }

    private bool isZoomedOut = false, isSelectingServer = false, isWarping = false;
	private SubnetSceneSelector hoverSubnet;
	private ServerSelector currentServer;
	// Update is called once per frame
	void Update () {
		if (CrossPlatformInputManager.GetButtonDown("Zoom")){
            ToggleZoom();
		}
		if (Input.GetKeyUp(KeyCode.Escape)){
			TransitionToMeatspace();
		}
        if (Input.GetKeyUp(KeyCode.Q))
        {
            TransitionToSubroutineWorkspace();
        }

		if (isZoomedOut){
			if (CrossPlatformInputManager.GetButtonDown("Fire1")){
				if (currentServer != null)
					OnServerSelect();
				//else if (hoverSubnet != null)
				//	OnSubnetSelect();
			} else if (!isWarping){
				RaycastHit hit;
				Ray pointRay = zoomCamera.ScreenPointToRay(Input.mousePosition);

				if (Physics.Raycast(pointRay, out hit, Mathf.Infinity)){

					if (hit.collider != null){
						//print ("got a collider: "+hit.collider.gameObject.name);
						//SubnetSceneSelector sss = hit.collider.GetComponent<SubnetSceneSelector>();

						//if (sss != null){
						//	hoverSubnet = sss;
						//	sss.OnHoverOn();
						//}

						ServerSelector s = hit.collider.GetComponent<ServerSelector>();
						
						if (s != null){
							if (currentServer != null)
							{
								currentServer.OnHoverOff();
							}
							currentServer = s;
							s.OnHoverOver();
						}
					}
				} else {
					if (!isSelectingServer && (hoverSubnet != null)){
						hoverSubnet.OnHoverOff();
						hoverSubnet = null;
					}
					if (currentServer != null){
						currentServer.OnHoverOff();
						currentServer = null;
					}
				}
			}
		}
	}

    private void ToggleZoom()
    {
        isZoomedOut = !isZoomedOut;

        //if (selectedSubnet != null)
        //{
        //    selectedSubnet.OnDeselect();
        //    selectedSubnet = null;
        //}


        zoomHint.enabled = isZoomedOut;

        if (!isZoomedOut)
        {
            CurrentSubnetState = SubnetState.Infosec;
            RefreshButtonLayout();
        }

        RefreshZoomButtonVisibility();
        

        zoomScript.ZoomTo(isZoomedOut);
    }

    private void ZoomToCurrentSubnetPoint()
    {

        //this.transform.GetComponent<CameraZoomToZoom>().ZoomTo(isZoomedOut);
        zoomScript.ZoomTo(SubnetLayoutInfo[CurrentSubnetState].ZoomToPoint, 1f);
    }

    private void RefreshZoomButtonVisibility()
    {
        ZoomTop.gameObject.SetActive(isZoomedOut);
        ZoomRight.gameObject.SetActive(isZoomedOut);
        ZoomBottom.gameObject.SetActive(isZoomedOut);
        ZoomLeft.gameObject.SetActive(isZoomedOut);
        ZoomCurrentSubnet.gameObject.SetActive(isZoomedOut);
    }

    private void OnSubnetSelect()
	{
        if (selectedSubnet != null)
            selectedSubnet.OnDeselect();

        selectedSubnet = hoverSubnet;
		isSelectingServer = true;
		Transform zoomTo = selectedSubnet.transform.FindChild("ZoomPosition");
        selectedSubnet.OnSelect();
		//zoomScript.afterZoom = new CameraZoomToZoom.AfterZoomFinished(AfterSubnetZoom);
		//TextMesh[] meshes = selectedSubnet.GetComponentsInChildren<TextMesh>();
		//for (int i = 0; i < meshes.Length; i++) {
		//	meshes[i].transform.rotation = Quaternion.Euler(45f, 0f, 0f);
		//}
		//zoomScript.ZoomTo(zoomTo, 1f);
	}

	private void OnServerSelect()
	{
		if(FindNetworkLocation(currentServer.name))
		{
			isSelectingServer = false;
			isWarping = true;
			homeFiberGate.LookAt(HarmonicUtils.FindInChildren(currentServer.transform, "gateway").position);
			zoomScript.afterZoom = new CameraZoomToZoom.AfterZoomFinished(AfterSubnetZoom);
			zoomScript.ZoomTo(homeFiberGate.FindChild("gatewayStartNode"), 2f);
		}
	}

	private void AfterSubnetZoom(){
		StartCoroutine(BeforeWarpStart());
	}

	private IEnumerator BeforeWarpStart(){
		yield return new WaitForSeconds(.25f);
		OnWarpStart();
	}

	private Transform warpBubble;
    private SubnetSceneSelector selectedSubnet;

    public void OnWarpStart(){
		warpBubble = (Transform)Instantiate(WarpBubblePrefab, zoomCamera.transform.position, zoomCamera.transform.rotation);
		warpBubble.parent = zoomCamera.transform;
		warpBubble.localPosition = Vector3.forward*6;
		StartCoroutine(waitThenWarp());
	}
	
	private IEnumerator waitThenWarp()
	{
		yield return new WaitForSeconds(.75f);
		warpBubble.localPosition = Vector3.zero;
		zoomCamera.farClipPlane = 9f;
		zoomScript.afterZoom = new CameraZoomToZoom.AfterZoomFinished(OnWarpEnd);
		Transform goToGate = currentServer.transform.FindChild("gateway");
		zoomScript.ZoomTo(goToGate, 2f, 80f);

		if (RenderExitCamera != null){
			RenderExitCamera.transform.position = goToGate.position + (Vector3.forward * 10);
			RenderExitCamera.transform.rotation = goToGate.rotation;
		}
	}

	public void OnWarpEnd(){
		//GameObject.Destroy(warpBubble.gameObject);
		//isWarping = false;
		//zoomScript.MainCameraToZoom();
		warpBubble.Find("endGateScaler").GetComponent<Scaler>().scaleUp = true;
		StartCoroutine(waitThenBattle());
	}

	private IEnumerator waitThenBattle()
	{
		yield return new WaitForSeconds(1f);
		TransitionToBattlespace();
	}

	private void TransitionToBattlespace()
	{
		var pixelater = new SquaresTransition()
		{
			nextScene = NetworkMap.CurrentLocation.sceneIndex,

			//finalScaleEffect = PixelateTransition.PixelateFinalScaleEffect.ToPoint,
			duration = .33f
		};
		TransitionKit.instance.transitionWithDelegate( pixelater );
    }

    private void TransitionToSubroutineWorkspace()
    {
        var pixelater = new SquaresTransition()
        {
            nextScene = 3,

            //finalScaleEffect = PixelateTransition.PixelateFinalScaleEffect.ToPoint,
            duration = .33f
        };
        TransitionKit.instance.transitionWithDelegate(pixelater);
    }

    private void TransitionToMeatspace()
    {
        var pixelater = new PixelateTransition()
        {
            nextScene = 1,
            finalScaleEffect = PixelateTransition.PixelateFinalScaleEffect.ToPoint,
            duration = 1.0f
        };
        TransitionKit.instance.transitionWithDelegate(pixelater);
    }

    private bool FindNetworkLocation(string name)
	{
		NetworkMap.CurrentLocation = NetworkMap.GetLocationByLocationName(name);
		return (NetworkMap.CurrentLocation != null);
	}

    public void OnSubnetButtonClick(string name)
    {
        switch (name)
        {
            case "t":
                CurrentSubnetState = SubnetLayoutInfo[CurrentSubnetState].Top;
                break;
            case "r":
                CurrentSubnetState = SubnetLayoutInfo[CurrentSubnetState].Right;
                break;
            case "b":
                CurrentSubnetState = SubnetLayoutInfo[CurrentSubnetState].Bottom;
                break;
            case "l":
                CurrentSubnetState = SubnetLayoutInfo[CurrentSubnetState].Left;
                break;
        }
        RefreshButtonLayout();
        ZoomToCurrentSubnetPoint();
    }

    public void RefreshButtonLayout()
    {
        CheckSubnetButtonVisible(ZoomTop, SubnetLayoutInfo[CurrentSubnetState].Top);
        CheckSubnetButtonVisible(ZoomRight, SubnetLayoutInfo[CurrentSubnetState].Right);
        CheckSubnetButtonVisible(ZoomBottom, SubnetLayoutInfo[CurrentSubnetState].Bottom);
        CheckSubnetButtonVisible(ZoomLeft, SubnetLayoutInfo[CurrentSubnetState].Left);
        ZoomCurrentSubnet.GetComponent<Text>().text = CurrentSubnetState.ToString();
    }

    private void CheckSubnetButtonVisible(Button b, SubnetState s)
    {
        b.gameObject.SetActive(s != SubnetState.None);
        if (s != SubnetState.None)
        {
            b.GetComponentInChildren<Text>().text = s.ToString();
        }
    }

    //TODO: Add bridge
    public struct SubnetButtonLayout
    {
        public SubnetState Top;
        public SubnetState Right;
        public SubnetState Bottom;
        public SubnetState Left;
        public Transform ZoomToPoint;
    }

    public enum SubnetState
    {
        None,
        Infosec,
        Engineering,
        Archives,
        Payload,
        Medical,
        Bridge
    }

    public Dictionary<SubnetState, SubnetButtonLayout> SubnetLayoutInfo;
    private void SeedLayout()
    {
        SubnetLayoutInfo = new Dictionary<SubnetState, SubnetButtonLayout>()
        {
            {SubnetState.Infosec, new SubnetButtonLayout()
                {
                    ZoomToPoint = InfosecZoomPoint,
                    Left = SubnetState.Engineering,
                    Right = SubnetState.Archives,
                    Bottom = SubnetState.Payload,
                    Top = SubnetState.Medical
                }
            },
            {SubnetState.Engineering, new SubnetButtonLayout()
                {
                    ZoomToPoint = EngineeringZoomPoint,
                    Right = SubnetState.Infosec
                }
            },
            {SubnetState.Archives, new SubnetButtonLayout()
                {
                    ZoomToPoint = ArchivesZoomPoint,
                    Left = SubnetState.Infosec
                }
            },
            {SubnetState.Payload, new SubnetButtonLayout()
                {
                    ZoomToPoint = PayloadZoomPoint,
                    Top = SubnetState.Infosec
                }
            },
            {SubnetState.Medical, new SubnetButtonLayout()
                {
                    ZoomToPoint = MedicalZoomPoint,
                    Bottom = SubnetState.Infosec
                }
            }
            //TODO: add BRIDGE
        };
    }
}
