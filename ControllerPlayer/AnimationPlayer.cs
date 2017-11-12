using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
public class AnimationPlayer : MonoBehaviour {

	[System.Serializable]
	public struct strAnimation {
		public string nameVar;
		public int value;
	};
	
	public strAnimation StateAnim;
	public strAnimation MoveAnim;
	public strAnimation JumpAnim;
	public strAnimation RotateAnim;

	public float RotationSpeed = 1.0f;

	private Vector3 _direction;
	private Camera m_Camera;
	private ControllerPlayer m_controller_player;
	private Animator m_animator;

	// Use this for initialization
	void Start () {
		m_Camera = Camera.main;
		m_controller_player = GetComponentInParent <ControllerPlayer> ();
		m_animator = GetComponent <Animator> ();
	}

	void FixedUpdate () {
		
		float horizontal = PlatformInputManager.horizontalInput;
		float vertical = PlatformInputManager.verticalInput;

		if (PlatformInputManager.rotateInput && !(horizontal == 0 && vertical == 0)) {

			if (!m_controller_player.isJump) {
				m_controller_player.changeSpeedToRun ();
				StartCoroutine(waitAndFixedSpeedMove(0.8F));
			}

			Rotate ();

		} else {

			if (!m_controller_player.isJump) {
				if (horizontal == 0 && vertical == 0) {
					State ();
				} else {
					Move ();
				}
			} else {
				Jump ();
			}
		}
	}
	// Update is called once per frame
	void LateUpdate () {
		rotateDirection ();
	}

	public void rotateDirection(){

		float step = RotationSpeed * Time.deltaTime;
		Vector3 newDir = Vector3.RotateTowards(transform.forward,
		                         new Vector3 (m_controller_player.rotateToDirection.x ,0 ,m_controller_player.rotateToDirection.z
		             					), step,0.0f);

		transform.rotation = Quaternion.LookRotation(newDir);

	}

	public void State (){
		m_animator.SetInteger (StateAnim.nameVar ,StateAnim.value);
	}

	public void Move (){
		m_animator.SetInteger (MoveAnim.nameVar ,MoveAnim.value);
	}

	public void Jump (){
		m_animator.SetInteger (JumpAnim.nameVar ,JumpAnim.value);
	}

	public void Rotate (){
		m_animator.SetInteger (RotateAnim.nameVar ,RotateAnim.value);
	}

	IEnumerator waitAndFixedSpeedMove (float wailTime){

		yield return new WaitForSeconds (wailTime);
		m_controller_player.changeSpeedToWalk ();
	}
}
