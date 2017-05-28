using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CamFollow : MonoBehaviour
{
	public Transform target1;
	public Vector3 offset;

	void LateUpdate()
	{
		transform.position = target1.position + offset;
		Camera.main.transform.LookAt(target1);
	}
}
