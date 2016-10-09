using UnityEngine;
using System.Collections;

public class AI_Tower_Script : MonoBehaviour {

	private GameObject turretPoint;
	private GameObject player;

	public GameObject bulletTemplate;

	private bool cooling;
	private float coolingTimer;
	public float fireRadius, fireRate;
	// Use this for initialization
	void Start () {
		turretPoint = transform.GetChild (0).gameObject;
		player = Player_Script.playerObj;
	}

	void Update () {
		Fire ();
	}

	private void Fire () {
		if (!cooling) {
			if (Vector3.Distance(player.transform.position, transform.position) < fireRadius) {
				GameObject bullet = Instantiate (bulletTemplate, turretPoint.transform.position, Quaternion.identity) as GameObject;
				bullet.GetComponent<Rigidbody> ().AddForce (/*(player.transform.position - transform.position).normalized * 10*/ transform.forward * 10, ForceMode.VelocityChange);
				cooling = true;
				coolingTimer = fireRate;
			}
		}

		coolingTimer -= Time.deltaTime;
		if (coolingTimer <= 0) {
			cooling = false;
		}
	}

	public void DamageAI (int damage) {
		Destroy (this.gameObject);
	}
		
}
