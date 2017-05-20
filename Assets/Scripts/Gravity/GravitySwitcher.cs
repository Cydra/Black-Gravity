using UnityEngine;
using System;

public class GravitySwitcher : MonoBehaviour {

	public GameObject GravitySwitchController;                                                      // Controller needs to be registered

	void Start()
	{
		if (GravitySwitchController == null) Console.Error.WriteLine("GravitySwitchSphere '"        // Controller needs to be registered
			+ this.name + "' needs GravitySwitchController");
	}

	void OnCollisionEnter(Collision col)
	{
		if (col.gameObject.GetComponent<GravityController>() != null)                               // check if colliding Object has a gravity
		{
			GameObject obj = col.gameObject;

			GravitySwitchController.GetComponent<GravitySwitchController>().                        // register new Object in Controller
			register(obj, this.gameObject);

			Vector3 gravDir;

			if (this.gameObject.GetComponent<BoxCollider>() != null)
			{
				Vector3 direction = (obj.transform.position - this.transform.position).normalized;  // Direction from center to colliding object
				Ray MyRay = new Ray(obj.transform.position, -direction);                            // raycast from colliding object to center
				RaycastHit hit;
				Physics.Raycast(MyRay, out hit);                                                    // get Hitpoint of Player

				// convert collision point to local space
				Vector3 localPoint = hit.transform.InverseTransformPoint(hit.point);
				Vector3 localDir = localPoint.normalized;

				// If upDot is positive, object is above the box; if negative, object is below the box.
				// If fwdDot is positive, object is in front of the box; if negative, object is behind the box.
				// If rightDot is positive, object is to the box's right; if negative, object is to its left.
				float upDot = Vector3.Dot(localDir, Vector3.up);
				float fwdDot = Vector3.Dot(localDir, Vector3.forward);
				float rightDot = Vector3.Dot(localDir, Vector3.right);

				// If upPower is the largest, object is mostly above or below the box.
				// If fwdPower is the largest, object is mostly in front of or behind the box.
				// If rightPower is the largest, object is mostly to the left or right of the box.
				float upPower = Mathf.Abs(upDot);
				float fwdPower = Mathf.Abs(fwdDot);
				float rightPower = Mathf.Abs(rightDot);

				// set Gravity Direction by combining data calculated above
				if(upPower > fwdPower && upPower > rightPower)
				{
					if (upDot > 0) gravDir = -transform.up;
					else gravDir = transform.up;
				}
				else if(fwdPower > upPower && fwdPower > rightPower)
				{
					if (fwdDot > 0) gravDir = -transform.forward;
					else gravDir = transform.forward;
				}
				else
				{
					if (rightDot > 0) gravDir = -transform.right;
					else gravDir = transform.right;
				}
			}
			else if (this.gameObject.GetComponent<MeshCollider>() != null)
			{
				gravDir = -transform.up;                                                        // up vector for mesh collider
				print(-transform.up);
			}
			else
			{
				Console.Error.WriteLine("Bad Collider! Please use Box Collider "                // error if other colliders used
					+ "or Mesh Collider for this Script");
				gravDir = new Vector3(0f,0f,0f);
			}

			if (obj.tag == "Player")
				obj.GetComponent<PlayerController>().changeGravityDir(gravDir);                 // Change Dir of body if player
			obj.GetComponent<GravityController>().changeDir(gravDir);                           // change gravity direction
		}
	}
}