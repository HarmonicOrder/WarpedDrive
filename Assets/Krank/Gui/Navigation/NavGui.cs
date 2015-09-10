using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NavGui : MonoBehaviour {

	public RectTransform CommPrefab;

	private Vector3 navPos;
	private void ScanNavs(){
		foreach( Transform g in navs){
			navPos = Camera.main.WorldToScreenPoint(g.position);
			DisplayNavSpriteBounds(g);
		}
	}

	private Dictionary<Transform, RectTransform> NavSpritesByWorldNodes = new Dictionary<Transform, RectTransform>();

	private void CheckIfNavSpriteExists(Transform t){
		if (!NavSpritesByWorldNodes.ContainsKey(t)){
			NavSpritesByWorldNodes.Add(t, Instantiate(CommPrefab));
			NavSpritesByWorldNodes[t].SetParent(this.transform);
		} 
	}

	private float wOffset;
	private float hOffset;
	private void DisplayNavSprite(Transform t, float xCoefficient, float yCoefficient){
		CheckIfNavSpriteExists(t);
		if (xCoefficient == 0)
			wOffset = 50f;
		else if (xCoefficient == 1)
			wOffset = -100f;

		if (yCoefficient == 0)
			hOffset = 50f;
		else if (yCoefficient == 1)
			hOffset = -100f;

		//print (string.Format("xcoef:{0}\nycoef:{1}", xCoefficient, yCoefficient));

		NavSpritesByWorldNodes[t].transform.gameObject.SetActive(true);
		NavSpritesByWorldNodes[t].position = new Vector3((
			(Screen.width) * xCoefficient) + wOffset, 
		    ((Screen.height) * yCoefficient)+ hOffset);
	}

	private float xOffset;
	private float yOffset;
	private void DisplayNavSpriteBounds(Transform t){
		//print(string.Format("x:{0} y:{1}", navPos.x, navPos.y));

		if ((navPos.x >= 0) && (navPos.x <= Screen.width) && (navPos.y >= 0) && (navPos.y <= Screen.height)){
			if (NavSpritesByWorldNodes.ContainsKey(t)){
				NavSpritesByWorldNodes[t].transform.gameObject.SetActive(false);
			}
			return;
		}

		xOffset = Screen.width - Mathf.Abs(navPos.x);
		yOffset = Mathf.Abs(navPos.y) - Screen.height;

		if (Mathf.Abs(xOffset) > Mathf.Abs(yOffset)){
			if (navPos.x > 0){
				//print(string.Format("right x:{0} y:{1} xoff:{2} yoff:{3}", navPos.x, navPos.y, xOffset, yOffset));
				DisplayNavSprite(t, 1, Mathf.Clamp(Mathf.Abs(yOffset/Screen.height), 0, 1)); //offscreen right
			} else {
				//print(string.Format("left x:{0} y:{1} xoff:{2} yoff:{3}", navPos.x, navPos.y, xOffset, yOffset));
				DisplayNavSprite(t, 0, Mathf.Clamp(Mathf.Abs(yOffset/Screen.height), 0, 1)); //offscreen left
			}
		} else {
			if (navPos.y > 0){ //off-screen top
				//print(string.Format("top x:{0} y:{1} xoff:{2} yoff:{3}", navPos.x, navPos.y, xOffset, yOffset));
				DisplayNavSprite(t, Mathf.Clamp(Mathf.Abs(xOffset/Screen.width), 0, 1), 1);
			} else { //offscreen bottom
				//print(string.Format("bottom x:{0} y:{1} xoff:{2} yoff:{3}", navPos.x, navPos.y, xOffset, yOffset));
				DisplayNavSprite(t, Mathf.Clamp(Mathf.Abs(xOffset/Screen.width), 0, 1), 0);
			}
		} 
	}

	private List<Transform> navs = new List<Transform>();

	// Use this for initialization
	void Start () {
		foreach( GameObject g in GameObject.FindGameObjectsWithTag("CommNode")){
			navs.Add(g.transform);
		}
		//InvokeRepeating("ScanNavs", 0f, .1f);
	}
	
	// Update is called once per frame
	void Update () {
		ScanNavs();
	}
}
