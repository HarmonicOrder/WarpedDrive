using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Webserver : NetworkLocationButton, IActivatable {

	public Transform DOSPrefab;
	public List<Transform> Hangars;

    private Machine myMachine { get; set; }

    // Use this for initialization
    void Start ()
    {
        this.myMachine = CyberspaceBattlefield.Current.FindByName(this.transform.root.name);
        this.myMachine.OnMachineClean += OnMachineClean;
        this.transform.localScale = Vector3.zero;
    }

    private void OnMachineClean()
    {
        StartCoroutine(this.Open());
    }

    // Update is called once per frame
    void Update () {
	
	}

	public void Activate()
	{
		StartCoroutine(FireSalvo());
        StartCoroutine(Close());
    }

	public IEnumerator FireSalvo()
	{
		if (ActiveSubroutines.MalwareList.Count > 0f)
		{
			yield return new WaitForSecondsInterruptTime(UnityEngine.Random.Range(5f, 6f));

			foreach(Transform t in Hangars)
			{
				Transform i = (Transform)Instantiate(DOSPrefab, t.position, t.rotation);
				i.Rotate(Vector3.right, -90f, Space.Self);
				i.SetParent(t);

				yield return new WaitForSecondsInterruptTime(UnityEngine.Random.Range(.1f, .25f));
			}

			StartCoroutine(FireSalvo());
		}
	}

    void OnDestroy()
    {
        this.myMachine.OnMachineClean -= OnMachineClean;
    }
}
