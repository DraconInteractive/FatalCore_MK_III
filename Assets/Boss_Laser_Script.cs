using UnityEngine;
using System.Collections;

public class Boss_Laser_Script : MonoBehaviour {
	LineRenderer laser;
	public int currentHealth, maxHealth;

	public Material lasermat;

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

			if (hit.collider.tag == "Player") {
				hit.collider.gameObject.GetComponent<Player_Script> ().DamagePlayer (10);
			}
		}
	}

	public void DamageAI (int damage) {
		currentHealth -= damage;
		if (currentHealth <= 0) {
			StartCoroutine (ShieldDeath ());
		}
	}

	IEnumerator ShieldDeath () {
		Renderer r = GetComponent<Renderer> ();
		r.material = lasermat;

		float a = 1;

		while (a > 0) {
			
			Color colorRef = r.material.color;
			colorRef.a -= Time.deltaTime;
			colorRef.a = Mathf.Clamp (colorRef.a, 0.0f, 1.0f);
			r.material.color = colorRef;

			a -= Time.deltaTime;
			yield return null;
		}
		Invoke ("Death", 1);
		yield break;
	}

	void Death () {
		Destroy (this.gameObject);
	}
}
