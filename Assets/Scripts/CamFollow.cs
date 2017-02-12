using UnityEngine;
using System.Collections;

public class CamFollow : MonoBehaviour
{
	public Transform target1;
	public Vector3 offset;

	void LateUpdate()
	{
		transform.position = target1.position;
		transform.GetChild(0).position = transform.position + offset;
		transform.GetChild(0).LookAt(target1);
	}
}
