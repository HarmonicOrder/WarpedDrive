using UnityEngine;
using System.Collections;

public class NetworkLocationButton : MonoBehaviour {

    public bool IsRequired;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {

    }

    public IEnumerator Open()
    {
        if (IsRequired)
            CyberspaceBattlefield.Current.RequiredButtonExists = true;

        while (this.transform.localScale.x < 1f)
        {
            this.transform.localScale = new Vector3(this.transform.localScale.x + InterruptTime.deltaTime * 4, 1f, 1f);
            yield return null;
        }

        this.transform.localScale = Vector3.one;
        AfterOpen();
    }

    public virtual void AfterOpen() { }

    public IEnumerator Close()
    {
        if (IsRequired)
            CyberspaceBattlefield.Current.RequiredButtonExists = false;

        while (this.transform.localScale.x > 0f)
        {
            this.transform.localScale = new Vector3(this.transform.localScale.x - InterruptTime.deltaTime * 4, 1f, 1f);
            yield return null;
        }

        this.transform.localScale = Vector3.zero;
        AfterClose();
    }

    public virtual void AfterClose() { }

    void OnDestroy()
    {
        if (IsRequired)
            CyberspaceBattlefield.Current.RequiredButtonExists = false;

    }
}
