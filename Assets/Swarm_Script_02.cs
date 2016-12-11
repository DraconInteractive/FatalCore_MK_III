using UnityEngine;
using FMOD;
using FMODUnity; 
using System.Collections;
using System.Collections.Generic;

public class Swarm_Script_02 : MonoBehaviour {

	public float swarmSightRadius;
	public float playerSightRadius;

	public List<GameObject> swarmMembers;

	Vector3 centreForce;
	Vector3 sepForce;
	Vector3 targetForce;

	public float maxAccelleration;

	public GameObject bulletTemplate;

	bool movingToTarget = true;

	GameObject player;

	public GameObject home;

	Sprite mySprite;

	public float centerPriority, seperatePriority, targetPriority, fireRadius;

	Rigidbody rb;

	public bool drawRadii;

	float currentHealth;
	public float maxHealth;

	bool dead;

	public GameObject deadSwarm;

	[EventRef]
	public string movementEventRef;

	FMOD.Studio.EventInstance movementInst;
//	FMOD_StudioEventEmitter movementEmitter;

//	[FMODUnity.EventRef]
//	public string movementEvent;
//	public FMOD.Studio.EventInstance movementInstance;
	void Awake () {
		rb = GetComponent<Rigidbody> ();
	}

	void OnDrawGizmos () {
		if (drawRadii){
			Gizmos.color = Color.red;
			Gizmos.DrawWireSphere (transform.position, swarmSightRadius);
			Gizmos.color = Color.green;
			Gizmos.DrawWireSphere (transform.position, playerSightRadius);
			Gizmos.color = Color.blue;
			Gizmos.DrawWireSphere (transform.position, fireRadius);
		}
	}
	// Use this for initialization
	void Start () {
//		player = Player_Script.playerObj;
		player = Player_Script.playerObj;

		currentHealth = maxHealth;

		swarmMembers.Remove (this.gameObject);

		StartCoroutine (DoSwarm ());
		StartCoroutine (FireUpdate ());

		movementInst = FMODUnity.RuntimeManager.CreateInstance (movementEventRef);
//		FMODUnity.RuntimeManager.AttachInstanceToGameObject (movementInst, transform, rb);
		RuntimeManager.AttachInstanceToGameObject (movementInst, gameObject.transform, rb);
		movementInst.setVolume (10);
		movementInst.start ();
//		movementInstance = FMODUnity.RuntimeManager.CreateInstance (movementEvent);
//		FMODUnity.RuntimeManager.AttachInstanceToGameObject (movementInstance, this.gameObject.transform, rb);
//		movementInstance.start ();
//		RESULT Sound.set3DMinMaxDistance (1, 10000);
//		RESULT movementEmitter.Sound.set3DMinMaxDistance(1,10000;)


	}


		
	
	
	// Update is called once per frame
//	void Update () {
//	
//	}

	void DoSeperateAndCohesion () {
		Vector3 swarmDirection = Vector3.zero;
//		Vector3 closestSwarmerDirection = Vector3.zero;
		Vector3 swarmCenter = Vector3.zero;

		float shortestDistance = Mathf.Infinity;
		Vector3 closestLocation = transform.position;

		for (int i = 0; i < swarmMembers.Count; i++) {
			if (swarmMembers[i] != null) {
				swarmDirection += swarmMembers [i].transform.forward;
				swarmCenter += swarmMembers [i].transform.position;
				float testDistance = Vector3.Distance (transform.position, swarmMembers [i].transform.position);
				if (testDistance < shortestDistance) {
					shortestDistance = testDistance;
					closestLocation = swarmMembers [i].transform.position;
				}
			}
		}

		swarmDirection /= swarmMembers.Count - 1;
		swarmCenter /= swarmMembers.Count - 1;

		if (shortestDistance != Mathf.Infinity) {
			sepForce = transform.position - closestLocation;
		} else {
			sepForce = Vector3.zero;
		}

		if (movingToTarget) {
			if (Vector3.Distance(transform.position, player.transform.position) < 10) {
				targetForce = Vector3.Normalize (transform.position - player.transform.position) * maxAccelleration;
			} else {
				targetForce = Vector3.Normalize (player.transform.position - transform.position) * maxAccelleration;
			}

			if (Vector3.Distance(transform.position, player.transform.position) > playerSightRadius) {
				targetForce = Vector3.Normalize (home.transform.position - transform.position) * maxAccelleration;
			}
		}

		centreForce = Vector3.Normalize (swarmCenter - transform.position) * maxAccelleration;
		sepForce = Vector3.Normalize (sepForce) * maxAccelleration;
	}

	private IEnumerator DoSwarm () {
		while (true) {
			DoSeperateAndCohesion ();
			yield return new WaitForSeconds (0.5f);
		}
	}

	void FixedUpdate () {
		Vector3 accelToAdd = centreForce * centerPriority + sepForce * seperatePriority + targetForce * targetPriority;
		accelToAdd = Vector3.Normalize (accelToAdd) * maxAccelleration;
		rb.AddForce (accelToAdd, ForceMode.Acceleration);

		Quaternion desRot = Quaternion.LookRotation ((player.transform.position - transform.position), Vector3.up);
		transform.rotation = Quaternion.Lerp (transform.rotation, desRot, 0.15f);
	}

	private IEnumerator FireUpdate () {
		while (true) {
			if (Vector3.Distance(player.transform.position, transform.position) < fireRadius) {
				RaycastHit hit;
				if (Physics.Raycast (transform.position, player.transform.position - transform.position, out hit, Mathf.Infinity)) {
					if (hit.collider.gameObject == player) {
						GameObject bullet = Instantiate (bulletTemplate, transform.position, Quaternion.identity) as GameObject;
						bullet.GetComponent<Rigidbody> ().AddForce ((player.transform.position - transform.position).normalized * 50, ForceMode.VelocityChange);

					}
				}
			}
			yield return new WaitForSeconds (1.5f);
		}
	}

	public void DamageAI (int damage) {

		currentHealth -= damage;

		if (currentHealth < 0) {
			player.GetComponent<Player_Script> ().UpdateEnemyCounter ();
			if (!dead) {
				print ("Wasp Dead");
				dead = true;
//				GameObject g = Instantiate (deadSwarm, transform.position, transform.rotation) as GameObject;
				//g.GetComponent<Rigidbody> ().velocity = rb.velocity;
				Instantiate (deadSwarm, transform.position, transform.rotation);
				Destroy (this.gameObject);
			}

		}


	}
}
