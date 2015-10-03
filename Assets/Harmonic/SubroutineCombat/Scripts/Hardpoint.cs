using UnityEngine;
using System.Collections;

public class Hardpoint : MonoBehaviour, ILockTarget {
	public Color HighlightedEmission;
	public MeshRenderer lockedOnMesh;

	private Color unhighlighted;
	private int emissionColorPropertyID;

	// Use this for initialization
	void Start () {
		this.emissionColorPropertyID = Shader.PropertyToID("_EmissionColor");
		this.unhighlighted = this.lockedOnMesh.material.GetColor(this.emissionColorPropertyID);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void EnableLockedOnGui()
	{
		this.lockedOnMesh.material.SetColor(this.emissionColorPropertyID, HighlightedEmission);
	}
	
	public void DisableLockedOnGui()
	{
		this.lockedOnMesh.material.SetColor(this.emissionColorPropertyID, unhighlighted);
	}
}
