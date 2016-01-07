using UnityEngine;
using System.Collections;

public class LockFirmware : NetworkLocationButton, IActivatable
{
    private Machine myMachine { get; set; }
    public string DoorName;

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

    // Update is called once per frame
    void Update()
    {

    }

    public void Activate()
    {
        StartCoroutine(Close());
        if (!StarshipEnvironment.Instance.DoorIsOpen(this.DoorName.ToLower()))
        {
            StarshipEnvironment.Instance.OpenDoors.Add(this.DoorName.ToLower(), true);
            ToastLog.Toast("Door Unlocked!");
        }
    }

    void OnDestroy()
    {
        this.myMachine.OnSystemClean -= OnSystemClean;
    }
}
