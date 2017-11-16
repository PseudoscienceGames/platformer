using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
	public GameObject bullet;
	public float timer = 0;

	private void Update()
	{
		timer -= Time.deltaTime;
		if (timer < 0)
			timer = 0;
		if(Input.GetAxis("Fire1") != 0 && timer == 0)
		{
			Transform t = Camera.main.transform.parent.parent;
			Instantiate(bullet, transform.position + t.forward + (Vector3.up * 0.25f), t.rotation);
			timer = .1f;
		}
	}
}
