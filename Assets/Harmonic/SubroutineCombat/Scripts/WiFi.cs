using UnityEngine;
using System.Collections;

public class WiFi : MonoBehaviour {
    public MeshRenderer DepthBoxRenderer;
    public TextMesh LabelRenderer;
    public ParticleSystem WifiRenderer;

    private Machine myMachine; 
	// Use this for initialization
	void Start () {
        this.myMachine = CyberspaceBattlefield.Current.FindByName(this.transform.root.name);
        DepthBoxRenderer.enabled = false;
        StartCoroutine(DisableWifi());
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public IEnumerator DisableWifi()
    {
        yield return new WaitForSecondsInterruptTime(2f);
        this.myMachine.IsAccessible = false;
        DepthBoxRenderer.enabled = true;
        ParticleSystem.EmissionModule em = WifiRenderer.emission;
        em.rate = new ParticleSystem.MinMaxCurve(0);
        WifiRenderer.Clear();
        LabelRenderer.color = new Color(0, 0, 0, 0);
        ToastLog.Toast(this.myMachine.SubnetAddress + " has lost connection!");
    }
}
