using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGrab : MonoBehaviour {
	//private SpringJoint spring;
	private Rigidbody grabbed;
	private Vector3 grabPoint;
	private Vector3 grabPointCam;
	public GameObject crosshair;
	public float strengthFactor = 1;
	public float maxStrength = 100;
	public float maxDistance = 3;

	void Update() {
		if(Input.GetMouseButtonDown(0)) {
			RaycastHit hit;
			if(Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit)) {
				//spring = gameObject.AddComponent<SpringJoint>();
				if(hit.rigidbody is not null && hit.distance < maxDistance) {
					grabbed = hit.rigidbody;
					grabPoint = grabbed.transform.InverseTransformPoint(hit.point);
					grabPointCam = Camera.main.transform.InverseTransformPoint(hit.point);
				}
				if(hit.collider.gameObject.GetComponent<Clickable>() is not null) {
					hit.collider.gameObject.GetComponent<Clickable>().Click();
				}
			}
			crosshair.SetActive(grabbed is null);
		}

		if(Input.GetMouseButtonUp(0)) {
			//Destroy(spring);
			grabbed = null;
			crosshair.SetActive(false);
		}
	}

	void FixedUpdate() {
		if(grabbed is null)
			return;

		Vector3 worldPoint = grabbed.transform.TransformPoint(grabPoint);
		Vector3 targetPoint = Camera.main.transform.TransformPoint(grabPointCam);
		Vector3 dir = targetPoint - worldPoint;
		float strength = Mathf.Clamp(strengthFactor * grabbed.mass, 0, maxStrength);
		//dir.Normalize();
		grabbed.AddForceAtPosition(strength * dir, worldPoint);
		//grabbed.AddForce(strength * dir);
		//Debug.DrawLine(worldPoint, targetPoint);
	}
}
