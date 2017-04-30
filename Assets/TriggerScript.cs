using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerScript : MonoBehaviour {
	public bool triggered = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (triggered) {
			gameObject.GetComponent<MeshRenderer> ().enabled = false;
			gameObject.GetComponent<BoxCollider> ().enabled = false;
		} else {
			gameObject.GetComponent<MeshRenderer> ().enabled = true;
			gameObject.GetComponent<BoxCollider> ().enabled = true;
		}
	}
}
