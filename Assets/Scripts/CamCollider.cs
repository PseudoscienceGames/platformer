using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamCollider : MonoBehaviour
{
	public bool isLeft;

	private void OnCollisionStay(Collision other)
	{
		Debug.Log("LSKDJFGLKSDJFLKSDF");
		if(isLeft)
			transform.root.Rotate(Vector3.up * Time.deltaTime * -150f);
		else
			transform.root.Rotate(Vector3.up * Time.deltaTime * 150f);
	}
}
