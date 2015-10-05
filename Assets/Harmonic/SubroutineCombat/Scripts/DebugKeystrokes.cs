using UnityEngine;
using System.Collections;

public class DebugKeystrokes : MonoBehaviour {

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.BackQuote))
		{
			foreach(IMalware mal in ActiveSubroutines.MalwareList.ToArray())
			{
				mal.TakeDamage(999f);
			}
		}
	}
}
