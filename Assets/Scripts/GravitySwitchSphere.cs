using System.Collections.Generic;
using UnityEngine;
using System;

public class GravitySwitchSphere : MonoBehaviour {

    List<GameObject> dynamics = new List<GameObject>();                                         // List of Gameobject influenced by this objects gravity
    public GameObject GravitySwitchController;                                                  // Controller needs to be registered
    bool active = false;                                                                        // prevent unneccessary calculations if no objects are influenced

    // Use this for initialization
    void Start()
    {
        if (GravitySwitchController == null) Console.Error.WriteLine("GravitySwitchSphere '"    // Controller needs to be registered
            + name + "' needs GravitySwitchController");
    }

    // Update is called once per frame
    void Update()
    {
        if (active == true)
        {
            foreach (GameObject obj in dynamics)                                                // change each objects gravity 
            {
                Vector3 heading = obj.transform.position - transform.position;                  // calculate gravity direcvtion
                Vector3 direction = heading / heading.magnitude;                                // for object
                if (obj.tag == "Player")                                                        // Player needs to turn his body if gravity changes
                {
                    obj.GetComponent<GravityController>().changeDir(-direction);                // turn player
                }
                obj.GetComponent<PlayerController>().changeGravityDir(-direction);              // change gravity direction
            }
        }
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.GetComponent<GravityController>() != null)                           // check if colliding Object has a gravity
        {
            dynamics.Add(col.gameObject);                                                       // add collided object to List
            GravitySwitchController.GetComponent<GravitySwitchController>()
                .register(col.gameObject, this.gameObject);                                     // register new Object in controller
            active = true;                                                                      // activate calculations
        }
    }

    public void remove(GameObject obj)
    {
        if (!dynamics.Remove(obj)) Console.Error.WriteLine("Object not found "                  
            + "in GravitySwitchSphere");
        if(dynamics.Count == 0)
        {
            active = false;                                                                     // deactivate calculations if all objects got removed
        }
    }
}
