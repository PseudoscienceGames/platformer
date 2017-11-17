using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour {
	public Transform player;
	public NavMeshAgent nav;
	// Use this for initialization
	void Start () {
		player = GameObject.Find("Player").transform;
		nav = GetComponent<NavMeshAgent>();
		InvokeRepeating("CalcPath", 0, 1);
	}
	
	// Update is called once per frame
	void CalcPath()
	{
		nav.SetDestination(player.position);
	}
}
