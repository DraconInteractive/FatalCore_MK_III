using UnityEngine;
using System.Collections;

public class AI_Tower_Script : MonoBehaviour {

	private GameObject turretPoint;
	private GameObject player;

	public GameObject bulletTemplate;

	private bool cooling;
	private float coolingTimer;
	public float fireRadius, fireRate;

	float currentHealth;
	public float maxHealth;

	public Animator anim;
	public Tower_Laser laser;

	float timeDeviation;
	// Use this for initialization
	void Start () {
		turretPoint = transform.GetChild (0).gameObject;
		player = Player_Script.playerObj;
		currentHealth = maxHealth;

		timeDeviation = Random.Range (0.0f, 2.0f);
	}

	void Update () {
		Fire ();
	}

	private void Fire () {
		if (!cooling) {
			if (Vector3.Distance(player.transform.position, transform.position) < fireRadius) {
//				GameObject bullet = Instantiate (bulletTemplate, turretPoint.transform.position, Quaternion.identity) as GameObject;
//				bullet.GetComponent<Rigidbody> ().AddForce (/*(player.transform.position - transform.position).normalized * 10*/ (transform.forward + (transform.up * Random.Range (-0.25f, 0.25f)) + (transform.right * Random.Range(-0.25f, 0.25f))) * 25, ForceMode.VelocityChange);
				laser.StopCoroutine ("FireLaser");
				laser.StartCoroutine ("FireLaser");
				cooling = true;
				coolingTimer = fireRate;
			}
		}

		coolingTimer -= Time.deltaTime;
		if (coolingTimer <= 0 + timeDeviation) {
			timeDeviation = 0;
			cooling = false;
		}
	}

	public void DamageAI (int damage) {
		currentHealth -= damage;

		if (currentHealth <= 0) {
			player.GetComponent<Player_Script> ().UpdateEnemyCounter ();
			Destroy (this.gameObject);
		}
	}
		
}
