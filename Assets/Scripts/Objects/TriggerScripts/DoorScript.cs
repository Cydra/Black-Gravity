using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour, ITriggerEvent {
	public GameObject destination;
	private List<LineRenderer> lasers = new List<LineRenderer> ();
	private Collider collider;
	private bool active = true;

	// Use this for initialization
	void Start () {
		lasers.Add (destination.transform.Find ("Laser_1").gameObject.GetComponent<LineRenderer>());
		lasers.Add (destination.transform.Find ("Laser_2").gameObject.GetComponent<LineRenderer>());
		lasers.Add (destination.transform.Find ("Laser_3").gameObject.GetComponent<LineRenderer>());
		lasers.Add (destination.transform.Find ("Laser_4").gameObject.GetComponent<LineRenderer>());
		collider = destination.GetComponent<BoxCollider> ();
	}

	public void trigger(){
		if (active) {
			foreach (LineRenderer laser in lasers) {
				laser.enabled = false;
			}
			collider.enabled = false;
			active = false;
		} else {
			foreach (LineRenderer laser in lasers) {
				laser.enabled = true;
			}
			collider.enabled = true;
			active = true;
		}
	}
}
