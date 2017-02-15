using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
	public float time;

	void Update()
	{
		time += Time.deltaTime;
	}

	public void Reset()
	{
		time = 0;
	}
}
