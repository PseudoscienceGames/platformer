using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotor : MonoBehaviour
{

	void Start ()
	{
		transform.rotation = Random.rotation;
	}

}
