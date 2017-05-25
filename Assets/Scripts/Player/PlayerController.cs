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

	// Variable for the switchgun
	private SwitchGun switchGun;

	// Variables for the crosshair
	public Texture2D crosshairTexture;
	public float crosshairScale = 0.3f;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody> ();
		cameraTransform = Camera.main.transform;
		switchGun = GetComponent<SwitchGun> ();
	}
	
	// Update is called once per frame
	void Update () {
		transform.rotation = Quaternion.Lerp(transform.rotation, destRot, 0.1f);                                   // do rotation


		// Manage Inputs
		// TODO: Make a manager
		if (Input.GetButtonDown ("Activate")) {
			if (holdsItem == false) {
				activate ();
			} else {
				DropObject ();
			}
		}
		if (Input.GetButtonDown ("Fire1")) {
			if (holdsItem) {
				ThrowObject ();
			} else {
				switchGun.ShootGun ("left");
			}
		}
		if (Input.GetButtonDown ("Fire2")) {
			switchGun.ShootGun ("right");
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

	void activate(){
		RaycastHit hit;

		if (Physics.Raycast (cameraTransform.position, cameraTransform.forward, out hit, maxLiftDistance)) {
			GameObject objToCheck = hit.transform.gameObject;
            if (objToCheck.tag == "liftable")                                                                       // pick up object if liftable
            {
                hit.transform.rotation = cameraTransform.rotation;
                hit.transform.parent = cameraTransform;
                hit.transform.gameObject.GetComponent<GravityController>().enabled = false;
                item = objToCheck;
                item.transform.position = cameraTransform.position + cameraTransform.forward * holdingDistance;
                item.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
                holdsItem = true;
            }
            else if (objToCheck.tag == "activateable")                                                              // activate objects events if its "activateable"
            {
                objToCheck.GetComponent<IActivateable>().activate();
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

	void grip(float factor)
	{
		Vector3 veldiff = -1f * (rb.velocity * factor);
		if (veldiff.magnitude < 5) veldiff = veldiff.normalized * 5;                                                // minimum slow-down
		rb.AddForce(veldiff);
	}

	public void changeGravityDir(Vector3 _gravityDir)                                                               // rotate player upwards and change gravityDir
	{
		if (gravityDir != _gravityDir)
		{
			Vector3 r;
			if (gravityDir == -_gravityDir)                                                                         // when new gravity ist is 180° to current -> exceptipn handling since cross wont work to find orthogonal
			{
				r = transform.forward;                                                                              // using forward vector as orthonogal
			}
			else r = Vector3.Cross(transform.up, -_gravityDir);                                                     // get orthogonal of current and dest up vector
			r.Normalize();
			float alpha = Vector3.Angle(-_gravityDir, transform.up);                                                // get Angle that needs to be covered

			Quaternion quat = Quaternion.AngleAxis(alpha, r);
			destRot = quat * transform.rotation;                                                                    // calc rotation

			gravityDir = _gravityDir;                                                                               // save new Gravity (for jump)
		}

        
	}

	bool IsGrounded() {
        Ray r = new Ray(transform.position, -transform.up);
        CapsuleCollider capCol = this.GetComponent<CapsuleCollider>();
        return Physics.SphereCast(r, capCol.radius, capCol.height/2);
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