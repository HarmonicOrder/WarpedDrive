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
	public bool OnYAxis = false;

	private int numOrbiters = 0;
	public int NumberOfOrbiters
	{
		get {return numOrbiters;}
		set
		{
			numOrbiters = value;
			if (value < 1)
			{
				lineRenderer.enabled = false;
			} else if (!lineRenderer.enabled){
				lineRenderer.enabled = true;
			}
		}
	}
	
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
		Draw ();
	}
	
	void Update () {
	}

	public void Draw()
	{      
		Vector3 pos;
		float theta = 0f;
		for(int i = 0; i < size; i++){          
			theta += (2.0f * Mathf.PI * theta_scale);         
			float x = radius * Mathf.Cos(theta);
			float y = radius * Mathf.Sin(theta);          
			float z = radius * Mathf.Sin(theta);          
			x += gameObject.transform.position.x;
			if (OnYAxis)
			{
				z += gameObject.transform.position.z;
				pos = new Vector3(x, gameObject.transform.position.y, z);
			}
			else
			{
				y += gameObject.transform.position.y;
				pos = new Vector3(x, y, gameObject.transform.position.z);
			}
			lineRenderer.SetPosition(i, pos);
		}
	}
}