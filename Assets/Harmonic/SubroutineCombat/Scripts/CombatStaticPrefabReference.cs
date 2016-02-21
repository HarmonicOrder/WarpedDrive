using UnityEngine;
using System.Collections;

public class CombatStaticPrefabReference : MonoBehaviour {
    public static CombatStaticPrefabReference Instance;

    public Transform Popup;

	// Use this for initialization
	void Awake () {
        Instance = this;
	}
}
