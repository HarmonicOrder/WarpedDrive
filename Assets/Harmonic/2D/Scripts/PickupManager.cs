using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;
using System;

public class PickupManager : MonoBehaviour {
    
    public float PickupDistance = 1f;
    public float OxygenMinutesPerPickup = 4f;
    public float OxygenMinutePerTick = .2f;
    public uint RAMPerPickup = 2;
    public Transform Character;

    Coroutine check;

    private float ticksOfOxygenPerPickup;
    private float downscalePerTickForOxygen;

    // Use this for initialization
    void Start()
    {
        ticksOfOxygenPerPickup = (OxygenMinutesPerPickup / OxygenMinutePerTick);
        downscalePerTickForOxygen = (1 / ticksOfOxygenPerPickup);
        DestroyAlreadyPickedUpPickups();
        check = StartCoroutine(CheckPickups());
    }

    private void DestroyAlreadyPickedUpPickups()
    {
        foreach(Transform t in this.transform)
        {
            if (StarshipEnvironment.Instance.PickupsPickedUp.Contains(t.name.ToLower()))
            {
                GameObject.Destroy(t.gameObject);
            }
        }
    }

    private IEnumerator CheckPickups()
    {
        while (enabled)
        {
            foreach (Transform t in this.transform)
            {
                if (Vector3.Distance(Character.position, t.position) < PickupDistance)
                {
                    PickupTypes type = PickupTypes.oxygen;
                    //only get the first part of the name "oxygen1" becomes "oxygen"
                    if (EnumExtensions.TryParse<PickupTypes>(type, new Regex(@"([a-z]*)").Match(t.name.ToLower()).Captures[0].Value, out type))
                    {
                        bool destroy = true;
                        switch(type)
                        {
                            case PickupTypes.oxygen:
                                Transform scaler = t.GetChild(0).transform;
                                if (scaler.localScale.y > 0)
                                {
                                    if (StarshipEnvironment.Instance.OxygenLevel < StarshipEnvironment.Instance.OxygenStorage)
                                    {
                                        StarshipEnvironment.Instance.OxygenLevel += StarshipEnvironment.OxygenConsumedPerSecond * 60 * (OxygenMinutePerTick);
                                        //print(scaler.localScale.y - downscalePerTickForOxygen);
                                        scaler.localScale = new Vector3(1, scaler.localScale.y - downscalePerTickForOxygen, 1);
                                        t.GetComponent<AudioSource>().pitch = UnityEngine.Random.Range(.7f, 1.2f);
                                        t.GetComponent<AudioSource>().Play();
                                    }
                                    destroy = false;
                                }
                                //todo: fire OnOxygenPickup
                                break;
                            case PickupTypes.ram:
                                CyberspaceEnvironment.Instance.MaximumRAM += this.RAMPerPickup;
                                //todo: fire OnRAMPickup
                                break;
                        }
                        StarshipEnvironment.Instance.PickupsPickedUp.Add(t.name.ToLower());
                        if (destroy)
                            GameObject.Destroy(t.gameObject);
                    }
                    break;
                }
            }

            yield return new WaitForSeconds(.5f);
        }
    }

    void OnDestroy()
    {
        StopCoroutine(check);
    }

    public enum PickupTypes
    {
        oxygen,
        ram
    }
}
