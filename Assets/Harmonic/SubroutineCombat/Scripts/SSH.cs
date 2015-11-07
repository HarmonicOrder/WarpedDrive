using UnityEngine;
using System.Collections;

public class SSH : KeyListener, IActivatable {

    public Transform TunnelViz;
    public Transform LockViz;

	// Use this for initialization
	void Start () {
        TunnelViz.gameObject.SetActive(false);
        this.transform.localScale = Vector3.zero;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public override void OnKeyCopied(Keystore.KeyColor keyColor)
    {
        LockViz.gameObject.SetActive(false);
        StartCoroutine(Open());
    }

    public void Activate()
    {
        TunnelViz.gameObject.SetActive(true);
        StartCoroutine(Close());
    }

    public IEnumerator Open()
    {
        while (this.transform.localScale.x < 1f)
        {
            this.transform.localScale = new Vector3(this.transform.localScale.x + Time.deltaTime * 4, 1f, 1f);
            yield return null;
        }

        this.transform.localScale = Vector3.one;
    }

    public IEnumerator Close()
    {
        while (this.transform.localScale.x > 0f)
        {
            this.transform.localScale = new Vector3(this.transform.localScale.x - Time.deltaTime * 4, 1f, 1f);
            yield return null;
        }

        this.transform.localScale = Vector3.zero;
    }
}
