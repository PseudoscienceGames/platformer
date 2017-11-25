using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
	public Transform player;
	public NavMeshAgent nav;
	public float hp;

	void Start ()
	{
		player = GameObject.Find("Player").transform;
		nav = GetComponent<NavMeshAgent>();
		InvokeRepeating("CalcPath", 0, .1f);
	}
	
	void CalcPath()
	{
		nav.SetDestination(player.position);
	}

	public void TakeDamage(float damage)
	{
		hp -= damage;
		if (hp <= 0)
			Destroy(gameObject);
	}
}
