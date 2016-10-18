using UnityEngine;
using System.Collections;

public class BulletScript : MonoBehaviour {
	public float lifeTime, sparkleTime;
	public int damage;
	private bool dying;
	private ParticleSystem ps;
	// Use this for initialization
	void Start () {
		ps = GetComponent<ParticleSystem>();
		dying = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (dying){
			lifeTime -= Time.deltaTime;
		}

		if (lifeTime <= 0){
			Destroy(this.gameObject);
		}
	}

	void OnCollisionEnter(Collision col){
		if (col.gameObject.tag != "Bullet" || col.gameObject.tag != "Player"){
			Sparkles();
		} 

		if (col.gameObject.tag == "Swarm" || col.gameObject.tag == "Tower" || col.gameObject.tag == "Elite"){
			ApplyDamage (col.gameObject);
		}
	}

	private void Sparkles(){
		dying = false;
		ps.Emit(50);
		Invoke("Die", sparkleTime);
	}

	private void Die(){
		Destroy(this.gameObject);
	}

	private void ApplyDamage(GameObject g){
		switch (g.tag) {
		case "Swarm":
			g.GetComponent<Swarm_Script_02> ().DamageAI (damage);
			break;
		case "Elite":
			g.GetComponent<AI_Elite_01_Script> ().DamageAI (damage);
			break;
		case "Tower":
			g.GetComponent<AI_Tower_Script> ().DamageAI (damage);
			break;
		}
	}
		
}
