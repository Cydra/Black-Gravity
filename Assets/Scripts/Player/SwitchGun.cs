using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchGun : MonoBehaviour {
	private Transform cameraTransform;
	private GameObject objLeft;
	private GameObject objRight;
	private Vector3 objLeftNormal;
	private Vector3 objRightNormal;
	private Vector3 objLeftHitPos;
	private Vector3 objRightHitPos;

	public float maxShootDistance = 10.0f;

	// Use this for initialization
	void Start () {
		cameraTransform = Camera.main.transform;
	}

	public void ShootGun(string whatObj){
		Debug.Log ("ShootGun");

		RaycastHit hit;
		if (Physics.Raycast (cameraTransform.position, cameraTransform.forward, out hit, maxShootDistance)) {
			GameObject obj = hit.transform.gameObject;
			Vector3 normal = hit.normal;
			Vector3 hitPos = hit.point;

			if (whatObj.Equals ("left")) {
				objLeft = obj;
				objLeftNormal = normal;
				objLeftHitPos = hitPos;
			}

			if (whatObj.Equals ("right")) {
				objRight = obj;
				objRightNormal = normal;
				objRightHitPos = hitPos;
			}

			if (objLeft != null && objRight != null && objLeft != objRight) {
				Check_Switch ();
			}
		}
	}

	// Basicly a Switch Case Statement
	void Check_Switch(){
		Debug.Log ("CheckSwitch");

		if (objLeft.tag == "liftable" && objRight.tag == "liftable") {
			Switch_Liftable_Liftable (objLeft, objRight);
			return;
		}
		if (objLeft.tag == "liftable" && (objRight.tag == "environment" || objRight.tag == "extender")) {
			Switch_Liftable_Environment (objLeft, objRight, objRightHitPos);
			return;
		}
		if((objLeft.tag == "environment" || objLeft.tag == "extender") && objRight.tag == "liftable"){
			Switch_Liftable_Environment (objRight, objLeft, objLeftHitPos);
			return;
		}
		if (objLeft.tag == "extender" && objRight.tag == "environment") {
			Switch_Extender_Environment (objLeft, objRight, objLeftNormal, objRightNormal);
			return;
		}
		if (objLeft.tag == "environment" && objRight.tag == "extender") {
			Switch_Extender_Environment (objRight, objLeft, objRightNormal, objLeftNormal);
			return;
		}
	}

	// Helper methods
	void Switch_Liftable_Liftable(GameObject obj1, GameObject obj2){
		Vector3 middlePos = Vector3.Lerp (obj1.transform.position, obj2.transform.position, 0.5f);
		Vector3 targetGrav = (middlePos - obj1.transform.position).normalized * 9.81f;

		obj1.transform.LookAt(middlePos);
		obj2.transform.LookAt(middlePos);

		obj1.GetComponent<Rigidbody> ().freezeRotation = true;
		obj2.GetComponent<Rigidbody> ().freezeRotation = true;

		obj1.GetComponent<GravityController> ().changeDir (targetGrav);
		obj2.GetComponent<GravityController> ().changeDir (-targetGrav);

		RemoveObjs ();
	}

	void Switch_Liftable_Environment(GameObject obj1, GameObject obj2, Vector3 hitPos){
		//obj1.GetComponent<GravityController> ().changeDir (-obj2.transform.up);
		Vector3 middlePos = Vector3.Lerp (obj1.transform.position, hitPos, 0.5f);
		Vector3 targetGrav = (middlePos - obj1.transform.position).normalized * 9.81f;

		obj1.transform.LookAt(middlePos);

		obj1.GetComponent<GravityController> ().changeDir (targetGrav);

		RemoveObjs ();
	}

	void Switch_Extender_Environment(GameObject obj1, GameObject obj2, Vector3 obj1Normal, Vector3 obj2Normal){
		ExtenderScript es = obj1.GetComponent<ExtenderScript> ();

		if (obj1.transform.forward == obj2Normal) {
			es.moveExtender (false);
		}
	
		if (obj1.transform.forward == -obj2Normal) {
			es.moveExtender (true);
		}

		RemoveObjs ();
	}

	void RemoveObjs(){
		objLeft  = null;
		objRight = null;
	}
}
