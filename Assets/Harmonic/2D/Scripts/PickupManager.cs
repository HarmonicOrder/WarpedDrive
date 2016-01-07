using UnityEngine;
using System.Collections;

public class PickupManager : MonoBehaviour {
    
    public float PickupDistance = 1f;
    public float OxygenMinutesPerPickup = 4f;
    public uint RAMPerPickup = 2;
    public Transform Character;

    Coroutine check;


    // Use this for initialization
    void Start()
    {
        check = StartCoroutine(CheckPickups());
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
                    if (type.TryParse<PickupTypes>(t.name.ToLower(), out type))
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
