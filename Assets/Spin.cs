using UnityEngine;
using System.Collections;

public class Spin : MonoBehaviour {
	
	public enum Axis {X, Y, Z}

	public float ZOffset = 1f;
	public float speed = 10f;
	public Axis RotationAxis = Axis.Y;
    public bool UseInterruptTime = false;

	private Vector3 position;
	void Start () {
		position = this.transform.position;
		position.z += ZOffset;
	}


	// Update is called once per frame
	void Update () {
        float rotationAmount = 0f;
        if (UseInterruptTime)
        {
            rotationAmount = InterruptTime.deltaTime;
        }
        else
        {
            rotationAmount = Time.deltaTime;
        }

		this.transform.Rotate(GetVector(), speed*rotationAmount);
	}

	private Vector3 GetVector(){
		switch(RotationAxis){
		case Axis.X:
			return Vector3.right;
		case Axis.Y:
			return Vector3.up;
		case Axis.Z:
			return Vector3.forward;
		default:
			return Vector3.up;
		}
	}
}
