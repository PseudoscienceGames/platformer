using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamControlVolume : MonoBehaviour
{
	public Vector3 start;
	public Vector3 end;
	public Vector3 targetOffset;
	public Vector3 startingOffset;
	public Vector3 currentOffset;
	public float targetAngle;
	public float startingAngle;
	public float currentAngle;
	public bool active = false;
	public Transform player;

	private void Start()
	{
		start = transform.position - (transform.forward * GetComponent<BoxCollider>().size.z / 2f);
		end = transform.position + (transform.forward * GetComponent<BoxCollider>().size.z / 2f);
		player = GameObject.Find("Player").transform;
	}

	private void Update()
	{
		if(active)
		{
			float ap = Vector3.Distance(player.position, start);
			float angle = Vector3.Angle(end - start, player.position - start) * Mathf.Deg2Rad;
			float ab = Vector3.Distance(start, end);
			float x = ap * Mathf.Cos(angle);
			float percentage = x / ab;
			currentOffset = Vector3.Lerp(startingOffset, targetOffset, percentage);
			currentAngle = Mathf.LerpAngle(startingAngle, targetAngle, percentage);
			Camera.main.transform.localPosition = currentOffset;
			Camera.main.transform.root.eulerAngles = new Vector3(0, currentAngle, 0);
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
			active = true;
	}
	private void OnTriggerExit(Collider other)
	{
		if (other.tag == "Player")
			active = false;
	}
}
