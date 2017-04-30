using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlattformController : MonoBehaviour {
	public float maxScale = 3.0f;
	public float speed = 0.1f;
	public bool triggered = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (transform.localScale.z <= maxScale && triggered) {
			transform.localScale = Vector3.MoveTowards (transform.localScale, new Vector3 (transform.localScale.x, transform.localScale.y, maxScale), speed * Time.deltaTime);
		} else if (transform.localScale.z >= 1) {
			transform.localScale = Vector3.MoveTowards (new Vector3 (transform.localScale.x, transform.localScale.y, maxScale), transform.localScale,  speed * Time.deltaTime);
		}
	}
}
