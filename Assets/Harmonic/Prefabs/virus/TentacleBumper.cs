using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(Rigidbody))]
public class TentacleBumper : MonoBehaviour {
    private Rigidbody rigid;

    // Use this for initialization
    void Start () {
        this.rigid = this.GetComponent<Rigidbody>();
        StartCoroutine(RandomlyBump());
	}

    private IEnumerator RandomlyBump()
    {
        while (this.isActiveAndEnabled)
        {
            yield return new WaitForSeconds(UnityEngine.Random.Range(.5f, 2f));

            rigid.AddForce(HarmonicUtils.RandomVector(-1f, 1f), ForceMode.Impulse);
        }
    }
}
