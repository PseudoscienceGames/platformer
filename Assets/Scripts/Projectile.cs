using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
	public float force;

	void Start ()
	{
		GetComponent<Rigidbody>().AddForce(transform.forward * force);
		StartCoroutine("Timer");
	}

	private void OnCollisionEnter(Collision collision)
	{
		Destroy(gameObject);
	}

	IEnumerator Timer()
	{
		float time = 2;
		while(time > 0)
		{
			time -= Time.deltaTime;
			yield return null;
		}
		Destroy(gameObject);
		yield return null;
	}
}
