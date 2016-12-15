using UnityEngine;
using FMODUnity;
using FMOD;
using System.Collections;

public class AI_Elite_01_Script : MonoBehaviour {
	GameObject player;
	Rigidbody rb;
	public float detectionRange, firingRange, avoidanceRange;
	bool playerDetected;
	public bool attacking;
	float playerDistance;
	public float baseSpeed;
	float speed;

	public int currentHealth, maxHealth;
	public int currentShield, maxShield;

	public Animator anim;
	Golem_Attack_Behaviour gab;
	public GameObject bulletTemplate;
	public Golem_Laser laser;

	public GameObject deathElite;
	// Use this for initialization

	bool dead;

	public AudioSource weaponAS;
	public AudioSource movementAS;
	public AudioSource deathAS;

	public bool aggressive;
//	[EventRef]
//	public string movementSound;
//	FMOD.Studio.EventInstance movementEventRef;
	void Awake () {
//		anim = transform.GetChild (0).gameObject.GetComponent<Animator> ();
		gab = anim.GetBehaviour<Golem_Attack_Behaviour> ();
		print ("Gab retrieved: " + gab == null);
		gab.elite = this.gameObject.GetComponent<AI_Elite_01_Script> ();;

	}
	void Start () {
		player = Player_Script.playerObj;
		rb = GetComponent<Rigidbody> ();

		currentHealth = maxHealth;
		currentShield = maxShield;

//		movementEventRef = FMODUnity.RuntimeManager.CreateInstance (movementSound);
//		FMODUnity.RuntimeManager.AttachInstanceToGameObject (movementEventRef, this.gameObject.transform, rb);
//		movementEventRef.start ();
//		movementEventRef.setVolume (2);
//		float x = 0;
//		movementEventRef.getVolume (out x);
//		print (x);
	}
	
	// Update is called once per frame
	void Update () {
		GetPlayerDistance ();
		DetectPlayer ();

		if (aggressive) {
			MovementAndRotation ();
			Combat ();
		}

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
		if (playerDetected && playerDistance < firingRange && !attacking) {
			attacking = true;
			anim.SetTrigger ("Attack");
//			GameObject bullet = Instantiate (bulletTemplate, transform.position, transform.rotation) as GameObject;
//			bullet.transform.LookAt (player.transform.position);
//			bullet.GetComponent<Rigidbody> ().AddForce (transform.forward * 10, ForceMode.Impulse);
			laser.StopCoroutine ("FireLaser");
			laser.StartCoroutine ("FireLaser");
			StartCoroutine (AttackSoundFire ());
		}
	}
		
	IEnumerator AttackSoundFire () {
		yield return new WaitForSeconds (0.5f);
		weaponAS.Play();
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
			player.GetComponent<Player_Script> ().enemyList.Remove (this.gameObject);
			if (!dead) {
				dead = true;
//				GameObject g = Instantiate (deathElite, transform.position, transform.rotation) as GameObject;
				//			g.GetComponent<Rigidbody> ().velocity = rb.velocity;
				Instantiate (deathElite, transform.position, transform.rotation);
				deathAS.PlayOneShot (deathAS.clip);
				Destroy (this.gameObject);
			}
		}
	}

	void DestroyElite () {
		
	}
}
