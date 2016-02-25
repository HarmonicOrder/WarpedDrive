using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(ParticleSystem))]
public class LazerTorpedo : MonoBehaviour {

	public float laserSpeed = 20f;
	public float lifetime = 10f;
    public ICombatant origin;

	private bool liveFire = false;
    private GameObject targetObj;
    private ICombatant target;
    private bool dudFire = false;

    // Use this for initialization
    public void FireTorpedo (ICombatant target) {
		StartCoroutine(SelfDestruct());
        this.liveFire = true;
        this.target = target;
        this.targetObj = target.gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		if (liveFire && (target != null) && (targetObj != null))
        {
            this.transform.LookAt(target.transform);
			this.transform.Translate((target.transform.position-this.transform.position).normalized*laserSpeed*InterruptTime.deltaTime, Space.World);
        }
        else if (dudFire)
        {
            this.transform.Translate(Vector3.forward * laserSpeed * InterruptTime.deltaTime, Space.Self);
        }
	}

	IEnumerator SelfDestruct() {
		yield return new WaitForSecondsInterruptTime(lifetime);
		UnityEngine.Object.Destroy(this.gameObject);
	}

	void OnCollisionEnter(Collision coll){
		if (this.liveFire)
		{
			this.liveFire = false;

            bool hit = false;
			if (coll.gameObject != null)
			{
				IMalware v = coll.gameObject.GetComponent<IMalware>();
				if (v != null)
				{
                    hit = origin.DoAttack(v);
				}

				Subroutine s = coll.gameObject.GetComponent<Subroutine>();
				if (s != null)
                {
                    hit = origin.DoAttack(s);
				}
			}

            if (hit)
                killSelf();
            else
            {
                Rigidbody r = this.GetComponent<Rigidbody>();
                r.detectCollisions = false;
                r.freezeRotation = true;
                r.velocity = Vector3.zero;
                this.dudFire = true;
            }
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
