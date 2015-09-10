using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class LaserGun : MonoBehaviour {

	public Transform laserBeamPrefab;

	public bool PrimaryFire = true;
	public bool AltFire { get {
			return !PrimaryFire;
		}
		set {
			PrimaryFire = !value;
		}
	}

	public bool RotateAxisFix = false;

	public float coolDown = 5f;

	private string axis {get{
			if (PrimaryFire){
				return "Fire1";
			} else {
				return "Fire2";
			}
		}
	}

	private Animation a;
	void Start () {
		a = this.GetComponent<Animation>();
	}

	private bool isCooledOff = true;

	// Update is called once per frame
	void Update () {
		if (CrossPlatformInputManager.GetAxis(axis) > 0f){
			if (isCooledOff){
				FireLaserBeam();
			} else if (!hasCoroutine) {
				StartCoroutine(Cooldown());
			}
		}
	}

	private bool hasCoroutine = false;
	IEnumerator Cooldown() {
		hasCoroutine = true;
		yield return new WaitForSeconds(coolDown);
		hasCoroutine = false;
		isCooledOff = true;
	}

	private void FireLaserBeam(){
		isCooledOff = false;
		Object o = Instantiate(laserBeamPrefab, this.transform.position, this.transform.rotation);
		if (RotateAxisFix)
			(o as Transform).Rotate(Vector3.right, -90f);

		AnimateFire();
	}

	private void AnimateFire(){
		if (a != null){
			a.Play();
		} 
	}
}
