using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{
	public float moveSpeed = 10;
	public float slideSpeed;
	public float maxJumpHeight = 5;
	public float minJumpHeight = 0.5f;
	public float timeToJumpApex = 0.4f;
	public float smooth = 0.1f;
	public float airSmooth = 0.2f;
	public Vector3 wjUp;
	public Vector3 wjOut;
	public float wallSpeed = 8;
	public float wallStickTime;
	public float jumpLeeway;

	float timeSinceGrounded = 0;
	public float timeToWallUnstick;
	public float gravity;
	public float maxJumpVelocity;
	float minJumpVelocity;
	Vector3 smoothdamp;
	public Vector3 velocity;
	public Vector3 normal;
	CharacterController pc;
	Vector3 input;
	public bool isGrounded;
	public bool wasGrounded = true;
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
		input = Camera.main.transform.root.TransformDirection(new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")) * moveSpeed);
		isGrounded = GroundCheck();
		if (Input.GetButtonDown("Jump"))
		{
			Jump();
		}
		if (!isGrounded)
			velocity.y += gravity * Time.deltaTime;
		if (Input.GetButtonUp("Jump") && velocity.y > minJumpVelocity)
			velocity.y = minJumpVelocity;
		input.y = velocity.y;
		transform.LookAt(new Vector3(transform.position.x + velocity.x, transform.position.y, transform.position.z + velocity.z));
		if (pc.isGrounded)
			wasGrounded = true;
		else
			wasGrounded = false;
		float smoothMod = smooth;
		if (!isGrounded)
			smoothMod = airSmooth;
		if (!Input.GetButton("Jump") && Vector3.Angle(normal, Vector3.up) > pc.slopeLimit && isGrounded)
		{
			Vector3 cross = Vector3.Cross(normal, Vector3.up).normalized;
			Vector3 flatInput = new Vector3(input.x, 0, input.z);
			if (Vector3.Angle(cross, input) > 90)
				cross *= -1;
			Vector3 tempX = cross * (flatInput.magnitude * Mathf.Cos(Vector3.Angle(flatInput, cross) * Mathf.Deg2Rad));
			input = tempX + Quaternion.AngleAxis(-90, Vector3.Cross(normal, Vector3.up)) * normal * moveSpeed;
			Debug.DrawLine(transform.position, transform.position + input, Color.red, 100);
		}
		velocity = Vector3.SmoothDamp(velocity, input, ref smoothdamp, smoothMod);
		pc.Move(velocity * Time.deltaTime);
		if (timeToWallUnstick > 0)
			timeToWallUnstick -= Time.deltaTime;
		else
			timeToWallUnstick = 0;

		if (pc.isGrounded && !wasGrounded)
		{
			pe.Land();
		}
	}
	bool GroundCheck()
	{
		if (pc.isGrounded)
		{
			timeSinceGrounded = 0;
			velocity.y = -0.1f;
			timeToWallUnstick = 0;
			return true;
		}
		RaycastHit hit;
		if (Physics.Raycast(transform.position, -Vector3.up, out hit, 1) && !Input.GetButton("Jump") && wasGrounded)
		{
			pc.Move(Vector3.up * -hit.distance);
			timeSinceGrounded = 0;
			velocity.y = -0.1f;
			timeToWallUnstick = 0;
			return true;
		}
		timeSinceGrounded += Time.deltaTime;
		return false;
	}
	void Jump()
	{
		if (Mathf.Round(normal.y) >= 0)
		{
			if (isGrounded || timeSinceGrounded <= jumpLeeway)
			{
				velocity.y = maxJumpVelocity;
				timeSinceGrounded = jumpLeeway + 1;
			}
			if (timeToWallUnstick > 0)
			{
				Vector3 flatNormal = normal;
				flatNormal.y = 0;
				flatNormal.Normalize();
				timeToWallUnstick = 0;
				if (Vector3.Angle(normal, input) < 90)
				{
					velocity = flatNormal * wjOut.z;
					velocity.y = wjOut.y;
				}
				else
				{
					velocity = flatNormal * wjUp.z;
					velocity.y = wjUp.y;
				}
			}
		}
	}
	private void OnControllerColliderHit(ControllerColliderHit hit)
	{
		normal = hit.normal;
		if (Vector3.Angle(normal, Vector3.up) > pc.slopeLimit)
		{
			pe.ps.Emit(1);
			timeToWallUnstick = wallStickTime;
			if (velocity.y < -wallSpeed)
				velocity.y = -wallSpeed;
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "MovingObj")
			transform.parent = other.transform.parent;
		if (other.tag == "Pickup")
			pe.Pickup(other.gameObject);
	}
	private void OnTriggerExit(Collider other)
	{
		transform.localScale = Vector3.one;
		if (other.tag == "MovingObj")
			transform.parent = null;
	}
}