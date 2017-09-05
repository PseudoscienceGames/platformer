using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CamFollow : MonoBehaviour
{
	public Player player;
	public Transform camTarget;
	public Transform pivot;
	public float smoothTime = 0.3F;
	public float camTargetSmoothTime;
	public float rotSmoothTime;
	public Vector3 desiredRot;
	private Vector3 velocity = Vector3.zero;
	private Vector3 camTargetVelocity = Vector3.zero;
	private Vector3 rotVel = Vector3.zero;
	private Vector3 offset;
	public float yRotSpeed;
	public float xRotSpeed;
	public float camFollowDistance;
	public Vector3 boxDims;
	public float camEdgeBuffer;
	public LayerMask lM;
	public Vector3 localPos;
	public Vector3 tempPos;
	public float xShiftSpeed;
	public float maxCamTargetSpeed;
	public float turnAngle;

	private void Start()
	{
		player = GameObject.Find("Player").GetComponent<Player>();
		pivot = transform.Find("Pivot");
		camTarget = GameObject.Find("CamTarget").transform;
		camTarget.position = player.transform.position;
	}

	void LateUpdate()
	{
		transform.Rotate(Vector3.up * Time.deltaTime * yRotSpeed * Input.GetAxis("TurnCamY"));
		camTarget.rotation = transform.rotation;
		localPos = camTarget.InverseTransformPoint(player.transform.position);
		tempPos = camTarget.position;
		CheckY();
		CheckZ();
		CheckX();

		camTarget.position = Vector3.Lerp(camTarget.position, tempPos, maxCamTargetSpeed);
		transform.position = Vector3.SmoothDamp(transform.position, camTarget.position, ref velocity, smoothTime);


		//RaycastHit hit;
		//Ray ray = new Ray(camTarget.position, Camera.main.transform.position - camTarget.position);
		//if (Physics.Raycast(ray, out hit, Vector3.Distance(camTarget.position, Camera.main.transform.position)) && pivot.localEulerAngles.x < 90)
		//{
		//	pivot.transform.Rotate(Vector3.right * Time.deltaTime * yRotSpeed);
		//	Debug.DrawLine(ray.origin, hit.point, Color.red, 1000);
		//}

		//else if (pivot.localEulerAngles.x > desiredRot.x)
		//	pivot.transform.Rotate(Vector3.right * Time.deltaTime * -yRotSpeed);

	}

	void CheckX()
	{
		RaycastHit hit;
		Ray ray = new Ray(camTarget.position + (Vector3.up * player.GetComponent<CharacterController>().height), transform.right);
		if (Physics.Raycast(ray, out hit, boxDims.x, lM) && Vector3.Angle(Vector3.up, hit.normal) == 90f)
		{
			Debug.DrawLine(ray.origin, hit.point, Color.red, 1000f);
			tempPos -= transform.right * (boxDims.x - hit.distance) * xShiftSpeed;
		}
		ray.direction = -transform.right;
		if (Physics.Raycast(ray, out hit, boxDims.x, lM) && Vector3.Angle(Vector3.up, hit.normal) == 90f)
		{
			Debug.DrawLine(ray.origin, hit.point, Color.red, 1000f);
			tempPos += transform.right * (boxDims.x - hit.distance) * xShiftSpeed;
		}
	}

	void CheckY()
	{
		//RaycastHit hit;
		//Ray ray = new Ray(camTarget.position + (Vector3.up * boxDims.y), -Vector3.up);

		//if (player.isGrounded)
		//{
		//	if (Physics.Raycast(ray, out hit, 10, lM) && hit.distance < boxDims.y * .95f)
		//		tempPos.y = hit.point.y;
		//	else
		//		tempPos.y = player.transform.position.y;

		//}
		//else 
		if (localPos.y > boxDims.y * 2f)
			tempPos += transform.up * (localPos.y - (boxDims.y * 2f));
		else if (localPos.y < 0)
			tempPos += transform.up * localPos.y;
	}
	//kasldjfhglksjdfhglksjdhfglkjsdhfglksjdhfglkjsdhfglkjsdhfg
	void CheckZ()
	{
		Vector3 flatLocalPos = new Vector3(localPos.x, 0, localPos.z);

		Vector3 onEllipse = new Vector3();
		turnAngle = Vector3.Angle(Vector3.right, flatLocalPos);
		float angle = turnAngle * Mathf.Deg2Rad;
		float a = boxDims.x;
		float b = boxDims.z;
		onEllipse.x = a * b;
		onEllipse.x /= Mathf.Sqrt((b * b) + (a * a) * (Mathf.Pow(Mathf.Tan(angle), 2)));
		onEllipse.z = a * b * Mathf.Tan(angle);
		onEllipse.z /= Mathf.Sqrt((b * b) + (a * a) * (Mathf.Pow(Mathf.Tan(angle), 2)));
		if (Mathf.Rad2Deg * angle > 90 && Mathf.Rad2Deg * angle < 270)
			onEllipse *= -1f;
		if (localPos.z < 0)
			onEllipse.z *= -1;
		onEllipse = camTarget.TransformPoint(onEllipse);
		onEllipse.y = 0f;
		Debug.Log(flatLocalPos.magnitude + " " + onEllipse.magnitude);
		if (flatLocalPos.magnitude > onEllipse.magnitude)
		{
			Debug.DrawLine(player.transform.position, onEllipse, Color.red, 1000f);
			Vector3 pos = player.transform.position;
			pos = pos - onEllipse;
			pos.y = 0;
			//	//pos = camTarget.TransformPoint(pos);
			//	pos = player.transform.position - pos;
			//	pos.y = 0;
			tempPos += pos;
			}
		}
}
