using UnityEngine;

public class CameraController : MonoBehaviour {

	private float yaw;
	private float pitch = 0.0f;
	public float yawSpeed = 2.0f;
	public float pitchSpeed = 2.0f;

	private void Start()
	{
		yaw = Vector3.Angle(transform.right, Vector3.right);
	}

	void Update()
	{
		yaw += yawSpeed * Input.GetAxis("Mouse X");
		pitch -= pitchSpeed * Input.GetAxis("Mouse Y");
		if (pitch < -90) pitch = -90;
		if (pitch > 90) pitch = 90;

		transform.localEulerAngles = new Vector3(pitch, yaw, 0.0f);

	}

}