using UnityEngine;
using System.Collections;

public class GunAimer : MonoBehaviour {

	public Transform leftGun;
	public Transform rightGun;
	public Transform leftCrosshair;
	public Transform rightCrosshair;
	public Transform outerCrosshair;
	public Transform hitCrosshair;

	public float DefaultCrosshairDistance = 20.25f;
	public float MaximumCrosshairDistance = 300f;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		RaycastHit hit;
		Debug.DrawRay(this.transform.position, Vector3.forward, Color.red, 99f, false);
		if (Physics.Raycast(this.transform.position, this.transform.forward, out hit, Mathf.Infinity)){
			leftGun.LookAt(hit.point);
			rightGun.LookAt(hit.point);
			hitCrosshair.gameObject.SetActive(true);
			if (hit.distance < MaximumCrosshairDistance){
				//leftCrosshair.localPosition = Vector3.forward * (hit.distance-1.5f);
				//rightCrosshair.localPosition = Vector3.forward * (hit.distance-1.5f);
				outerCrosshair.gameObject.SetActive(true);
				outerCrosshair.localPosition = Vector3.forward * (hit.distance-1.5f);
			} else {
				HideOuterCrosshair();
			}
		} else {
			hitCrosshair.gameObject.SetActive(false);
			leftGun.localRotation = Quaternion.Euler( Vector3.up * 5.75f);
			rightGun.localRotation = Quaternion.Euler( Vector3.up * (360f -5.75f));
			HideOuterCrosshair();
		}
	}

	private void HideOuterCrosshair(){
		//leftCrosshair.localPosition = Vector3.forward * (DefaultCrosshairDistance);
		//rightCrosshair.localPosition = Vector3.forward * (DefaultCrosshairDistance);
		outerCrosshair.localPosition = Vector3.forward * (DefaultCrosshairDistance);
		outerCrosshair.gameObject.SetActive(false);
	}
}
