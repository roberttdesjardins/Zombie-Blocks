using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour {

	Transform player;
	UnityEngine.AI.NavMeshAgent nav;
	EnemyHealth enemyHealth;

	void Awake() {
		player = GameObject.FindGameObjectWithTag ("Player").transform;
		enemyHealth = GetComponent<EnemyHealth>();
		nav = GetComponent<UnityEngine.AI.NavMeshAgent>();
	}
	
	// Update is called once per frame
	void Update () {
		if (enemyHealth.currentHealth > 0) {
			nav.SetDestination (player.position);
		}
	}
}
