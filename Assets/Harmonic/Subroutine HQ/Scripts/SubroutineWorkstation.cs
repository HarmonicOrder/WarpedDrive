using UnityEngine;
using System.Collections;

public class SubroutineWorkstation : MonoBehaviour {

    public float rotateTime = .5f;

    bool isRotating = false;
    float currentRotateTime = 0;
    Quaternion from, to;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (isRotating)
        {
            this.transform.localRotation = Quaternion.Slerp(from, to, currentRotateTime / rotateTime);
            if (currentRotateTime >= rotateTime)
            {
                this.transform.localRotation = to;
                isRotating = false;
            }
            currentRotateTime += Time.deltaTime;
        }
	}

    public void listingToSubroutine()
    {
        currentRotateTime = 0f;
        from = this.transform.localRotation;
        to = Quaternion.Euler(Vector3.up * 90f);
        isRotating = true;
    }

    public void subroutineToListing()
    {
        currentRotateTime = 0f;
        from = this.transform.localRotation;
        to = Quaternion.Euler(Vector3.up);
        isRotating = true;
    }
}
