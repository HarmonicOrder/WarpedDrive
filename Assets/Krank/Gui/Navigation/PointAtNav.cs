using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof (MeshRenderer))]
public class PointAtNav : MonoBehaviour {
	
	public SpriteRenderer SpriteChild;
	private List<Transform> navs = new List<Transform>();
	
	private Vector3 navPos;
	private void ScanNavs(){
		foreach( Transform g in navs){
			navPos = Camera.main.WorldToScreenPoint(g.position);
			PossiblyPointAtNav(g);
		}
	}

	private Color newDimmedColor, newBrighterColor;
	private void DimMaterial(Material m){
		newDimmedColor = m.color;
		if (newDimmedColor.a > 0){
			newDimmedColor.a = Mathf.Lerp(newDimmedColor.a, 0, 2f * Time.deltaTime);
		}
		m.color = newDimmedColor;
	}
	private void BrightenMaterial(Material m){
		newBrighterColor = m.color;
		if (newBrighterColor.a < 1){
			newBrighterColor.a = Mathf.Lerp(newBrighterColor.a, 1, 2f * Time.deltaTime);
		}
		m.color = newBrighterColor;
	}

	private void PossiblyPointAtNav(Transform t){
		//print(string.Format("{0}, {1}", navPos.x, navPos.z));
		if ((navPos.x >= 0) && (navPos.x <= Screen.width) && (navPos.y >= 0) && (navPos.y <= Screen.height)& (navPos.z >= 0)){
			DimMaterial(meshR.material);
			DimMaterial(SpriteChild.material);
		} else {
			BrightenMaterial(meshR.material);
			BrightenMaterial(SpriteChild.material);
			this.transform.LookAt(t.transform.position);
		}
	}
	
	private MeshRenderer meshR;
	void Start () {
		meshR = GetComponent<MeshRenderer>();
		foreach( GameObject g in GameObject.FindGameObjectsWithTag("CommNode")){
			navs.Add(g.transform);
		}
	}
	
	// Update is called once per frame
	void Update () {
		ScanNavs();
	}
}
