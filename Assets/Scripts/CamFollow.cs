using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CamFollow : MonoBehaviour
{
	public Transform target1;
	public Vector3 offset;
	public List<Transform> trackingTargets = new List<Transform>();
	public Transform currentTracker;
	public Transform nextTracker;
	int trackerNum = 0;

	void Start()
	{
		currentTracker = trackingTargets[0];
		nextTracker = trackingTargets[1];
	}
	void LateUpdate()
	{
		//if(Vector3.Distance(transform.position, currentTracker.position) > Vector3.Distance(nextTracker.position, currentTracker.position))
		//{
		//	trackerNum++;
		//	currentTracker = trackingTargets[trackerNum];
		//	nextTracker = trackingTargets[trackerNum + 1];
		//}
		//Vector3 ap = target1.transform.position - currentTracker.position;
		//Vector3 ab = nextTracker.position - currentTracker.position;
		//float ab2 = ab.x * ab.x + ab.y * ab.y + ab.z * ab.z;
		//float apab = ap.x * ab.x + ap.y * ab.y + ap.z * ab.z;
		//float t = apab / ab2;
		//Vector3 pos = currentTracker.position + ab * t;
		//pos.x = target1.position.x;
		//transform.position = Vector3.Lerp(transform.position, pos, 0.5f);
		transform.position = target1.position + offset;
		Camera.main.transform.LookAt(target1);
	}
}
