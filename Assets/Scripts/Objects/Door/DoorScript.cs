using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour, ITriggerEvent {
	private List<GameObject> objs = new List<GameObject> ();

	// Use this for initialization
	void Start () {
		objs.Add (transform.Find ("Laser_1").gameObject);
		objs.Add (transform.Find ("Laser_2").gameObject);
		objs.Add (transform.Find ("Laser_3").gameObject);
		objs.Add (transform.Find ("Laser_4").gameObject);
		objs.Add (transform.Find ("Collider").gameObject);
	}

	public void trigger(){
		foreach(GameObject obj in objs){
			obj.SetActive(false);
		}
	}
}
