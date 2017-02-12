using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{
	public float moveSpeed = 6;
	public float maxJumpHeight = 4;
	public float minJumpHeight = 1;
	public float timeToJumpApex = 0.4f;
	public float groundSmooth = 0.1f;
	public float airSmooth = 0.2f;
	public float wjSmooth = 0.1f;
	public Vector3 wjOff;
	public Vector3 wjClimb;
	public Vector3 wjLeap;
	public float wallSpeed = 3;
	public float wallStickTime;
	public float runMultiplier;

	float timeToWallUnstick;
	float gravity;
	float maxJumpVelocity;
	float minJumpVelocity;
	Vector3 smoothdamp;
	public Vector3 velocity;
	Vector3 normal;
	CharacterController pc;

	void Start()
	{
		pc = GetComponent<CharacterController>();
		//gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
		//maxJumpVelocity = Mathf.Abs(gravity * timeToJumpApex);
		//minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight);
	}

	void Update()
	{
		//===================================================================
		gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
		maxJumpVelocity = Mathf.Abs(gravity * timeToJumpApex);
		minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight);
		//===================================================================
		if (pc.isGrounded)
			velocity.y = -0.1f;
		Vector3 input = Camera.main.transform.root.TransformDirection(new Vector3(Input.GetAxisRaw("Horizontal") * moveSpeed, 0, Input.GetAxisRaw("Vertical") * moveSpeed));
		if (Input.GetButton("Run"))
			input *= runMultiplier;
		Vector3 target = input;
		if (timeToWallUnstick > 0)
			timeToWallUnstick -= Time.deltaTime;
		if (Input.GetButtonDown("Jump"))
		{
			if (pc.isGrounded)
				velocity.y = maxJumpVelocity;
			else if (timeToWallUnstick > 0)
			{
				timeToWallUnstick = 0;
				if (input.x == 0 && input.z == 0)//no movement
				{
					velocity = normal * wjOff.z;
					velocity.y = wjOff.y;
					Debug.Log("OFF");
				}
				else if (Vector3.Angle(normal, input) < 90)//Backward movement
				{
					velocity = normal * wjLeap.z;
					velocity.y = wjLeap.y;
					Debug.Log("LEAP");
				}
				else//forward movement
				{
					velocity = (normal + input) / 2f * wjClimb.z;
					velocity.y = wjClimb.y;
					Debug.Log("CLIMB");
				}
			}
		}
		if(Input.GetButtonUp("Jump") && velocity.y > minJumpVelocity)
				velocity.y = minJumpVelocity;
		
		target.y = velocity.y;
		float smoothSpeed;
		if (pc.isGrounded)
			smoothSpeed = groundSmooth;
		else
			smoothSpeed = airSmooth;
		velocity = Vector3.SmoothDamp(velocity, target, ref smoothdamp, smoothSpeed);
		velocity.y += gravity * Time.deltaTime;
		transform.LookAt(new Vector3(transform.position.x + velocity.x, transform.position.y, transform.position.z + velocity.z));
		pc.Move(velocity * Time.deltaTime);
	}

	private void OnControllerColliderHit(ControllerColliderHit hit)
	{
		normal = hit.normal;
		if (!pc.isGrounded && hit.normal.y < 0.1f && hit.normal.y > -0.5f)
		{
			timeToWallUnstick = wallStickTime;
			if (velocity.y < -wallSpeed)
				velocity.y = -wallSpeed;
		}
		if (hit.normal.y < -0.5f)
		{
			velocity.y = gravity * Time.deltaTime;
		}
	}
}
