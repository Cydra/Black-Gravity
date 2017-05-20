using System.Collections.Generic;
using UnityEngine;

public class GravitySwitchController : MonoBehaviour {

    private Dictionary<string, GameObject> dynamics = new Dictionary<string, GameObject>();     // List of all Gameobjects with gravity and their actove switcher

    public void register(GameObject dest, GameObject switcher)
    {

        if (dynamics.ContainsKey(dest.name))                                                    // check if object is already registered
        {
            if (dynamics[dest.name].GetComponent<GravitySwitchSphere>() != null)                // check if old switcher was a sphere
            {
                dynamics[dest.name].GetComponent<GravitySwitchSphere>().remove(dest);           // remove object frome old switchers list
            }
            dynamics[dest.name] = switcher;                                                     // register new switcher to object
        } else dynamics.Add(dest.name, switcher);                                               // add object to list if not yet registered
    }
}
