using UnityEngine;
using System.Collections;

public class Boss_Laser_Script : MonoBehaviour {
	LineRenderer laser;
	public int currentHealth, maxHealth;

	void Awake () {
		laser = transform.GetChild (0).gameObject.GetComponent<LineRenderer>();
	}
	// Use this for initialization
	void Start () {
		currentHealth = maxHealth;
		Ray ray = new Ray (laser.transform.position, transform.forward);

		laser.SetPosition (0, ray.origin);
		laser.SetPosition (1, ray.GetPoint (1000));

	}
	
	// Update is called once per frame
	void Update () {
		Ray ray = new Ray (laser.transform.position, transform.forward);

		laser.SetPosition (0, ray.origin);

		RaycastHit hit;
		if (Physics.Raycast(ray, out hit, Mathf.Infinity)) {
			laser.SetPosition (1, hit.point);
		}
	}

	public void DamageAI (int damage) {
		currentHealth -= damage;
		if (currentHealth <= 0) {
			Destroy (this.gameObject);
		}
	}
}
