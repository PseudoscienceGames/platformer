using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlobAnim : MonoBehaviour
{
	public List<Transform> rightBones = new List<Transform>();
	public List<Transform> centerBones = new List<Transform>();
	public List<Transform> leftBones = new List<Transform>();
	public Transform leftArm;
	public Transform rightArm;
	public float rotSpeed;

	private void Update()
	{
		leftArm.Rotate(Vector3.right, rotSpeed * Time.deltaTime, Space.World);
		rightArm.Rotate(Vector3.right, rotSpeed * Time.deltaTime, Space.World);
		foreach (Transform bone in rightBones)
		{
			bone.Rotate(Vector3.right, rotSpeed * Time.deltaTime, Space.World);
		}
		foreach (Transform bone in centerBones)
		{
			bone.Rotate(Vector3.right, rotSpeed * Time.deltaTime, Space.World);
		}
		foreach (Transform bone in leftBones)
		{
			bone.Rotate(Vector3.right, rotSpeed * Time.deltaTime, Space.World);
		}
	}
}
