using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshRenderer))]
public class Hardpoint : MonoBehaviour, ILockTarget {
	public Color HighlightedEmission;

	private MeshRenderer meshR;
	private Color unhighlighted;
	private int emissionColorPropertyID;

	// Use this for initialization
	void Start () {
		this.emissionColorPropertyID = Shader.PropertyToID("_EmissionColor");
		this.meshR = this.GetComponent<MeshRenderer>();
		this.unhighlighted = this.meshR.material.GetColor(this.emissionColorPropertyID);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void EnableLockedOnGui()
	{
		print ("stuff");
		this.meshR.material.SetColor(this.emissionColorPropertyID, HighlightedEmission);
	}
	
	public void DisableLockedOnGui()
	{
		this.meshR.material.SetColor(this.emissionColorPropertyID, unhighlighted);
	}
}
