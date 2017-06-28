using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CamFollow : MonoBehaviour
{
	public Player player;
	public Transform pivot;
	public float smoothTime = 0.3F;
	public float rotSmoothTime;
	public Vector3 desiredRot;
	private Vector3 velocity = Vector3.zero;
	private Vector3 rotVel = Vector3.zero;
	private Vector3 offset;
	public float rotSpeed;


	private void Start()
	{
		player = GameObject.Find("Player").GetComponent<Player>();
		pivot = transform.Find("Pivot");

	}

	void LateUpdate()
	{
		transform.Rotate(Vector3.up * Time.deltaTime * rotSpeed * Input.GetAxis("TurnCamY"));
		float rot = pivot.localEulerAngles.x + (Time.deltaTime * rotSpeed * Input.GetAxis("TurnCamX"));
		pivot.localEulerAngles = new Vector3(Mathf.Clamp(rot, 0, 85), 0, 0);
		transform.position = Vector3.SmoothDamp(transform.position, player.transform.position, ref velocity, smoothTime);
		if (Input.GetAxis("TurnCamX") == 0)
			pivot.localEulerAngles = Vector3.SmoothDamp(pivot.localEulerAngles, desiredRot, ref rotVel, rotSmoothTime);
		Camera.main.transform.LookAt(player.transform.Find("CamTarget"));
	}
}
