using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(ParticleSystem))]
public class LazerBeam : MonoBehaviour {

	public float laserSpeed = 20f;
	public float lifetime = 10f;
	public float damage = 1f;
	private bool liveFire = true;

	// Use this for initialization
	void Start () {
		StartCoroutine(SelfDestruct());
	}
	
	// Update is called once per frame
	void Update () {
		if (liveFire)
			this.transform.Translate(Vector3.forward*laserSpeed*Time.deltaTime, Space.Self);
	}

	IEnumerator SelfDestruct() {
		yield return new WaitForSeconds(lifetime);
		UnityEngine.Object.Destroy(this.gameObject);
	}

	void OnCollisionEnter(Collision coll){
		if (this.liveFire)
		{
			this.liveFire = false;

			if (coll.gameObject != null)
			{
				VirusAI v = coll.gameObject.GetComponent<VirusAI>();
				if (v != null)
				{
					v.TakeDamage(damage);
				}

				Subroutine s = coll.gameObject.GetComponent<Subroutine>();
				if (s != null)
				{
					s.TakeDamage(damage);
				}
				print (coll.gameObject.name);
			}

			killSelf();
		}
	}

	private void killSelf()
	{
		this.GetComponent<Rigidbody>().isKinematic = false;
		this.transform.GetComponentInChildren<LineRenderer>().enabled = false;
		this.transform.GetComponent<ParticleSystem>().Emit(30);
		this.transform.GetComponent<Collider>().enabled = false;
	}
}
