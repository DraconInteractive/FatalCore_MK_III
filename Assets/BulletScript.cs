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

		if (col.gameObject.tag == "Swarm"){
			Kill (col.gameObject);
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

	private void Kill(GameObject g){
		Destroy (g);
	}
		
}
