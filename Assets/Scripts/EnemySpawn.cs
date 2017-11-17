using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour {

	public GameObject enemy;
	public float timer;

	// Use this for initialization
	void Start () {
		InvokeRepeating("Spawn", 0, 1);
	}

	void Spawn()
	{
		Instantiate(enemy, transform.position, Quaternion.identity);
	}
}