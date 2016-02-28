using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class LagBomb : MonoBehaviour {

    public GameObject ExplosionRoot;
    public GameObject ShellRoot;
    public ParticleSystem ps1, ps2, ps3;
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
        //doesn''t work!
        //ExplosionRoot.GetComponent<ParticleSystem>().Play(true);
        ps1.Play();
        //it's a bug,
        //https://issuetracker.unity3d.com/issues/particle-system-plays-only-once
        //how the heck is this a workaround
        ps2.Play();
        ps2.startLifetime = ps2.startLifetime;
        ps3.Play();
        ps3.startLifetime = ps3.startLifetime;

        ShellRoot.SetActive(false);
        StartCoroutine(LetExplode());
    }

    private IEnumerator LetExplode()
    {
        yield return new WaitForSeconds(3f);
        ps1.Stop();
        ps2.Stop();
        ps3.Stop();
        //doesn't work!
        //ExplosionRoot.GetComponent<ParticleSystem>().Stop(true);
        //ExplosionRoot.GetComponent<ParticleSystem>().Clear(true);
    }

    private void LagTargetsInProximity(Vector3 position)
    {

    }
}
