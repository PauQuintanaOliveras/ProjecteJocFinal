using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
	private Vector3 dir;
	//private float fov;
	public new Camera camera;
	public float walkVel = 1;
	public float sensitivity = 1;
	public float zoomFov = 30;

	void Start() {
		//fov = camera.fieldOfView;
		Cursor.lockState = CursorLockMode.Locked;
	}

	void Update() {
		GetComponent<Rigidbody>().MoveRotation(GetComponent<Rigidbody>().rotation * Quaternion.Euler(0, sensitivity * Input.GetAxis("Mouse X"), 0));
		camera.transform.Rotate(new Vector3(-1, 0, 0), sensitivity * Input.GetAxis("Mouse Y"));
		dir = transform.forward * Input.GetAxisRaw("Vertical") + transform.right * Input.GetAxisRaw("Horizontal");
	}

	void FixedUpdate() {
		dir.Normalize();
		GetComponent<Rigidbody>().AddForce(walkVel * dir - GetComponent<Rigidbody>().velocity, ForceMode.VelocityChange);
	}

	void LateUpdate() {
		if(Input.GetKey(KeyCode.Escape))
			Cursor.lockState = CursorLockMode.None;
		else if(Input.GetMouseButtonUp(0))
			Cursor.lockState = CursorLockMode.Locked;

		if(Input.GetMouseButtonDown(2))
			camera.GetComponent<Animator>().Play("Zoom In");
			//camera.fieldOfView = zoomFov;
		else if(Input.GetMouseButtonUp(2))
			camera.GetComponent<Animator>().Play("Zoom Out");
			//camera.fieldOfView = fov;
	}
}
