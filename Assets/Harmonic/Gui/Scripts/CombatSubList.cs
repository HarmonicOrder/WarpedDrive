using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class CombatSubList : MonoBehaviour {

    public Sprite Delete, Terminate, Corrupt, Lag, Freeze, Honeypot, Sandbox;
    public Sprite Tracer, Defender;
    public RectTransform prefab;

    void Start()
    {
        foreach(SubroutineInfo si in CyberspaceEnvironment.Instance.Subroutines)
        {
            AddSub(si.Hotkey, si.MovementName, si.FunctionName, si.CoreCost);
        }
        prefab.gameObject.SetActive(false);
    }

    private void AddSub(int hotkey, string movementName, string functionName, uint coreCost)
    {
        RectTransform newList = GameObject.Instantiate<RectTransform>(prefab);
        newList.SetParent(this.transform);
        newList.anchoredPosition3D = Vector3.zero;
        newList.localRotation = Quaternion.identity;
        newList.localScale = Vector3.one;
        newList.FindChild("Hotkey").GetComponent<Text>().text = ">" + hotkey;
        newList.FindChild("Movement").GetComponent<Image>().sprite = GetSprite(movementName);
        newList.FindChild("Function").GetComponent<Image>().sprite = GetSprite(functionName);
        newList.FindChild("Core").GetComponent<Text>().text = coreCost + "¢";
    }

    private Sprite GetSprite(string name)
    {
        switch(name)
        {
            case "Delete":
                return Delete;
            case "Terminate":
                return Terminate;
            case "Corrupt":
                return Corrupt;
            case "Lag":
                return Lag;
            case "Freeze":
                return Freeze;
            case "Honeypot":
                return Honeypot;
            case "Sandbox":
                return Sandbox;
            case "Tracer":
                return Tracer;
            case "Station":
                return Defender;
            default:
                goto case "Delete";
        }
    }
}
