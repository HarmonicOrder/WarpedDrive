using UnityEngine;
using System.Collections;

public class CircleDraw : MonoBehaviour {   
	public float theta_scale = 0.01f;//Set lower to add more points
	public float radius = 3f;
	public Material LineMaterial;
	public float lineThickness = .1f;
	public Color StartColor = Color.red;
	public Color EndColor = Color.red;
	LineRenderer lineRenderer;
	
	private int size; //Total number of points in circle
	void Awake () {       
		float sizeValue = (2.0f * Mathf.PI) / theta_scale; 
		size = (int)sizeValue;
		size++;
		lineRenderer = gameObject.AddComponent<LineRenderer>();
		lineRenderer.material = LineMaterial;
		lineRenderer.SetWidth(lineThickness, lineThickness); //thickness of line
		lineRenderer.SetVertexCount(size);      
		lineRenderer.SetColors(StartColor, EndColor);
	}
	
	void Update () {      
		Vector3 pos;
		float theta = 0f;
		for(int i = 0; i < size; i++){          
			theta += (2.0f * Mathf.PI * theta_scale);         
			float x = radius * Mathf.Cos(theta);
			float y = radius * Mathf.Sin(theta);          
			x += gameObject.transform.position.x;
			y += gameObject.transform.position.y;
			pos = new Vector3(x, y, 0);
			lineRenderer.SetPosition(i, pos);
		}
	}
}