using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spinner : MonoBehaviour
{
	public Vector3 speedDir;

	private void Update()
	{
		transform.Rotate(speedDir * Time.deltaTime);
	}
}
