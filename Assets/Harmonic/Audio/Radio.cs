using UnityEngine;
using System.Collections;

public class Radio : MonoBehaviour {

    public static Radio Instance { get; set; }

	// Use this for initialization
	void Awake () {
        Radio.Instance = this;
        GameObject.DontDestroyOnLoad(this.gameObject);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
