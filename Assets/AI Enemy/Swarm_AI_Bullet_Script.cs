using UnityEngine;
using System.Collections;

public class Swarm_AI_Bullet_Script : MonoBehaviour {
	public float lifeTime;
	public int damage;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		lifeTime -= Time.deltaTime;
		if (lifeTime < 0){
			Destroy (this.gameObject);
		}
	}

	void OnTriggerEnter (Collider col) {
		if (col.gameObject.tag == "Player"){
			print ("HitPlayer");
			col.gameObject.GetComponent<Player_Script> ().DamagePlayer (damage);
			GetComponent<SphereCollider> ().enabled = false;
			GetComponent<MeshRenderer> ().enabled = false;
		}
	}
}
