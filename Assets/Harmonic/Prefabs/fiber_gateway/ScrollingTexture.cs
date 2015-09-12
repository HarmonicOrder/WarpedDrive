using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshRenderer))]
public class ScrollingTexture : MonoBehaviour {

	public int materialIndex = 0;
	public Vector2 uvAnimationRate = new Vector2( 1, 0 );
	public string textureName = "_MainTex";

	private Vector2 uvOffset = Vector2.zero;
	private MeshRenderer myRender;
	void Start(){
		myRender = GetComponent<MeshRenderer>();
	}

	// Update is called once per frame
	void LateUpdate () {
		uvOffset += ( uvAnimationRate * Time.deltaTime );
		if (myRender != null)
		{
			myRender.materials[ materialIndex ].SetTextureOffset( textureName, uvOffset);
		}
	}
}
