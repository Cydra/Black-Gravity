using UnityEngine;
using System;

public class GravitySwitcher : MonoBehaviour {

    void OnCollisionEnter(Collision col){
        Vector3 newGrav = col.contacts[0].normal;
        GameObject obj = col.gameObject;

        Debug.Log("Switch!");

        if (obj.tag == "Player")
        {
            obj.GetComponent<PlayerController>().changeGravityDir(newGrav);                 // Change Dir of body if player
        }
        obj.GetComponent<GravityController>().changeDir(newGrav);                       // change gravity direction
    }
}