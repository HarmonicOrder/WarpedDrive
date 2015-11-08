using UnityEngine;
using System.Collections;

public class Hardpoint : MonoBehaviour, ILockTarget {
	public Color HighlightedEmission;
	public Color HighlightedSprite;
	public MeshRenderer lockedOnMesh;
	public SpriteRenderer lockedOnSprite;
    
	private Color unhighlighted;
	private int emissionColorPropertyID;
	private Color unhighlightedSprite;
	// Use this for initialization
	void Start () {
		this.emissionColorPropertyID = Shader.PropertyToID("_EmissionColor");
		this.unhighlighted = this.lockedOnMesh.material.GetColor(this.emissionColorPropertyID);
		if (lockedOnSprite != null){
			unhighlightedSprite =  lockedOnSprite.color;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void EnableLockedOnGui()
	{
		this.lockedOnMesh.material.SetColor(this.emissionColorPropertyID, HighlightedEmission);
		if (this.lockedOnSprite != null){
			this.lockedOnSprite.color = HighlightedSprite;
		}
	}
	
	public void DisableLockedOnGui()
	{
		this.lockedOnMesh.material.SetColor(this.emissionColorPropertyID, unhighlighted);
		if (this.lockedOnSprite != null){
			this.lockedOnSprite.color = unhighlightedSprite;
		}
	}
}
