using UnityEngine;
using System.Collections;
using System;

public class OxygenConsumer : MonoBehaviour {

    public static OxygenConsumer _instance;
    public static OxygenConsumer Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameObject("OxygenConsumer").AddComponent<OxygenConsumer>();
            }

            return _instance;
        }
    }

    public bool IsConsuming = true;

	// Use this for initialization
	void Start () {
        GameObject.DontDestroyOnLoad(this.gameObject);
        StartCoroutine(Consume());
	}

    private IEnumerator Consume()
    {
        while(enabled && IsConsuming)
        {
            StarshipEnvironment.Instance.OxygenLevel -= (StarshipEnvironment.OxygenConsumedPerSecond + StarshipEnvironment.Instance.OxygenProductionPerSecond);
            yield return new WaitForSeconds(1f);
        }
    }

    // Update is called once per frame
    void Update () {
	
	}
}
