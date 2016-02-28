using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class LagBomb : MonoBehaviour {

    public GameObject ExplosionRoot;
    public GameObject ShellRoot;
    private bool firing;
    private HarmonicUtils.LerpContext BombLerp;

    public void Fire (Vector3 target, float duration, float lagPenalty) {
        firing = true;
        BombLerp = new HarmonicUtils.LerpContext(duration)
        {
            From = this.transform.position,
            To = target
        };

        ExplosionRoot.SetActive(false);
        ShellRoot.SetActive(true);
    }
	
	// Update is called once per frame
	void Update () {
        if (firing)
        {
            if (BombLerp.IsPastDuration())
            {
                this.transform.position = BombLerp.Finalize();
                ExplodeLagBomb();
            }
            else
            {
                this.transform.position = BombLerp.Lerp();
                BombLerp.CurrentTime += InterruptTime.deltaTime;
            }
        }
    }



    private void ExplodeLagBomb()
    {
        firing = false;

        LagTargetsInProximity(this.transform.position);

        ExplosionRoot.SetActive(true);
        ExplosionRoot.GetComponent<ParticleSystem>().Play(true);

        ShellRoot.SetActive(false);
        StartCoroutine(LetExplode());
    }

    private IEnumerator LetExplode()
    {
        yield return new WaitForSeconds(3f);
        ExplosionRoot.GetComponent<ParticleSystem>().Stop(true);
    }

    private void LagTargetsInProximity(Vector3 position)
    {

    }
}
