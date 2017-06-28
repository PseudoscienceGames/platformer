using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterController))]
public class Player1 : MonoBehaviour
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

	float timeSinceGrounded = 0;
	float timeToWallUnstick;
	public float gravity;
	float maxJumpVelocity;
	float minJumpVelocity;
	Vector3 smoothdamp;
	public Vector3 velocity;
	public Vector3 normal;
	CharacterController pc;

	//public bool wasGrounded = true;
	PlayerEffects pe;
	bool slide;

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
		if(normal.y < .866f)
			pc.Move(new Vector3(normal.x, -(1 - normal.y), normal.z) * 0.1f);
		if (pc.isGrounded)
		{
			timeSinceGrounded = 0;
			velocity.y = -0.1f;
		}
		else
		{
			timeSinceGrounded += Time.deltaTime;
			//wasGrounded = false;
		}
		Vector3 input = Camera.main.transform.root.TransformDirection(new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")) * moveSpeed);
		if (timeToWallUnstick > 0)
			timeToWallUnstick -= Time.deltaTime;
		if (Input.GetButtonDown("Jump"))
		{
			Jump(input);
		}
		if (Input.GetButtonUp("Jump") && velocity.y > minJumpVelocity)
			velocity.y = minJumpVelocity;
		input.y = velocity.y;
		//if (pc.isGrounded && !wasGrounded)
		//{
		//	wasGrounded = true;
		//	pe.Land();
		//}
		velocity = Vector3.SmoothDamp(velocity, input, ref smoothdamp, smooth);
		velocity.y += gravity * Time.deltaTime;
		transform.LookAt(new Vector3(transform.position.x + velocity.x, transform.position.y, transform.position.z + velocity.z));
		bool wasGrounded = false;
		if (pc.isGrounded)
			wasGrounded = true;
		pc.Move(velocity * Time.deltaTime);
		if (!pc.isGrounded && wasGrounded && !Input.GetButton("Jump"))
		{
			RaycastHit hit;
			if (Physics.Raycast(transform.position, -Vector3.up, out hit, 1))
			{
				pc.Move(Vector3.up * -hit.distance);
			}
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
		Debug.Log(normal.y);
		if (!pc.isGrounded && hit.normal.y < 0.866f && hit.normal.y > -0.5f)
		{
			slide = true;
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
