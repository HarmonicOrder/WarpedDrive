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
    public bool IsConsumingSlowly = false;

	// Use this for initialization
	void Start () {
        GameObject.DontDestroyOnLoad(this.gameObject);
        StartCoroutine(Consume());
	}

    private IEnumerator Consume()
    {
        while(enabled)
        {
            while(IsConsuming)
            {
                if (IsConsumingSlowly)
                {
                    StarshipEnvironment.Instance.OxygenLevel -= (StarshipEnvironment.OxygenConsumedPerSecond / 20 + StarshipEnvironment.Instance.OxygenProductionPerSecond / 20);
                }
                else
                {
                    StarshipEnvironment.Instance.OxygenLevel -= (StarshipEnvironment.OxygenConsumedPerSecond / 2 + StarshipEnvironment.Instance.OxygenProductionPerSecond / 2 );
                }
                yield return new WaitForSeconds(.5f);
            }
            //do NOT remove this! coroutines MUST yield return or they will hold up the thread!
            yield return new WaitForSeconds(.5f);
        }
    }

    // Update is called once per frame
    void Update () {
	
	}
}
