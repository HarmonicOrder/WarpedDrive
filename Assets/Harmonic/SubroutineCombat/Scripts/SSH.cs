using UnityEngine;
using System.Collections;

public class SSH : NetworkLocationButton, IKeyListener, IActivatable {

    public Transform TunnelViz;
    public Transform LockViz;
    public Keystore.KeyColor KeyColor = Keystore.KeyColor.Green;
    public MachineStrategyAnchor OpenedAnchor;
    public bool ForwardToTarget = false, RightToTarget = true;

    private MachineStrategyAnchor StartAnchor;
	// Use this for initialization
	void Start () {
        StartAnchor = this.transform.root.GetComponent<MachineStrategyAnchor>();

        TunnelViz.gameObject.SetActive(false);
        this.transform.localScale = Vector3.zero;
        Keystore.OnKeyCopied += OnKeyCopied;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnKeyCopied(Keystore.KeyColor keyColor)
    {
        if (keyColor == this.KeyColor)
        {
            LockViz.gameObject.SetActive(false);
            StartCoroutine(Open());
        }
    }

    public void Activate()
    {
        TunnelViz.gameObject.SetActive(true);
        ToastLog.Toast("SSH Tunnel\nOpened");
        OpenedAnchor.myMachine.IsAccessible = true;
        if (ForwardToTarget)
        {
            this.StartAnchor.Forward = this.OpenedAnchor;
            this.OpenedAnchor.Backward = this.StartAnchor;
        }
        else if (RightToTarget)
        {
            this.StartAnchor.Right = this.OpenedAnchor;
            this.OpenedAnchor.Left = this.StartAnchor;
        }

        StartCoroutine(Close());
    }

    void OnDestroy()
    {
        Keystore.OnKeyCopied -= OnKeyCopied;
    }
}
