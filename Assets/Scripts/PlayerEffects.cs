using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerEffects : MonoBehaviour
{
	public ParticleSystem ps;
	public ParticleSystem landingPuff;
	public Animator anim;
	public Vector3 velocity;
	public float animRunSpeed;
	public int pickups;
	public Text pickupCount;
	Player p;

	private void Start()
	{
		p = GetComponent<Player>();
		Invoke("Respawn", .5f);
		anim = transform.Find("Char").GetComponent<Animator>();
		pickupCount = GameObject.Find("PickupCount").GetComponent<Text>();
	}
	private void LateUpdate()
	{
		velocity = p.velocity;
		if (p.isGrounded)
		{
			if (!ps.isEmitting && (new Vector3(velocity.x, 0, velocity.z)).magnitude > 5)
				ps.Play();
			if (ps.isEmitting && (new Vector3(velocity.x, 0, velocity.z)).magnitude <= 5)
				ps.Stop();
			else
			{
				anim.SetBool("isSliding", false);
				
				anim.SetBool("isJumping", false);
				if (new Vector3(velocity.x, 0, velocity.z).magnitude > 0.05f)
				{
					anim.SetBool("isRunning", true);
					anim.SetFloat("Speed", (new Vector3(velocity.x, 0, velocity.z)).magnitude * animRunSpeed);
				}
				else
				{
					anim.SetBool("isRunning", false);
					anim.SetFloat("Speed", 1);
				}
			}
		}
		else
		{
			if (ps.isEmitting)
				ps.Stop();
			anim.SetBool("isJumping", true);
		}
		if (Input.GetButtonDown("Jump"))
		{
			//if (pc.isGrounded || timeSinceGrounded <= jumpLeeway)
			//{
			//	anim.SetBool("isJumping", true);
			//	velocity.y = maxJumpVelocity;
			//	timeSinceGrounded = jumpLeeway + 1;
			//}
			//else if (timeToWallUnstick > 0)
			//{
			//	anim.SetTrigger("WallJump");
			//}
		}
	}
	private void OnControllerColliderHit(ControllerColliderHit hit)
	{
		if (hit.gameObject.tag == "Pickup")
			Pickup(hit.gameObject);
		if (hit.gameObject.tag == "Lethal")
			Die();
		if (hit.gameObject.tag == "Goal")
			Win(hit.gameObject.GetComponent<Goal>());
	}

	public void Land()
	{
		landingPuff.Emit(50);
	}

	public void Pickup(GameObject pickup)
	{
		pickups++;
		pickup.SetActive(false);
		pickupCount.text = pickups.ToString();
	}
	void Die()
	{
		Explode();
		Invoke("MoveCamera", 0.5f);
		Invoke("Respawn", 1);
		p.velocity = Vector3.zero;
		p.enabled = false;
	}
	void Explode()
	{
		transform.Find("Char").gameObject.SetActive(false);
		velocity = Vector3.zero;
	}
	void MoveCamera()
	{
		transform.position = GameObject.FindGameObjectWithTag("Spawn").transform.position;
	}
	void Respawn()
	{
		transform.position = GameObject.FindGameObjectWithTag("Spawn").transform.position;
		GameObject.Find("Timer").GetComponent<Timer>().Reset();
		transform.Find("Char").gameObject.SetActive(true);
		p.enabled = true;
	}
	void Win(Goal goal)
	{
		anim.SetBool("Stop", true);
		ps.Pause();
		landingPuff.Pause();
		goal.Load();
		GameObject.Find("Timer").GetComponent<Timer>().Stop();
		p.enabled = false;
	}

	public void Slide()
	{
		anim.SetBool("isSliding", true);
	}
}
