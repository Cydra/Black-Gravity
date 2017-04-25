using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	public Vector3 gravityDir = new Vector3(0.0f,-1.0f, 0.0f);
	private Rigidbody rb;

	// Variables to move the player
	GameObject ground;
	public float gripF = 0.5f;
	public float jumpStrength = 50;
	public float speed = 50.0f;
	private Vector3 input = new Vector3(0f, 0f, 0f);
	private short grounded = 0;
	Transform cameraTransform;

	// Variables for lifting and dropping dynamic items
	private GameObject item;
	private bool holdsItem = false;
	public float maxLiftDistance = 10.0f;
	public float holdingDistance = 2.0f;
	public float throwStrength   = 5.0f;


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
		Debug.DrawRay (cameraTransform.position, cameraTransform.forward * 10, Color.green);

		movePlayer ();

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
		if (holdsItem) {
			Rigidbody itemRB = item.GetComponent<Rigidbody> ();

			// If you look down and then up again, fix the position of the cube
			if (cameraTransform.localEulerAngles.x >= 10 && cameraTransform.localEulerAngles.x <= 90) { 
				item.transform.position = new Vector3(item.transform.position.x, transform.position.y - 0.2f , item.transform.position.z);
			}

			// Dont let the cube drift away while holding it
			itemRB.velocity = new Vector3 (0, 0, 0);
		}
	}

	void movePlayer(){
		input = new Vector3(Input.GetAxis("Vertical"), 0, -Input.GetAxis("Horizontal"));

		input = Quaternion.Euler(0, -90, 0) * input;                                                                // Angle Movement 90° to the right
		input = cameraTransform.TransformDirection(input);                                                          // Trnsform local coords to world coords
		input = input.normalized * speed;                                                                           // make sure movement has always the same speed

		if (rb.velocity.magnitude - input.magnitude <= 0)
		{
			rb.velocity = new Vector3(0, 0, 0);                                                                     // TODO: bad solution for stop walking
		}

		rb.AddForce(input);                                                                                         // actual movement

		if(grounded > 0) {
			grip(gripF);                                                                                            // slow slide
			Vector3 jump = -gravityDir * Input.GetAxis("Jump") * jumpStrength;                                      // calc Jump
			rb.AddForce(jump, ForceMode.Impulse);                                                                   // jump
		}
	}

	void LiftObject(){
		RaycastHit hit;

		if (Physics.Raycast (cameraTransform.position, cameraTransform.forward, out hit, maxLiftDistance)) {
			Debug.Log("Hit obj");
			GameObject objToCheck = hit.transform.gameObject;
			if (objToCheck.tag == "liftable") {
				
				Debug.Log("Lifting obj");
				hit.transform.rotation = cameraTransform.rotation;
				hit.transform.parent = cameraTransform;
				hit.transform.gameObject.GetComponent<GravityController> ().enabled = false;
				item = objToCheck;
				Debug.Log (cameraTransform.forward);
				item.transform.position = cameraTransform.position + cameraTransform.forward * holdingDistance;
				item.GetComponent<Rigidbody> ().freezeRotation = true;
				holdsItem = true;
			}
		}
	}

	void DropObject(){
		item.transform.parent = null;
		item.GetComponent<Rigidbody> ().freezeRotation = false;
		item.GetComponent<GravityController> ().enabled = true;
		holdsItem = false;
		item = null;
	}

	void ThrowObject(){
		item.transform.parent = null;
		item.GetComponent<Rigidbody> ().freezeRotation = false;
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

	public void changeGravityDir(Vector3 _gravityDir)                                                               // change direction of player
	{
		gravityDir = _gravityDir;                                                                                   // save new Gravity (for jump)
		transform.rotation = Quaternion.LookRotation(transform.forward, -_gravityDir);                              // rotate body
	}

	void OnCollisionEnter(Collision other)
	{
		ground = other.gameObject;
		grounded++;                                                                                                 // TODO bad solutuion feor checking if grounded
	}

	void OnCollisionExit(Collision other)
	{
		grounded--;                                                                                                 // TODO bad solutuion feor checking if grounded
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