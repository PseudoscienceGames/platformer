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
	public float runMultiplier;
	public float animRunSpeed;
	public ParticleSystem ps;
	public ParticleSystem landingPuff;

	float timeSinceGrounded = 0;
	float timeToWallUnstick;
	public float gravity;
	float maxJumpVelocity;
	float minJumpVelocity;
	Vector3 smoothdamp;
	public Vector3 velocity;
	Vector3 normal;
	CharacterController pc;
	bool isAlive = true;
	public GameObject deadPlayer;
	public Animator anim;
	public bool wasGrounded = true;

	void Start()
	{
		pc = GetComponent<CharacterController>();
		gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
		maxJumpVelocity = Mathf.Abs(gravity * timeToJumpApex);
		minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight);
		GameObject.Find("Timer").GetComponent<Timer>().Reset();
	}

	void Update()
	{
		if (isAlive)
		{
			if (pc.isGrounded)
			{
				if(!wasGrounded)
				{
					Debug.Log("Puff");
					if(Time.timeSinceLevelLoad > 1)
						landingPuff.Emit(20);
					wasGrounded = true;
				}
				if (!ps.isEmitting && (new Vector3(velocity.x, 0, velocity.z)).magnitude > 5)
					ps.Play();
				if (ps.isEmitting && (new Vector3(velocity.x, 0, velocity.z)).magnitude <= 5)
					ps.Stop();
				timeSinceGrounded = 0;
				velocity.y = -0.1f;
				anim.SetBool("isJumping", false);
			}
			else
			{
				if (ps.isEmitting)
					ps.Stop();
				timeSinceGrounded += Time.deltaTime;
				anim.SetBool("isJumping", true);
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
				if (pc.isGrounded || timeSinceGrounded <= jumpLeeway)
				{
					anim.SetBool("isJumping", true);
					velocity.y = maxJumpVelocity;
					timeSinceGrounded = jumpLeeway + 1;
				}
				else if (timeToWallUnstick > 0)
				{
					anim.SetTrigger("WallJump");
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
					//velocity *= gravity / -62.5f;
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
			if ((new Vector3(velocity.x, 0, velocity.z)).magnitude > 0.05f)
			{
				anim.SetBool("isRunning", true);
				anim.SetFloat("Speed", (new Vector3(velocity.x, 0, velocity.z)).magnitude * animRunSpeed);
			}
			else
			{
				anim.SetBool("isRunning", false);
				anim.SetFloat("Speed", 1);
			}
			pc.Move(velocity * Time.deltaTime);
		}
	}

	private void OnControllerColliderHit(ControllerColliderHit hit)
	{
		if (hit.gameObject.tag == "Lethal")
			Die();
		if (hit.gameObject.tag == "Goal")
		{
			Win();
			hit.gameObject.GetComponent<Goal>().Load();
		}
		if (hit.gameObject.tag == "LevelIcon")
		{
			hit.gameObject.GetComponent<LevelIcon>().Load();
		}
		normal = hit.normal;
		if (!pc.isGrounded && hit.normal.y < 0.1f && hit.normal.y > -0.5f)
		{
			ps.Emit(1);
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
		transform.FindChild("Char").gameObject.SetActive(false);
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
		transform.FindChild("Char").gameObject.SetActive(true);
	}
	void Win()
	{
		GameObject.Find("Timer").GetComponent<Timer>().Stop();
		isAlive = false;
	}
}
