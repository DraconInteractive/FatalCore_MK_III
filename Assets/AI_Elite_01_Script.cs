﻿using UnityEngine;
using System.Collections;

public class AI_Elite_01_Script : MonoBehaviour {
	GameObject player;
	Rigidbody rb;
	public float detectionRange, firingRange, avoidanceRange;
	bool playerDetected, attacking;
	float playerDistance;
	public float baseSpeed;
	float speed;

	public int currentHealth, maxHealth;
	public int currentShield, maxShield;

	public Animator anim;
	// Use this for initialization
	void Start () {
		player = Player_Script.playerObj;
		rb = GetComponent<Rigidbody> ();

		currentHealth = maxHealth;
		currentShield = maxShield;
	}
	
	// Update is called once per frame
	void Update () {
		GetPlayerDistance ();
		DetectPlayer ();
		MovementAndRotation ();
		Combat ();
		UpdateAnim ();
	}

	void OnDrawGizmos () {
//		Gizmos.color = Color.green;
//		Gizmos.DrawWireSphere (transform.position, detectionRange);
//		Gizmos.color = Color.cyan;
//		Gizmos.DrawWireSphere (transform.position, avoidanceRange);
//		Gizmos.color = Color.red;
//		Gizmos.DrawWireSphere (transform.position, firingRange);
	}

	void UpdateAnim () {
		anim.SetFloat ("forwardVelocity", rb.velocity.z);
		anim.SetFloat ("rightVelocity", rb.velocity.x);
	}
	void GetPlayerDistance () {
		playerDistance = Vector3.Distance (transform.position, player.transform.position);
		speed = (playerDistance * baseSpeed) / 10;
	}

	void DetectPlayer () {
		if (playerDistance < detectionRange) {
			playerDetected = true;
		} else {
			playerDetected = false;
		}
	}

	void MovementAndRotation () {
		if (playerDetected && !attacking) {
			if (playerDistance < avoidanceRange) {
				rb.AddForce ((transform.position - player.transform.position).normalized * baseSpeed * 25);
			} else {
				rb.AddForce ((player.transform.position - transform.position).normalized * speed);
			}
		} else if (playerDetected && attacking) {
			rb.velocity = Vector3.Lerp (rb.velocity, Vector3.zero, 0.1f);
		}

		Quaternion desRot = Quaternion.LookRotation ((player.transform.position - transform.position).normalized, Vector3.up);
		transform.rotation = Quaternion.Lerp (transform.rotation, desRot, 0.15f);
	}

	void Combat () {
		if (playerDetected && playerDistance < firingRange) {
			
		}
	}

	public void DamageAI (int amount) {
		if (currentShield > 0) {
			currentShield -= amount;
		} else {
			currentHealth -= amount;
		}
	
		if (currentShield < 0) {
			currentShield = 0;
			currentHealth += currentShield;
		}

		if (currentHealth < 0) {
			player.GetComponent<Player_Script> ().UpdateEnemyCounter ();
			Destroy (this.gameObject);
		}
	}
}
