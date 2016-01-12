using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;
using System;

public class PickupManager : MonoBehaviour {
    
    public float PickupDistance = 1f;
    public float OxygenMinutesPerPickup = 4f;
    public uint RAMPerPickup = 2;
    public Transform Character;

    Coroutine check;


    // Use this for initialization
    void Start()
    {
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
                    if (type.TryParse<PickupTypes>(new Regex(@"([a-z]*)").Match(t.name.ToLower()).Captures[0].Value, out type))
                    {
                        switch(type)
                        {
                            case PickupTypes.oxygen:
                                StarshipEnvironment.Instance.OxygenLevel += StarshipEnvironment.OxygenConsumedPerSecond * 60 * this.OxygenMinutesPerPickup;
                                break;
                            case PickupTypes.ram:
                                CyberspaceEnvironment.Instance.MaximumRAM += this.RAMPerPickup;
                                break;
                        }
                        StarshipEnvironment.Instance.PickupsPickedUp.Add(t.name.ToLower());
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
