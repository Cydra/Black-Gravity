using UnityEngine;

public class GravityController : MonoBehaviour {

	private Rigidbody rb;
	private Vector3 gravityDir;
	public float gravity = 9.81f;

	// Use this for initialization
	void Start()
	{
        gravityDir = -this.transform.up;
		rb = GetComponent<Rigidbody>();
	}

	// Update is called once per frame
	void Update()
	{

	}

    void FixedUpdate()
    {
        rb.AddForce(gravity * gravityDir.normalized * rb.mass);
        // Test
        RaycastHit hit;
        if (Physics.Raycast(transform.position, gravityDir, out hit, 5.0f))
        {
            if (hit.collider.gameObject.GetComponent<GravitySwitcher>() != null)
            {
                Vector3 newGrav = hit.normal;
                if (this.tag == "Player")
                {
                    GetComponent<PlayerController>().changeGravityDir(-newGrav);
                }
                changeDir(-newGrav);
            }
        }
    }

    public void changeDir(Vector3 newDir)
    {
        gravityDir = newDir.normalized;
    }

}
