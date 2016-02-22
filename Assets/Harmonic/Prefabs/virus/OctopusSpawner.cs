using UnityEngine;
using System.Collections;
using System;

public class OctopusSpawner : MeshNode
{
    public Transform InstantiationPrefab;
    public Transform InstantiationPoint;
    public ParticleSystem InstantiateParticles;
    public bool CanReplenishMalware = true;
    public float TimeToReplenishSingleMalware = 5f;

    public override VirusAI.VirusType Type { get { return VirusAI.VirusType.Ransomware; } }

    private Transform targetT;
    private Coroutine Replenish;
    private int malwareQueue = 0;

    void Start()
    {
        ActiveSubroutines.OnMalwareListChange += OnMalwareListChange;
        InstantiateParticles.emissionRate = 0;
    }

    void OnDestroy()
    {
        ActiveSubroutines.OnMalwareListChange -= OnMalwareListChange;
        if (Replenish != null)
            StopCoroutine(Replenish);
    }

    private void OnMalwareListChange(IMalware dead)
    {
        if (CanReplenishMalware && (dead != null) && (dead.gameObject.transform.root.name == this.gameObject.transform.root.name))
        {
            malwareQueue += 1;
            //if there is no active coroutine, start it
            if (Replenish == null)
            {
                Replenish = StartCoroutine(ReplenishMalware());
            }
        }
    }

    private ParticleSystem.EmissionModule particlesEmission;
    private IEnumerator ReplenishMalware()
    {
        particlesEmission = InstantiateParticles.emission;
        particlesEmission.rate = new ParticleSystem.MinMaxCurve(10f);
        yield return new WaitForSecondsInterruptTime(TimeToReplenishSingleMalware - 1);
        particlesEmission.rate = new ParticleSystem.MinMaxCurve(40f);
        yield return new WaitForSecondsInterruptTime(1f);
        DoReplenishMalware();
        particlesEmission.rate = new ParticleSystem.MinMaxCurve(0f);

        //if there are malware to create, recall this coroutine
        if (malwareQueue > 0)
        {
            Replenish = StartCoroutine(ReplenishMalware());
        }
        else
        {
            //otherwise null out this reference
            Replenish = null;
        }
    }

    /// <summary>
    /// creates malware, decrements malwareQueue
    /// </summary>
    private void DoReplenishMalware()
    {
        Transform t = (Transform)GameObject.Instantiate(InstantiationPrefab, InstantiationPoint.position, InstantiationPoint.rotation);
        t.SetParent(this.transform.root);

        VirusAI newV = t.GetComponent<VirusAI>();
        if (newV != null)
        {
            newV.DoOnAwake();
            if (ActiveSubroutines.List.Count > 0)
            {
                newV.OnSubroutineActive(null);
            }
        }

        malwareQueue -= 1;
    }
}
