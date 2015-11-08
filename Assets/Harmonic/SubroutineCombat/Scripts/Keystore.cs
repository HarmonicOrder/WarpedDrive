using UnityEngine;
using System.Collections;

public class Keystore : MonoBehaviour, IActivatable {

    public KeyColor ThisKeyColor = KeyColor.Green;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Activate()
    {
        StartCoroutine(Close());
        ToastLog.Toast("Key Copied!");
    }

    public IEnumerator Close()
    {
        while (this.transform.localScale.x > 0f)
        {
            this.transform.localScale = new Vector3(this.transform.localScale.x - Time.deltaTime * 4, 1f, 1f);
            yield return null;
        }

        this.transform.localScale = Vector3.zero;

        BroadcastKeyCopied();
    }

    private void BroadcastKeyCopied()
    {
        KeyListener[] listeners = FindObjectsOfType(typeof(KeyListener)) as KeyListener[];

        foreach(KeyListener listen in listeners)
        {
            listen.OnKeyCopied(this.ThisKeyColor);
        }
    }

    public enum KeyColor { Green, Blue, Purple }
}
