using UnityEngine;
using System.Collections;

public class PixelateIn : MonoBehaviour {

	public float pixelateInTime = 4f;
	public float fromCellSize = 0.05f;
	public float toCellSize = .001f;

	private float currentPixelateTime = 0f;
	private MeshRenderer meshRender;

	// Use this for initialization
	void Start () {
		this.meshRender = this.GetComponent<MeshRenderer>();
	}
	
	// Update is called once per frame
	void Update () {

		currentPixelateTime += InterruptTime.deltaTime;

		if (currentPixelateTime <= pixelateInTime)
		{
			float lerpValue = Mathf.Lerp(fromCellSize, toCellSize, currentPixelateTime / pixelateInTime);

			this.meshRender.material.SetVector("_CellSize", new Vector4(lerpValue, lerpValue, 0, 0));
		}
	}
}
