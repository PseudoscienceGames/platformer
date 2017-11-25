using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameCounter : MonoBehaviour
{
	void Update()
	{
		if(Mathf.RoundToInt(1f / Time.deltaTime) < 60f)
			Debug.Log(Mathf.RoundToInt(1f / Time.deltaTime) + " " + Time.deltaTime * 1000f);
	}
}
