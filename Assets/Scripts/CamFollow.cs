using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CamFollow : MonoBehaviour
{
	public Player player;
	public Transform camTarget;
	public Transform pivot;
	public float smoothTime = 0.3F;
	public float rotSmoothTime;
	public Vector3 desiredRot;
	private Vector3 velocity = Vector3.zero;
	private Vector3 rotVel = Vector3.zero;
	private Vector3 offset;
	public float yRotSpeed;
	public float xRotSpeed;
	public float camFollowDistance;

	private void Start()
	{
		player = GameObject.Find("Player").GetComponent<Player>();
		pivot = transform.Find("Pivot");
		camTarget = GameObject.Find("CamTarget").transform;
	}

	void LateUpdate()
	{
		transform.Rotate(Vector3.up * Time.deltaTime * yRotSpeed * Input.GetAxis("TurnCamY"));
		//if (Mathf.Abs(Input.GetAxis("TurnCamX")) > Mathf.Abs(Input.GetAxis("TurnCamY")))
		//{
		//	float rot = pivot.localEulerAngles.x + (Time.deltaTime * xRotSpeed * Input.GetAxis("TurnCamX"));
		//	pivot.localEulerAngles = new Vector3(Mathf.Clamp(rot, 0, 85), 0, 0);
		//}
		if (Vector3.Distance(transform.position, camTarget.position) > camFollowDistance)
		{
			Vector3 cFPos = (transform.position - camTarget.position).normalized * camFollowDistance;
			transform.position = Vector3.SmoothDamp(transform.position, camTarget.position + cFPos, ref velocity, smoothTime);
		}
		//if (Input.GetAxis("TurnCamX") == 0)
		//	pivot.localEulerAngles = Vector3.SmoothDamp(pivot.localEulerAngles, desiredRot, ref rotVel, rotSmoothTime);
	}
}
