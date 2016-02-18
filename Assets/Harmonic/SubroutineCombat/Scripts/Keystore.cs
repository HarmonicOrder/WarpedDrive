using UnityEngine;
using System.Collections;

public class Keystore : NetworkLocationButton, IActivatable {

    public KeyColor ThisKeyColor = KeyColor.Green;
    private Machine myMachine { get; set; }

    public delegate void KeyCopiedEvent(KeyColor keyColor);
    public static KeyCopiedEvent OnKeyCopied;

	// Use this for initialization
	void Start () {
        this.transform.localScale = Vector3.zero;
        this.myMachine = CyberspaceBattlefield.Current.FindByName(this.transform.root.name);
        this.myMachine.OnMachineClean += OnSystemClean;
	}

	private void OnSystemClean() {
        StartCoroutine(Open());
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Activate()
    {
        StartCoroutine(Close());
        ToastLog.Toast("Key Copied!");
    }

    public override void AfterClose()
    {
        BroadcastKeyCopied();
    }

    private void BroadcastKeyCopied()
    {
        if (OnKeyCopied != null)
            OnKeyCopied(this.ThisKeyColor);
    }

    public enum KeyColor { Green, Blue, Purple }

    void OnDestroy()
    {
        this.myMachine.OnMachineClean -= OnSystemClean;
    }
}
