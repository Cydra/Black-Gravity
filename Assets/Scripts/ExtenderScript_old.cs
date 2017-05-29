using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtenderScript_old : MonoBehaviour {
	public float maxExtend;
	public float speed;
	public float jumpheight;

	private Rigidbody rb;
	private float movePos;
	private bool upOrDown = false;
	private bool active;
	private bool liftforce = false;
	private GameObject obj;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody> ();
		active = false;
		movePos = 0;
	}

	void Update(){
		
	}

	void FixedUpdate(){
		if (upOrDown == true && transform.localPosition.y > movePos && active) {
			rb.velocity = Vector3.zero;
			rb.constraints = RigidbodyConstraints.FreezeAll;
			active = false;
			if (obj) {
				obj.GetComponent<Rigidbody> ().AddForce (Vector3.up * jumpheight, ForceMode.Impulse);
			}
			obj = null;
		}

		if (upOrDown == false && transform.localPosition.y < movePos && active) {
			rb.velocity = Vector3.zero;
			rb.constraints = RigidbodyConstraints.FreezeAll;
			active = false;
			obj = null;
		}

		if (liftforce && active && upOrDown) {
			rb.AddForce (Vector3.up * speed);
		}
	}

	public void moveExtender(bool extendOrRetrieve){
		Debug.Log ("triggered");
		rb.constraints = RigidbodyConstraints.None|RigidbodyConstraints.FreezePositionX|RigidbodyConstraints.FreezePositionZ|RigidbodyConstraints.FreezeRotation;
		if (extendOrRetrieve) {
			movePos = maxExtend;
			upOrDown = true;
			rb.AddForce (Vector3.up * speed, ForceMode.Force);
			active = true;
		} else {
			movePos = 0;
			upOrDown = false;
			rb.AddForce (Vector3.up * -speed, ForceMode.Force);
			active = true;
		}
	}

	public void OnCollisionEnter(Collision col){
		GameObject other = col.gameObject;
		if (other.tag == "Player") {
			liftforce = true;
			obj = other;
		}
	}

	public void OnCollisionExit(Collision col){
		GameObject other = col.gameObject;
		if (other.tag == "Player") {
			liftforce = false;
			obj = other;
		}
	}
}
