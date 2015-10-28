using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Webserver : MonoBehaviour {

	public Transform DOSPrefab;
	public List<Transform> Hangars;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Activate()
	{
		StartCoroutine(FireSalvo());
	}

	public IEnumerator FireSalvo()
	{
		if (ActiveSubroutines.MalwareList.Count > 0f)
		{
			yield return new WaitForSeconds(Random.Range(5f, 6f));

			foreach(Transform t in Hangars)
			{
				Transform i = (Transform)Instantiate(DOSPrefab, t.position, t.rotation);
				i.Rotate(Vector3.right, -90f, Space.Self);
				i.SetParent(t);

				yield return new WaitForSeconds(Random.Range(.1f, .25f));
			}

			StartCoroutine(FireSalvo());
		}
	}
}
