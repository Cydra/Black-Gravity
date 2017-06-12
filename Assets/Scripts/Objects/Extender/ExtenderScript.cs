using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtenderScript : MonoBehaviour {
	public float speed;
	public float jumpheight;
	public enum PositionAtStart{ Top = 0, Bottom = 1};
	public PositionAtStart positionAtStart;

	private Rigidbody rb;
	private bool upOrDown = false;
	private bool active;
	private bool liftforce = false;
	private GameObject obj;
	private float startPos;

	// Differenz = 1.3 Units

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody> ();
		startPos = transform.localPosition.y;
		active = false;

		// Position the extender at bottom if wanted
		if (positionAtStart == PositionAtStart.Bottom) {
			Debug.Log ("Extender at bottom");
			transform.localPosition = new Vector3(transform.localPosition.x, -startPos + 1.5f, transform.localPosition.z);
		}
	}

	void Update(){
		
	}

	void FixedUpdate(){
		// Move up finished
		if (upOrDown == true && transform.localPosition.y >= startPos && active) {
			transform.localPosition = new Vector3(transform.localPosition.x, startPos, transform.localPosition.z);
			rb.velocity = Vector3.zero;
			rb.constraints = RigidbodyConstraints.FreezeAll;
			active = false;
			if (obj) {
				obj.GetComponent<Rigidbody> ().AddForce (transform.forward * jumpheight, ForceMode.Impulse);
			}
			obj = null;
		}

		// Move down finished
		if (upOrDown == false && transform.localPosition.y <= -startPos + 1.5f && active) {
			transform.localPosition = new Vector3(transform.localPosition.x, -startPos + 1.5f, transform.localPosition.z);
			rb.velocity = Vector3.zero;
			rb.constraints = RigidbodyConstraints.FreezeAll;
			active = false;
			obj = null;
		}

		if (liftforce && active && upOrDown) {
			rb.AddForce (this.transform.forward * speed);
		}
	}

	public void moveExtender(bool extendOrRetrieve){
		rb.constraints = RigidbodyConstraints.None|RigidbodyConstraints.FreezeRotation;
		if (extendOrRetrieve) {
			Debug.Log ("Extending");
			upOrDown = true;
			rb.AddForce (this.transform.forward * speed, ForceMode.Force);
			active = true;
		} else {
			Debug.Log ("Retrieving");
			upOrDown = false;
			rb.AddForce (this.transform.forward * -speed, ForceMode.Force);
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
