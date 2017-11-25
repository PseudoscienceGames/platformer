using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour {
	Transform cam;
	// Use this for initialization
	void Start () {
		cam = Camera.main.transform.parent.parent;
	}
	
	// Update is called once per frame
	void Update () {
		transform.rotation = cam.rotation;
	}
}
