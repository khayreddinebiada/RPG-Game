using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class ControllerPlayer : MonoBehaviour {
	public Vector3 directionPlayer;
	public Vector3 rotateToDirection;
	public float jumpSpeed = 1;
	public float gravity = 0.5f;
	public float walkSpeed = 6.0f;
	public float runSpeed = 12.0f;
	public bool isGrounded = false;
	public bool isJump = false;
	public bool isRotate = false;

	private float currentSpeed;
	private Transform cameraMain;
	private float failSpeed;
	private Rigidbody m_rigid;
	private AnimationPlayer m_Animation;
	private Vector3 lestDirectionPlayer;
	private CharacterController m_Controller;
	// Use this for initialization
	void Awake () {
		currentSpeed = walkSpeed;
		cameraMain = Camera.main.transform;
		m_Controller = GetComponent<CharacterController> ();

		if (GetComponentInChildren <AnimationPlayer> ())
			m_Animation = GetComponentInChildren <AnimationPlayer> ();
	}

	void Start (){
		lestDirectionPlayer = Vector3.zero;
	}
	// Update is called once per frame
	void Update () {
		Fail ();
		Jump ();
		Move ();
	}

	void Fail (){
		if (!m_Controller.isGrounded) {
			failSpeed += gravity * Time.deltaTime;
		} else {
			if (failSpeed > 0) failSpeed = 0;
		}
		m_Controller.Move (new Vector3 (0 ,- failSpeed) * Time.deltaTime);
	}

	void Move (){
		Vector3 difX = (transform.position - cameraMain.position) / Vector3.Distance (transform.position, cameraMain.position);
		Vector3 difZ = - new Vector3 (-difX.z,0,difX.x);

		float horizontal = PlatformInputManager.horizontalInput;
		float vertical = PlatformInputManager.verticalInput;

		m_Controller.Move (new Vector3 (difX.x,0 , difX.z) * currentSpeed * vertical * Time.deltaTime);
		m_Controller.Move (new Vector3 (difZ.x,0 , difZ.z) * currentSpeed * horizontal * Time.deltaTime);

		directionPlayer = difX*vertical + difZ*horizontal;

		if (horizontal != 0 || vertical != 0) {
			if (lestDirectionPlayer.x != directionPlayer.x && lestDirectionPlayer.z != directionPlayer.z) {
				rotateToDirection = directionPlayer;
			}
			lestDirectionPlayer = directionPlayer;
		}
	}

	void Jump (){
		isGrounded = m_Controller.isGrounded;

		if (isGrounded)
			isJump = false;

		if (PlatformInputManager.jumpInput && isGrounded) {
			failSpeed = -jumpSpeed;
			isJump = true;
		}
	}

	public void changeSpeedToWalk (){
		currentSpeed = walkSpeed;
	}

	public void changeSpeedToRun (){
		currentSpeed = runSpeed;
	}
}
