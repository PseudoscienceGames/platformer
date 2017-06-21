﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEffects : MonoBehaviour
{
	public ParticleSystem ps;
	public ParticleSystem landingPuff;
	public GameObject deadPlayer;
	public Animator anim;
	public Vector3 velocity;
	public float animRunSpeed;
	Player p;
	CharacterController pc;
	bool isAlive;
	WindZone wind;

	private void Start()
	{
		p = GetComponent<Player>();
		pc = GetComponent<CharacterController>();
		wind = transform.Find("WindZone").GetComponent<WindZone>();
		Invoke("Respawn", .5f);
	}
	private void LateUpdate()
	{
		isAlive = p.isAlive;
		velocity = p.velocity;
		wind.windMain = new Vector3(velocity.x, 0, velocity.z).magnitude;
		if (isAlive)
		{
			if (pc.isGrounded)
			{
				if (!ps.isEmitting && (new Vector3(velocity.x, 0, velocity.z)).magnitude > 5)
					ps.Play();
				if (ps.isEmitting && (new Vector3(velocity.x, 0, velocity.z)).magnitude <= 5)
					ps.Stop();
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

	void Pickup(GameObject pickup)
	{
		//pickups++;
		//pickup.SetActive(false);
		//pickupCount.text = pickups.ToString();
	}
	void Die()
	{
		isAlive = false;
		GetComponent<Player>().isAlive = false;
		Explode();
		Invoke("MoveCamera", 0.5f);
		Invoke("Respawn", 1);
		p.velocity = Vector3.zero;
	}
	void Explode()
	{
		transform.Find("Char").gameObject.SetActive(false);
		velocity = Vector3.zero;
		GameObject dp = Instantiate(deadPlayer, transform.position, transform.rotation) as GameObject;
		foreach (Rigidbody rb in dp.GetComponentsInChildren<Rigidbody>())
		{
			rb.AddForce(new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)) * 100);
		}
	}
	void MoveCamera()
	{
		transform.position = GameObject.FindGameObjectWithTag("Spawn").transform.position;
	}
	void Respawn()
	{
		transform.position = GameObject.FindGameObjectWithTag("Spawn").transform.position;
		GameObject.Find("Timer").GetComponent<Timer>().Reset();
		GetComponent<Player>().isAlive = true;
		isAlive = true;
		transform.Find("Char").gameObject.SetActive(true);
	}
	void Win(Goal goal)
	{
		anim.SetBool("Stop", true);
		ps.Pause();
		landingPuff.Pause();
		goal.Load();
		GameObject.Find("Timer").GetComponent<Timer>().Stop();
		isAlive = false;
	}
}