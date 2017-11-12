using UnityEngine;
using System.Collections;

public class PlatformInputManager : MonoBehaviour {
	public static float verticalInput = 0.0f;
	public static float horizontalInput = 0.0f;
	public static bool jumpInput = false;
	public static bool rotateInput = false;

	// Use this for initialization
	void Awake () {
		horizontalInput = 0.0f;
		verticalInput = 0.0f;
		jumpInput = false;
		rotateInput = false;
	}
	
	// Update is called once per frame
	void Update () {

		jumpInput = Input.GetButtonDown ("Jump");
		rotateInput = Input.GetButtonDown ("Rotate");
		horizontalInput = Input.GetAxis ("Horizontal");
		verticalInput = Input.GetAxis ("Vertical");

	}
}
