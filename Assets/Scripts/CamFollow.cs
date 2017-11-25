using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CamFollow : MonoBehaviour
{
	public Player player;
	public Transform camTarget;
	public Transform pivot;
	public float smoothTimeX = 0.075F;
	public float smoothTimeY = 0.075F;
	public float smoothTimeZ = 0.075F;
	private float velocityX = 0;
	private float velocityY = 0;
	private float velocityZ = 0;
	public float yRotSpeed;
	public float xRotSpeed;
	public Vector3 boxDims;
	public LayerMask lM;
	public Vector3 localPos;
	public Vector3 tempPos;
	public float xShiftSpeed;
	public float turnAngle;

	private void Start()
	{
		player = GameObject.Find("Player").GetComponent<Player>();
		pivot = transform.Find("Pivot");
		camTarget = GameObject.Find("CamTarget").transform;
		camTarget.position = player.transform.position;
	}

	private void OnGUI()
	{
		Cursor.lockState = CursorLockMode.Locked;
	}

	void LateUpdate()
	{
		transform.Rotate(Vector3.up * Time.deltaTime * yRotSpeed * Input.GetAxis("Mouse X"));
		float rot = pivot.localEulerAngles.x + (Time.deltaTime * xRotSpeed * -Input.GetAxis("Mouse Y"));
		pivot.localEulerAngles = new Vector3(Mathf.Clamp(rot, 0, 85), 0, 0);
		camTarget.rotation = transform.rotation;
		localPos = camTarget.InverseTransformPoint(player.transform.position);
		tempPos = camTarget.position;
		CheckY();
		CheckZ();
		CheckX();

		camTarget.position = tempPos;// Vector3.MoveTowards(camTarget.position, tempPos, camTargetSmoothTime);
		Vector3 newPos = transform.position;
		newPos.x = Mathf.SmoothDamp(newPos.x, camTarget.position.x, ref velocityX, smoothTimeX);
		float tempSmoothTimeY = smoothTimeY;
		if (localPos.y < 0)
			tempSmoothTimeY *= 0.5f;

		newPos.y = Mathf.SmoothDamp(newPos.y, camTarget.position.y, ref velocityY, tempSmoothTimeY);
		newPos.z = Mathf.SmoothDamp(newPos.z, camTarget.position.z, ref velocityZ, smoothTimeZ);
		transform.position = newPos;
		CheckZPivot();

	}

	void CheckZPivot()
	{
		//Transform lOS = GameObject.Find("LOSCheck").transform;
		//Ray rayTop = new Ray((lOS.position + (Vector3.up * 0.25f)), Camera.main.transform.position );
		//Ray rayMid = new Ray(lOS.position, Camera.main.transform.position);
		//Ray rayBot = new Ray((lOS.position - (Vector3.up * 0.25f)), Camera.main.transform.position);
		//RaycastHit hitTop;
		//RaycastHit hitMid;
		//RaycastHit hitBot;

		//if (Physics.Raycast(rayMid, out hitMid) && hitMid.transform.gameObject.tag != "Player")
		//{
		//	Debug.DrawLine(rayMid.origin, hitMid.point, Color.red, 1000f);
		//	if (Physics.Raycast(rayTop, out hitTop))
		//		Debug.DrawLine(rayTop.origin, hitTop.point, Color.red, 1000f);
		//	 if(Physics.Raycast(rayBot, out hitBot))
		//		Debug.DrawLine(rayBot.origin, hitBot.point, Color.red, 1000f);
		//	Debug.Break();
		//}
	}

	void CheckX()
	{
		RaycastHit hit;
		Ray ray = new Ray(camTarget.position + (Vector3.up * player.GetComponent<CharacterController>().height), transform.right);
		if (Physics.Raycast(ray, out hit, boxDims.x, lM) && Vector3.Angle(Vector3.up, hit.normal) == 90f)
		{
			Debug.DrawLine(ray.origin, hit.point, Color.red, 1000f);
			ray.origin = ray.origin + (Vector3.up * boxDims.y);
			if (Physics.Raycast(ray, out hit, boxDims.x, lM) && Vector3.Angle(Vector3.up, hit.normal) == 90f)
			{
				tempPos -= transform.right * (boxDims.x - hit.distance) * xShiftSpeed;
			}
		}
		ray = new Ray(camTarget.position + (Vector3.up * player.GetComponent<CharacterController>().height), -transform.right);
		if (Physics.Raycast(ray, out hit, boxDims.x, lM) && Vector3.Angle(Vector3.up, hit.normal) == 90f)
		{
			Debug.DrawLine(ray.origin, hit.point, Color.red, 1000f);
			ray.origin = ray.origin + (Vector3.up * boxDims.y);
			if (Physics.Raycast(ray, out hit, boxDims.x, lM) && Vector3.Angle(Vector3.up, hit.normal) == 90f)
			{
				tempPos += transform.right * (boxDims.x - hit.distance) * xShiftSpeed;
			}
		}
	}

	void CheckY()
	{
		RaycastHit hit;
		Vector3 origin = player.transform.position;
		origin.y = camTarget.position.y + boxDims.y;
		Ray ray = new Ray(origin, -Vector3.up);
		if (localPos.y > boxDims.y)
			tempPos += transform.up * (localPos.y - (boxDims.y));
		else if (localPos.y < 0)
			tempPos += transform.up * localPos.y;
		if (player.isGrounded && Physics.Raycast(ray, out hit, boxDims.y, lM))
			tempPos.y += player.transform.position.y - tempPos.y;
	}

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
		Vector3 flatOnEllipse = camTarget.InverseTransformPoint(onEllipse);
		flatOnEllipse.y = 0;
		if (flatLocalPos.magnitude >= flatOnEllipse.magnitude)
		{
			Vector3 pos = player.transform.position;
			pos = pos - onEllipse;
			pos.y = 0;
			tempPos += pos;
		}
	}
}
