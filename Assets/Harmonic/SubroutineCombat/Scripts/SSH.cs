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
        Machine myMachine = CyberspaceBattlefield.Current.FindByName(wipeCube.root.name);
        myMachine.IsAccessible = true;
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
        float currentTime = 0f;
        while (currentTime < 1f)
        {
            this.wipeCube.localPosition = Vector3.Lerp(Vector3.zero, new Vector3(0, 400f, 0f), currentTime);
            yield return null;
            currentTime += Time.deltaTime;
        }

        this.wipeCube.gameObject.SetActive(false);
    }
}
