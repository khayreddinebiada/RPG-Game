using UnityEngine;
using System.Collections;

public class RPGCamera : MonoBehaviour {
	public  Transform target;
	public float targetHeight = 1.2f;
	public float distance = 4.0f;
	public float maxDistance = 6f;
	public float minDistance = 1.0f;
	public float xSpeed = 250.0f;
	public float ySpeed = 120.0f;
	public float yMinLimit = -10;
	public float yMaxLimit = 70;
	public float zoomRate = 80;
	public float rotationDampening = 3.0f;
	public float x = 20.0f;
	public float y = 0.0f;
	public Quaternion aim;
	private float aimAngle = 8;
	private bool lockOn = false;
	// Use this for initialization
	void Start () {
		if(!target){
			target = GameObject.FindWithTag ("Player").transform;
		}
		Vector3 angles  = transform.eulerAngles;
		x = angles.y;
		y = angles.x;
		
		if (GetComponent<Rigidbody>())
			GetComponent <Rigidbody>().freezeRotation = true;
		Screen.lockCursor = true;
	}
	
	// Update is called once per frame
	void LateUpdate () {
		
		if(!target)
			return;

		if (Time.timeScale == 0.0){
			return;
		}
		
		x += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
		y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;
		
		distance -= (Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime) * zoomRate * Mathf.Abs(distance);
		distance = Mathf.Clamp(distance, minDistance, maxDistance);
		
		y = ClampAngle(y, yMinLimit, yMaxLimit);
		
		// Rotate Camera
		Quaternion rotation = Quaternion.Euler(y, x, 0);
		transform.rotation = rotation;
		aim = Quaternion.Euler(y- aimAngle, x, 0);
		
		//Rotate Target
		/*
		if(Input.GetButton("Fire1") || Input.GetButton("Fire2") || Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0 || lockOn){
			targetBody.transform.rotation = Quaternion.Euler(0, x, 0);
		}
		*/

		//Camera Position
		Vector3 position = target.position - (rotation * Vector3.forward * distance + new Vector3(0,-targetHeight,0));
		transform.position = position;
		
		RaycastHit hit;
		Vector3 trueTargetPosition = target.transform.position - new Vector3(0,-targetHeight,0);
		if (Physics.Linecast (trueTargetPosition, transform.position, out hit)) {
			if (hit.transform.tag == "Wall") {
				float tempDistance = Vector3.Distance (trueTargetPosition, hit.point) - 0.28f;
				
				position = target.position - (rotation * Vector3.forward * tempDistance + new Vector3 (0, -targetHeight, 0));
				transform.position = position;
			}
			
		}
	}

	static float ClampAngle (float angle ,float min, float max) {
		if (angle < -360)
			angle += 360;
		if (angle > 360)
			angle -= 360;
		return Mathf.Clamp (angle, min, max);
		
	}
}
