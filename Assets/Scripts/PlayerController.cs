using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	public Vector3 gravityDir = new Vector3(0.0f,-1.0f, 0.0f);
	private Rigidbody rb;

	// Variables to move the player
	Transform cameraTransform;
	public float jumpStrength = 5f;
	public float speed = 50.0f;
	private Vector3 input = new Vector3(0f, 0f, 0f);
	private bool jumped = false;
	Quaternion destRot;

	// Variables for lifting and dropping dynamic items
	private GameObject item;
	private bool holdsItem = false;
	public float maxLiftDistance = 10.0f;
	public float holdingDistance = 2.0f;
	public float throwStrength   = 5.0f;
	public float lerpSpeed       = 2.0f;

	// Variables for switchgun
	private GameObject switchObj;
	public float maxShootDistance = 10.0f;

	// Variables for the crosshair
	public Texture2D crosshairTexture;
	public float crosshairScale = 0.3f;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody> ();
		cameraTransform = Camera.main.transform;
	}
	
	// Update is called once per frame
	void Update () {
		transform.rotation = Quaternion.Lerp(transform.rotation, destRot, 0.1f);                                   // do rotation

		// Lift and drop object
		if (Input.GetButtonDown ("Submit")) {
			if (holdsItem == false) {
				LiftObject ();
			} else {
				DropObject ();
			}
		}

		// Throw object
		if (Input.GetButtonDown ("Fire1")) {
			if (holdsItem) {
				ThrowObject ();
			} else {
				ShootGun ();
			}
		}
	}

	// Physik Operationen
	void FixedUpdate(){
		movePlayer ();


		// Fix for the cube movement
		if (holdsItem) {
			item.transform.position = Vector3.Lerp (item.transform.position, cameraTransform.position + cameraTransform.forward * holdingDistance, lerpSpeed * Time.deltaTime);

			item.GetComponent<Rigidbody> ().velocity = Vector3.zero;
		}
	}

	void movePlayer()
	{

		input = new Vector3(Input.GetAxis("Vertical"), 0, -Input.GetAxis("Horizontal"));

		input = Quaternion.Euler(0, -90, 0) * input;                                                                // angle Movement 90° to the right
		input = cameraTransform.TransformDirection(input);                                                          // transform local coords to world coords
		input = Vector3.ProjectOnPlane(input, transform.up);                                                        // prevent walking upwards
		input = input.normalized * speed; 


		if (IsGrounded() && jumped == false)                                                                        // check if grounded and didnt jump (prevent multiple jumps at once)
		{

			rb.AddForce(input);                                                                                     // walk
			rb.drag = 10;                                                                                           // slowed down when grounded
			Vector3 jump = -gravityDir * Input.GetAxis("Jump") * jumpStrength;                                      // calc Jump
			rb.AddForce(jump, ForceMode.Impulse);                                                                   // jump
			if (jump.magnitude > 0f) jumped = true;                                                                 // check if jumped this frame and set jumped
		} else rb.drag = 0;                                                                                         // no slow when airborne

	}

	void LiftObject(){
		RaycastHit hit;

		if (Physics.Raycast (cameraTransform.position, cameraTransform.forward, out hit, maxLiftDistance)) {
			GameObject objToCheck = hit.transform.gameObject;
			if (objToCheck.tag == "liftable") {
				hit.transform.rotation = cameraTransform.rotation;
				hit.transform.parent = cameraTransform;
				hit.transform.gameObject.GetComponent<GravityController> ().enabled = false;
				item = objToCheck;
				item.transform.position = cameraTransform.position + cameraTransform.forward * holdingDistance;
				item.GetComponent<Rigidbody> ().constraints = RigidbodyConstraints.FreezeRotation;
				holdsItem = true;
			}
		}
	}

	void DropObject(){
		item.transform.parent = null;
		item.GetComponent<Rigidbody> ().constraints = RigidbodyConstraints.None;
		item.GetComponent<GravityController> ().enabled = true;
		holdsItem = false;
		item = null;
	}

	void ThrowObject(){
		item.transform.parent = null;
		item.GetComponent<Rigidbody> ().constraints = RigidbodyConstraints.None;
		item.GetComponent<Rigidbody> ().AddForce (cameraTransform.forward * throwStrength, ForceMode.Impulse);
		item.GetComponent<GravityController> ().enabled = true;
		holdsItem = false;
		item = null;
	}

	void ShootGun(){
		RaycastHit hit;
		if (Physics.Raycast (cameraTransform.position, cameraTransform.forward, out hit, maxShootDistance)) {
			GameObject obj = hit.transform.gameObject;

			if (switchObj == null) {
				Debug.Log ("Set obj 1 for switch" + obj);
				switchObj = obj;
			} else if(switchObj != null && switchObj != obj){
				Debug.Log ("Set obj 2 for switch" + obj);
				// Change grav of objs

				// Obj1 is dynamic and obj2 is static
				if (switchObj.tag == "liftable" && obj.tag != "liftable") {
					switchObj.GetComponent<GravityController> ().changeDir (-obj.transform.up);
				}
				// Obj2 is dynamic and obj1 is static
				else if(obj.tag == "liftable" && switchObj.tag != "liftable"){
					obj.GetComponent<GravityController>().changeDir(-switchObj.transform.up);
				}
				// Both objs are dynamic
				else if(obj.tag == "liftable" && switchObj.tag == "liftable"){
					Vector3 newGravPos = ((obj.transform.position - switchObj.transform.position) * 0.5f) + obj.transform.position;

					Vector3 targetGrav = (newGravPos - switchObj.transform.position).normalized * 9.81f;

					// Turn objects for "floating" effect
					switchObj.transform.LookAt(newGravPos);
					obj.transform.LookAt(newGravPos);

					switchObj.GetComponent<Rigidbody> ().freezeRotation = true;
					obj.GetComponent<Rigidbody> ().freezeRotation = true;

					// TODO: Minor fix to disable the shaking of the cubes

					switchObj.GetComponent<GravityController> ().changeDir (targetGrav);
					obj.GetComponent<GravityController> ().changeDir (-targetGrav);
				}
				// Obj1 is extender and obj2 is static
				else if(switchObj.tag == "extender" && obj.tag != "liftable"){
					Debug.Log(obj.transform.up + " and " + switchObj.transform.up);
					Vector3 temp = switchObj.transform.up + hit.normal;
					Debug.Log (hit.normal);
					if (temp == Vector3.zero) {
						Debug.Log("test");
						switchObj.GetComponent<PlattformController> ().triggered = true;
					} else if (switchObj.transform.up == obj.transform.up) {
						switchObj.GetComponent<PlattformController> ().triggered = false;
					}
				}
				// Obj1 is static and obj2 is extender
				else if(switchObj.tag != "liftable" && obj.tag == "extender"){
					Debug.Log ("test");
					if (obj.transform.up == -switchObj.transform.up) {
						obj.GetComponent<PlattformController> ().triggered = true;
					} else if (obj.transform.up == switchObj.transform.up) {
						obj.GetComponent<PlattformController> ().triggered = false;
					}
				}

				// Reset the switchgun
				switchObj = null;
			}
		}
	}

	void grip(float factor)
	{
		Vector3 veldiff = -1f * (rb.velocity * factor);
		if (veldiff.magnitude < 5) veldiff = veldiff.normalized * 5;                                                // minimum slow-down
		rb.AddForce(veldiff);
	}

	public void changeGravityDir(Vector3 _gravityDir)                                                               // rotate player upwards and change gravityDir
	{
		print("In PC: "+_gravityDir);
		if (gravityDir != _gravityDir)
		{
			Vector3 r;
			if (gravityDir == -_gravityDir)                                                                         // when new gravity ist is 180° to current -> exceptipn handling since cross wont work to find orthogonal
			{
				r = transform.forward;                                                                              // using forward vector as orthonogal
			}
			else r = Vector3.Cross(transform.up, -_gravityDir);                                                     // get orthogonal of current and dest up vector
			r.Normalize();
			float alpha = Vector3.Angle(-_gravityDir, transform.up);                                                // get Ngle that needs to be covered

			Quaternion quat = Quaternion.AngleAxis(alpha, r);
			destRot = quat * transform.rotation; // calc rotation

			gravityDir = _gravityDir;                                                                               // save new Gravity (for jump)
		}
	}

	bool IsGrounded() {
		return Physics.Raycast(transform.position, -transform.up, 1.0f);
	}
		
	void OnCollisionEnter(Collision col)
	{
		jumped = false;                                                                                             // reactivates movement after jumping
	}
		
	void OnGUI(){
		// Check if game is paused
		if (Time.timeScale != 0) {
			if (crosshairTexture != null) {
				GUI.DrawTexture(new Rect((Screen.width-crosshairTexture.width*crosshairScale)/2 ,(Screen.height-crosshairTexture.height*crosshairScale)/2, crosshairTexture.width*crosshairScale, crosshairTexture.height*crosshairScale),crosshairTexture);
			} else {
				Debug.Log ("Please set a texture to the crosshair");
			}
		}
	}
}