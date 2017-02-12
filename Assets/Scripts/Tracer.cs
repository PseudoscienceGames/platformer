using UnityEngine;
using System.Collections;

public class Tracer : MonoBehaviour
{
	public Color myColor;
	public float time;

	Vector3 oldPos;

	void Start()
	{
		oldPos = transform.position;
	}

	void Update()
	{
		//Debug.DrawLine(transform.position, oldPos, myColor, time);
		Debug.DrawLine(transform.position, oldPos, Color.red, time);
		oldPos = transform.position;
	}

}
