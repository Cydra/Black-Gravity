using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodenplatteController : MonoBehaviour {
	private bool triggered = false;
	public float upPos = 2.12f;
	public float downPos = 1.05f;
	public float moveSpeed = 0.02f;
    private ITriggerEvent[] triggers;

	// Use this for initialization
	void Start () {
        triggers = GetComponents<ITriggerEvent>();
    }
	
	// Update is called once per frame
	void Update () {
		// Move up
		if(transform.localPosition.y < upPos && triggered == false){
			transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + moveSpeed, transform.localPosition.z);
		}
	}

	void OnCollisionEnter(Collision col){
		triggered = true;
        foreach (ITriggerEvent trigger in triggers)             // activates trigger function of each component that implements ITriggerEvent
        {
            trigger.trigger();
        }
    }

	void OnCollisionExit(Collision col){
		triggered = false;
        foreach (ITriggerEvent trigger in triggers)             // activates trigger function of each component that implements ITriggerEvent
        {
            trigger.trigger();
        }
    }

	void OnCollisionStay(Collision col){
		if(transform.localPosition.y > downPos && (col.transform.tag == "Player" || col.transform.tag == "liftable")){
			transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y - moveSpeed, transform.localPosition.z); 
		}
	}
}