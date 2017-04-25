using UnityEngine;

public class GravityController : MonoBehaviour {

	private Rigidbody rb;
	private Vector3 gravityDir = new Vector3(0.0f, -1.0f, 0.0f);
	public float gravity = 9.81f;

	// Use this for initialization
	void Start()
	{
		rb = GetComponent<Rigidbody>();
	}

	// Update is called once per frame
	void Update()
	{

	}

	public void changeDir(Vector3 newDir)
	{
		gravityDir = newDir.normalized;
	} 

	void FixedUpdate()
	{
		rb.AddForce(gravity * gravityDir.normalized * rb.mass);                                                                                    // Gravity
	}
}
