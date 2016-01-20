using UnityEngine;
using System.Collections;

public class LockFirmware : NetworkLocationButton, IActivatable
{
    private Machine myMachine { get; set; }
    public string DoorName;
    public ParticleSystem[] EMVisualizations;

    // Use this for initialization
    void Start()
    {
        this.transform.localScale = Vector3.zero;
        this.myMachine = CyberspaceBattlefield.Current.FindByName(this.transform.root.name);
        this.myMachine.OnSystemClean += OnSystemClean;
    }

    private void OnSystemClean()
    {
        StartCoroutine(Open());
    }

    public void Activate()
    {
        StartCoroutine(Close());
        if (!StarshipEnvironment.Instance.DoorIsOpen(this.DoorName.ToLower()))
        {
            StarshipEnvironment.Instance.OpenDoors.Add(this.DoorName.ToLower(), true);
            ToastLog.Toast("Door Unlocked!");

            foreach(ParticleSystem ps in EMVisualizations)
            {
                ps.emissionRate = 0;
            }
        }
    }

    void OnDestroy()
    {
        this.myMachine.OnSystemClean -= OnSystemClean;
    }
}
