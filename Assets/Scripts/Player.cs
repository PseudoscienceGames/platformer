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
	public float jumpLeeway;

	float timeSinceGrounded = 0;
	float timeToWallUnstick;
	float gravity;
	float maxJumpVelocity;
	float minJumpVelocity;
	Vector3 smoothdamp;
	public Vector3 velocity;
	Vector3 normal;
	CharacterController pc;
	bool isAlive = true;
	public GameObject deadPlayer;

	void Start()
	{
		pc = GetComponent<CharacterController>();
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
				timeSinceGrounded += Time.deltaTime;
			Vector3 input = Camera.main.transform.root.TransformDirection(new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")) * moveSpeed);
			Vector3 target = input;
			if (timeToWallUnstick > 0)
				timeToWallUnstick -= Time.deltaTime;
			if (Input.GetButtonDown("Jump"))
			{
				if (pc.isGrounded || timeSinceGrounded <= jumpLeeway)
				{
					velocity.y = maxJumpVelocity;
					timeSinceGrounded = jumpLeeway + 1;
				}
				else if (timeToWallUnstick > 0)
				{
					timeToWallUnstick = 0;
					if (input.x == 0 && input.z == 0)//no movement
					{
						velocity = normal * wjOff.z;
						velocity.y = wjOff.y;
					}
					else if (Vector3.Angle(normal, input) < 90)//Backward movement
					{
						velocity = (normal + input).normalized * wjLeap.z;
						velocity.y = wjLeap.y;
					}
					else//forward movement
					{
						velocity = normal * wjClimb.z;//(normal + input).normalized * wjClimb.z;
						velocity.y = wjClimb.y;
					}
				}
			}
			if (Input.GetButtonUp("Jump") && velocity.y > minJumpVelocity)
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
	}

	private void OnControllerColliderHit(ControllerColliderHit hit)
	{
		if (hit.gameObject.tag == "Lethal")
			Die();
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

	void Die()
	{
		isAlive = false;
		Explode();
		Invoke("Respawn", 1);
	}

	void Explode()
	{
		transform.FindChild("Sphere").gameObject.SetActive(false);
		velocity = Vector3.zero;
		GameObject dp = Instantiate(deadPlayer, transform.position, transform.rotation) as GameObject;
		foreach(Rigidbody rb in dp.GetComponentsInChildren<Rigidbody>())
		{
			rb.AddForce(new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)) * 100);
		}
	}

	void Respawn()
	{
		transform.position = GameObject.FindGameObjectWithTag("Spawn").transform.position;
		GameObject.Find("Timer").GetComponent<Timer>().Reset();
		isAlive = true;
		transform.FindChild("Sphere").gameObject.SetActive(true);
	}
}
