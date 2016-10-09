using UnityEngine;
using System.Collections;

public class AIScript : MonoBehaviour {

	public int currentHealth, maxHealth;
	public float speed;
	public bool inCombat;
	public Rigidbody rb;
	public GameObject playerObj;
	public ParticleSystem ps;
	public bool hasPS;

	void Awake () {
		rb = GetComponent<Rigidbody> ();
		ps = GetComponent<ParticleSystem> ();
		if (GetComponent<ParticleSystem>()){
			hasPS = true;
		} else {
			hasPS = false;
		}
	}
	// Use this for initialization
	void Start () {
		StartFunction ();
	}

	public void StartFunction(){
		playerObj = Player_Script.playerObj;

		currentHealth = maxHealth;
	}

	void OnCollisionEnter(Collision col){
		if (col.gameObject.tag == "Bullet"){
			print ("Bullet hit " + gameObject.name);
			Damage (col.gameObject.GetComponent<BulletScript>().damage);
			HealthCheck();
		}
	}

	public void Damage(int damage){
		currentHealth -= damage;
	}

	public void Heal(int healAmount){
		currentHealth += healAmount;
	}

	public void HealthCheck(){
		if (currentHealth <= 0){
			if (hasPS){
				ps.Emit (30);
				Invoke ("Die", 2);
			}
		}
	}

	public void Die(){
		Destroy (this.gameObject);
	}

		


}
