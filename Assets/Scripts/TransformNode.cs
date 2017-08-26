using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformNode : MonoBehaviour
{
	public TransformController tc;
	public Vector3 rotate;
	public Vector3 scale;
	public float time;
	private Vector3 startPos;
	private Vector3 startRot;
	private Vector3 startScale;
	private float timeElapsed = 0;
	public bool active;

	public void Activate()
	{
		timeElapsed = 0;
		tc = transform.parent.GetComponent<TransformController>();
		active = true;
		startPos = tc.obj.position;
		startRot = tc.obj.eulerAngles;
		startScale = tc.obj.localScale;
		if (scale == Vector3.zero)
			scale = tc.obj.localScale;
	}

	void Update ()
	{
		if (active)
		{
			tc.obj.position = Vector3.Lerp(startPos, transform.position, timeElapsed / time);
			tc.obj.eulerAngles = Vector3.Lerp(startRot, startRot + rotate, timeElapsed / time);
			tc.obj.localScale = Vector3.Lerp(startScale, scale, timeElapsed / time);
			timeElapsed += Time.deltaTime;
			if (timeElapsed >= time)
			{
				active = false;
				transform.parent.GetComponent<TransformController>().Next();
			}
		}
	}
}
