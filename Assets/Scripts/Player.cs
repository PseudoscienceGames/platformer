using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{
	public float moveSpeed = 10;
	public float maxJumpHeight = 5;
	public float minJumpHeight = 0.5f;
	public float timeToJumpApex = 0.4f;
	public float smooth = 0.1f;
	public Vector3 wjForce;
	public float wallSpeed = 8;
	public float wallStickTime;
	public float jumpLeeway;
	public float runMultiplier;

	public int pickups;
	public bool G;

	float timeSinceGrounded = 0;
	float timeToWallUnstick;
	public float gravity;
	float maxJumpVelocity;
	float minJumpVelocity;
	Vector3 smoothdamp;
	public Vector3 velocity;
	Vector3 normal;
	CharacterController pc;
	public bool isAlive = false;

	public bool wasGrounded = true;
	public Text pickupCount;
	PlayerEffects pe;

	void Start()
	{
		pc = GetComponent<CharacterController>();
		pe = GetComponent<PlayerEffects>();
		gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
		maxJumpVelocity = Mathf.Abs(gravity * timeToJumpApex);
		minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight);
	}

	void Update()
	{
		if (isAlive)
		{
			if (pc.isGrounded)
			{
				timeSinceGrounded = 0;
				velocity.y = -0.1f;
			}
			else
			{
				timeSinceGrounded += Time.deltaTime;
				wasGrounded = false;
			}
			Vector3 input = Camera.main.transform.root.TransformDirection(new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")) * moveSpeed);
			if (Input.GetButton("Run"))
				input *= runMultiplier;
			Vector3 target = input;
			if (timeToWallUnstick > 0)
				timeToWallUnstick -= Time.deltaTime;
			if (Input.GetButtonDown("Jump"))
			{
				Jump(input);
			}
			if (Input.GetButtonUp("Jump") && velocity.y > minJumpVelocity)
				velocity.y = minJumpVelocity;
			target.y = velocity.y;
			if(pc.isGrounded && !wasGrounded)
			{
				wasGrounded = true;
				pe.Land();
			}
			velocity = Vector3.SmoothDamp(velocity, target, ref smoothdamp, smooth);
			velocity.y += gravity * Time.deltaTime;
			transform.LookAt(new Vector3(transform.position.x + velocity.x, transform.position.y, transform.position.z + velocity.z));
			pc.Move(velocity * Time.deltaTime);
		}
	}

	void Jump(Vector3 input)
	{
		//Regular Jump
		if (pc.isGrounded || timeSinceGrounded <= jumpLeeway)
		{
			velocity.y = maxJumpVelocity;
			timeSinceGrounded = jumpLeeway + 1;
		}
		//Wall Jump
		else if (timeToWallUnstick > 0)
		{
			timeToWallUnstick = 0;
			velocity = normal * wjForce.z; ;
			velocity.y = wjForce.y;
		}
	}

	private void OnControllerColliderHit(ControllerColliderHit hit)
	{
		normal = hit.normal;
		if (!pc.isGrounded && hit.normal.y < 0.5f && hit.normal.y > -0.5f)
		{
			pe.ps.Emit(1);
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
