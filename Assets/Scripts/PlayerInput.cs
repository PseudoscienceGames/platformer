using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
	public float moveSpeed;
	public float runMultiplyer;
	public float maxJumpHeight;
	public float minJumpHeight;
	public float jumpLeeway;

	Vector3 input;
	Vector3 velocity;
	bool isJumping;
	float timeSinceGrounded;
	bool isGrounded;
	CharState myState;
	CharState lastState;
	CharacterController cc;
	PlayerMotor pm;

	private void Start()
	{
		cc = GetComponent<CharacterController>();
		pm = GetComponent<PlayerMotor>();
	}

	private void Update()
	{
		velocity = cc.velocity;
		input = Camera.main.transform.root.TransformDirection(new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")) * moveSpeed);
		if (Input.GetButton("Run"))
			input = input * runMultiplyer;
		isJumping = Input.GetButton("Jump");
		isGrounded = cc.isGrounded;
		if (isGrounded)
			timeSinceGrounded = 0;
		else
			timeSinceGrounded += Time.deltaTime;
		FindState();
		//pm.Move(myState, lastState, input);
	}

	void FindState()
	{
		CharState state = CharState.Idle;
		if (input.magnitude < 0.05f && !isJumping && isGrounded)
			state = CharState.Idle;
		if (input.magnitude >= 0.05f && !isJumping && isGrounded)
			state = CharState.Running;
		if (isJumping && (isGrounded || timeSinceGrounded <= jumpLeeway))
			state = CharState.Jumping;
		if (!isGrounded && velocity.y <= 0)
			state = CharState.Falling;
		lastState = myState;
		myState = state;
	}
}
public enum CharState
{
	Idle,
	Running,
	Jumping,
	Falling,
	Sliding,

}