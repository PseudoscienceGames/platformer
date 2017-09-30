using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour
{
	Vector3 lastPos;

	private void Start()
	{
		lastPos = transform.position;
	}

	private void Update()
	{
		Debug.DrawLine(transform.position, lastPos, Color.red, 1000);
		lastPos = transform.position;
	}
}
