using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
	public float time;
	bool count = false;

	void LateUpdate()
	{
		if(count)
			time += Time.deltaTime;
	}

	public void Reset()
	{
		time = 0;
		count = true;
	}
	public void Stop()
	{
		count = false;
	}
}
