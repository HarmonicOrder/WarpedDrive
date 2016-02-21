using UnityEngine;
using System.Collections;

public class DOS : MonoBehaviour {
	public float Damage = 1f;

	// Use this for initialization
	void Start () {
		IterationTime = .5f;
		StartCoroutine(FireOutOfHangar());
	}

	private Transform pointAt;
	private float lookAtSpeed = 4f;

	// Update is called once per frame
	void Update () {
		if (pointAt != null)
		{
			this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(pointAt.position), InterruptTime.deltaTime * lookAtSpeed);
		}
	}

	private float CurrentIterationTime = 0f;
	private float IterationTime = 0f;
	private IEnumerator FireOutOfHangar()
	{
		while(CurrentIterationTime < IterationTime)
		{
			CurrentIterationTime += Time.deltaTime;
			this.transform.Translate(Vector3.forward * .33f);
			yield return null;
		}
		StartCoroutine(MoveToSubnet());
	}

	private Vector3 lerpFrom;
	private IMalware target;
	private IEnumerator MoveToSubnet()
	{
		CurrentIterationTime = 0f;
		IterationTime = 1f;

		this.pointAt = this.transform.root.Find("EthernetJack");
		this.lerpFrom = this.transform.position;

		while(CurrentIterationTime < IterationTime)
		{
			CurrentIterationTime += InterruptTime.deltaTime;
			this.transform.position = Vector3.Lerp(lerpFrom, this.pointAt.position, CurrentIterationTime / IterationTime);
			yield return null;
		}

		AcquireTarget();

		this.pointAt = this.target.transform.root.Find("EthernetJack");


		while(CurrentIterationTime < IterationTime)
		{
			CurrentIterationTime += InterruptTime.deltaTime;
			this.transform.position = Vector3.Lerp(lerpFrom, this.pointAt.position, CurrentIterationTime / IterationTime);
			yield return null;
		}

		StartCoroutine(MoveTowardsTarget());
	}

	private IEnumerator MoveTowardsTarget()
	{
		CurrentIterationTime = 0f;
		IterationTime = 1f;
		this.lerpFrom = this.transform.position;
		this.pointAt = this.target.transform;

		while(CurrentIterationTime < IterationTime)
		{
			CurrentIterationTime += InterruptTime.deltaTime;
			if ((this.target == null) || ((this.target as MonoBehaviour).enabled == false))
				AcquireTarget();

			if (this.target != null)
				this.transform.position = Vector3.Lerp(lerpFrom, this.target.transform.position, CurrentIterationTime / IterationTime);
			yield return null;
		}

#warning DOS is toothless now
        //if (this.target != null)
        //	this.target.TakeDamage(Damage);

        GameObject.Destroy(this.gameObject);

	}

	private void AcquireTarget()
	{
		this.target = ActiveSubroutines.FindClosestMalware(this.transform.position, 999f);
		this.lerpFrom = this.transform.position;
		CurrentIterationTime = 0f;
		IterationTime = 1f;
	}


}
