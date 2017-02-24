using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
	public float time;
	public Text text;

	bool count = false;

	void LateUpdate()
	{
		if (count)
		{
			time += Time.deltaTime;
			text.text = time.ToString();
		}
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
