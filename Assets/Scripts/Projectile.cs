using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
	public float force;
	public float lifespan;
	public bool grav;
	public float damage;

	void Start ()
	{
		Rigidbody rb = GetComponent<Rigidbody>();
		rb.useGravity = grav;
		rb.AddForce(transform.forward * force);
		StartCoroutine("Timer");
	}

	public virtual void OnCollisionEnter(Collision collision)
	{
		Hit(collision.gameObject);
	}

	IEnumerator Timer()
	{
		float time = lifespan;
		while(time > 0)
		{
			time -= Time.deltaTime;
			yield return null;
		}
		Hit(null);
		yield return null;
	}

	public virtual void Hit(GameObject hit)
	{
		Debug.Log(hit);
		if (hit != null)
		{
			if (hit.tag == "Enemy")
				hit.GetComponent<Enemy>().TakeDamage(damage);
		}
		Destroy(gameObject);
	}
}
