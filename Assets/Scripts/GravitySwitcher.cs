using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravitySwitcher : MonoBehaviour {

	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}

	// Just a demo -> changes gravity to +z
	// Switch the gravity when touching the surface of the object
	void OnCollisionEnter(Collision col)
	{
		GameObject obj = col.gameObject;
		if (obj.tag == "Player") {
			obj.GetComponent<GravityController> ().changeDir (-transform.up);
			obj.GetComponent<PlayerController> ().changeGravityDir (-transform.up);
		} else {
			obj.GetComponent<GravityController> ().changeDir (-transform.up);
		}
	}
}
