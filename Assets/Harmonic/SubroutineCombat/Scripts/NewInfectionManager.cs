using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class NewInfectionManager : MonoBehaviour {
    public static NewInfectionManager Instance;

    public Transform WabbitPrefab, StandardPrefab, TankPrefab, BombPrefab;
    public bool AllowWabbit, AllowStandard, AllowTank, AllowBomb;
    public List<Transform> InvasionForce { get; set; }

    //idea: 'threat level' that can be tied to UI

    private List<MachineStrategyAnchor> invasionCandidates = new List<MachineStrategyAnchor>();
    private int MachinesToInfect = 1;
    private MachineStrategyAnchor MachineInvasionTarget;
    private float InvasionRadius = 175f;
    private Coroutine WaitingInvasion { get; set; }
    private float CurrentWaitDuration { get; set; }
    private bool IsPlottingOrInvading = false;

    private static float InvasionSuccessCountdownInSeconds = 30;

    void Awake()
    {
#if UNITY_EDITOR
        NetworkMap.CurrentLocation = NetworkMap.GetLocationByCurrentScene();
#endif
        Instance = this;
    }

    // Use this for initialization
    void Start() {
        for (int i = 0; i < CyberspaceBattlefield.Current.CurrentNetwork.Machines.Count; i++)
        {
            MachineStrategyAnchor msa = GameObject.Find(CyberspaceBattlefield.Current.CurrentNetwork.Machines[i].Name).GetComponent<MachineStrategyAnchor>();
            invasionCandidates.Add(msa);
            msa.myMachine.OnMachineClean += OnMachineClean;
        }

        InvasionForce = new List<Transform>();
    }

    private void OnMachineClean()
    {
        MachinesToInfect++;

        StartRandomCountdown();
    }

    /// <summary>
    /// starts an invasion countdown if not currently planning or executing a reinfection
    /// and if there are more than one machine
    /// and if the player hasn't won
    /// </summary>
    private void StartRandomCountdown()
    {
        if (!IsPlottingOrInvading && (MachinesToInfect > 1) && NetworkMap.CurrentLocation.IsInfected)
        {
            WaitingInvasion = StartCoroutine(WaitUntilInvasion());
        }
    }

    private IEnumerator WaitUntilInvasion()
    {
        IsPlottingOrInvading = true;
        InvasionForce = GetRandomInvasionForce();
        yield return new WaitForSecondsInterruptTime(GetRandomInvasionTime());
        MachineInvasionTarget = FindRandomMachine();
        StartInvasion();
    }

    private List<Transform> invasionForceCandidates = new List<Transform>();
    private Coroutine WaitToWin;

    private List<Transform> GetRandomInvasionForce()
    {
        InvasionForce.Clear();
        invasionForceCandidates.Clear();

        if (AllowWabbit)
            invasionForceCandidates.Add(WabbitPrefab);
        if (AllowStandard)
            invasionForceCandidates.Add(StandardPrefab);
        if (AllowTank)
            invasionForceCandidates.Add(TankPrefab);
        if (AllowBomb)
            invasionForceCandidates.Add(BombPrefab);

        int NumOfInvaders = UnityEngine.Random.Range(3, 6);

        int randomIndex = -1;
        for (int i = 0; i < NumOfInvaders; i++)
        {
            randomIndex = UnityEngine.Random.Range(0, invasionForceCandidates.Count);
            InvasionForce.Add(invasionForceCandidates[randomIndex]);
        }

        return InvasionForce;
    }

    private MachineStrategyAnchor FindRandomMachine()
    {
        int randomIndex = UnityEngine.Random.Range(0, MachinesToInfect);

        return invasionCandidates.FindAll(c => !c.myMachine.IsInfected)[randomIndex];
    }

    /// <summary>
    /// begins an invasion if there is a target and the battlefield is infected
    /// </summary>
    private void StartInvasion()
    {
        if ((MachineInvasionTarget != null) && NetworkMap.CurrentLocation.IsInfected)
        {
            MachineInvasionTarget.myMachine.StartReinfection();

            foreach(Transform t in InvasionForce)
            {
                Transform newT = GameObject.Instantiate<Transform>(t);
                VirusAI.AfterInstantiateVirus(newT, MachineInvasionTarget.transform);
                newT.localPosition = Vector3.back * InvasionRadius;
                newT.RotateAround(MachineInvasionTarget.transform.position, Vector3.up, UnityEngine.Random.Range(0f, 360f));
            }

            WaitToWin = StartCoroutine(WaitUntilWin());
        }
    }

    private IEnumerator WaitUntilWin()
    {
        yield return new WaitForSecondsInterruptTime(InvasionSuccessCountdownInSeconds);

        if (MachineInvasionTarget.myMachine.IsBeingReinfected)
        {
            OnInvasionSuccess();
        }
        else
        {
            OnInvasionFailure();
        }
    }

    public void OnInvasionSuccess()
    {
        MachinesToInfect--;
        StartRandomCountdown();
    }

    public void OnInvasionFailure()
    {
        MachinesToInfect--;
        StartRandomCountdown();
    }

    private float GetRandomInvasionTime()
    {
#if UNITY_EDITOR
        this.CurrentWaitDuration = UnityEngine.Random.Range(30f, 60f);
#else
        this.CurrentWaitDuration = UnityEngine.Random.Range(30f, 120f);
#endif
        return this.CurrentWaitDuration;
    }

    void OnDestroy()
    {
        foreach(MachineStrategyAnchor msa in invasionCandidates)
        {
            msa.myMachine.OnMachineClean -= OnMachineClean;
        }
    }
}
