using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour {

    private CameraController cameraController;


    void Start()
    {
        cameraController = this.gameObject.GetComponentInChildren<CameraController>();
    }

    void Update () {
		if (Input.GetButtonDown("Cancel"))
        {
            this.toggleMenu();
        }
	}

    private void toggleMenu()
    {
        print(Cursor.lockState.ToString());
        if (Cursor.lockState == CursorLockMode.Locked)
        {
            Cursor.lockState = CursorLockMode.None;
        }
        else Cursor.lockState = CursorLockMode.Locked;

        cameraController.toggleMovement();
    }
}
