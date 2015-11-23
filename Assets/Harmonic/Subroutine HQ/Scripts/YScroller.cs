using UnityEngine;
using System.Collections;

public class YScroller : MonoBehaviour {

    public float startY = 0f;
    public float finishY = 0f;
    public float increment = 1f;
    public short direction = -1;

	// Use this for initialization
	void Start () {
        this.transform.localPosition = new Vector3(this.transform.localPosition.x, startY, this.transform.localPosition.z);
    }
	
	// Update is called once per frame
	void Update () {
        if (this.transform.localPosition.y < finishY)
        {
            this.transform.localPosition = new Vector3(this.transform.localPosition.x, startY, this.transform.localPosition.z);
        }
        this.transform.Translate(Vector3.up * increment * direction);
	}
}
