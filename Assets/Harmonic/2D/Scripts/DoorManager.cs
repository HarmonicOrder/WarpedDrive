using UnityEngine;
using System.Collections;

public class DoorManager : MonoBehaviour {

    public Transform[] Doors;

    // Use this for initialization
    void Start () {
	    foreach(string door in StarshipEnvironment.DoorNames)
        {
            if (StarshipEnvironment.Instance.DoorIsOpen(door))
            {
                foreach(Transform t in Doors)
                {
                    if (t.name.ToLower().Contains(door))
                        t.gameObject.SetActive(false);
                }
            }
        }
	}
}
