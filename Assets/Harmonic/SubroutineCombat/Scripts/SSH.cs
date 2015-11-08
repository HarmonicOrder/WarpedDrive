using UnityEngine;
using System.Collections;

public class SSH : NetworkLocationButton, IKeyListener, IActivatable {

    public Transform TunnelViz;
    public Transform LockViz;
    public Transform wipeCube;

	// Use this for initialization
	void Start () {
        TunnelViz.gameObject.SetActive(false);
        this.transform.localScale = Vector3.zero;
        Keystore.OnKeyCopied += OnKeyCopied;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnKeyCopied(Keystore.KeyColor keyColor)
    {
        LockViz.gameObject.SetActive(false);
        StartCoroutine(Open());
    }

    public void Activate()
    {
        TunnelViz.gameObject.SetActive(true);
        ToastLog.Toast("SSH Tunnel\nOpened");
        if (wipeCube != null)
            StartCoroutine(CloseCube());
        StartCoroutine(Close());
    }

    void Destroy()
    {
        Keystore.OnKeyCopied -= OnKeyCopied;
    }

    public IEnumerator CloseCube()
    {
        //while (wipeCube.localPosition.y < 1000f)
        //{
        //    this.wipeCube.localPosition = new Vector3(0, this.transform.localScale.y + (Time.deltaTime * 11000f), 0f);
            yield return null;
        //}

        this.wipeCube.gameObject.SetActive(false);
    }
}
