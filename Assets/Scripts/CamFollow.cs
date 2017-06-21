using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CamFollow : MonoBehaviour
{
	public Transform player;
	public Transform pivot;
	public float smoothTime = 0.3F;
	private Vector3 velocity = Vector3.zero;
	private Vector3 offset;
	public float rotSpeed;


	private void Start()
	{
		player = GameObject.Find("Player").transform;
		pivot = transform.Find("Pivot");
	}

	void LateUpdate()
	{
		transform.Rotate(Vector3.up * Time.deltaTime * rotSpeed * Input.GetAxis("TurnCamY"));
		pivot.Rotate(Vector3.right * Time.deltaTime * rotSpeed * Input.GetAxis("TurnCamX"));
		transform.position = Vector3.SmoothDamp(transform.position, player.position, ref velocity, smoothTime);
		Camera.main.transform.LookAt(player.Find("CamTarget"));
	}
}
