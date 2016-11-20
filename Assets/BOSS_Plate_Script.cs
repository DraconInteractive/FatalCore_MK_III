using UnityEngine;
using System.Collections;

public class BOSS_Plate_Script : MonoBehaviour {
	public float currentHealth;
	public float maxHealth, stepTarget;
	GameObject player;
	public GameObject bulletTemplate;
	float stepTimer, stMod;
	// Use this for initialization
	void Start () {
		currentHealth = maxHealth;
		player = Player_Script.playerObj;
		stMod = Random.Range (0.5f, 1.0f);
	}

	public void Damage (int amount) {
		currentHealth -= amount;
		if (currentHealth <= 0) {
			Die ();
		}
	}

	void Update () {
		stepTimer += Time.deltaTime;
		if (stepTimer >= stepTarget + stMod) {
			Step ();
			stepTimer = 0;
		}
	}

	void Step () {
		FireBullet ();
		stMod = Random.Range (0.5f, 1.0f);
	}

	void FireBullet () {
		GameObject bullet = Instantiate (bulletTemplate, transform.position, Quaternion.identity) as GameObject;
		bullet.transform.LookAt (player.transform.position);
		bullet.GetComponent<Rigidbody> ().AddForce (bullet.transform.forward * 1 * Time.deltaTime, ForceMode.Impulse);
	}

	public void Die () {
		this.gameObject.SetActive (false);
	}
}
