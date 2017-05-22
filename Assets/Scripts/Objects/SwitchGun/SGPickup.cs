using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SGPickup : MonoBehaviour {

    public GameObject switchGun;
    void Start()
    {
        switchGun.active = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            switchGun.active = true;
            this.gameObject.active = false;
        }
    }
}
