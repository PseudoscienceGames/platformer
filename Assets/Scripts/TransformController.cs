using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformController : MonoBehaviour
{
	public Transform obj;
	public float startTime;
	public bool repeat;
	public int onNode;
	private bool going = false;
	public List<TransformNode> nodes = new List<TransformNode>();

	public void Begin()
	{
		nodes[0].Activate();
	}
	public void Next()
	{
		onNode++;
		if(onNode >= nodes.Count)
		{
			if (repeat)
				onNode = 0;
		}
		nodes[onNode].Activate();
	}
	private void Update()
	{
		if (!going)
		{
			if (GameObject.Find("Timer").GetComponent<Timer>().time >= startTime)
			{
				Begin();
				going = true;
			}
		}
	}
}
