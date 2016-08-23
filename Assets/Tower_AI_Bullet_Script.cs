using UnityEngine;
using System.Collections;

public class Tower_AI_Bullet_Script : MonoBehaviour {
	public float lifeTime;
	public int damage;
	private Rigidbody rb;
	private GameObject player;

	void Start () {
		rb = GetComponent<Rigidbody> ();
		player = Player_Script.playerObj;
	}
	// Update is called once per frame
	void Update () {
		lifeTime -= Time.deltaTime;
		if (lifeTime < 0){
			Destroy (this.gameObject);
		}

		rb.AddForce ((player.transform.position - transform.position).normalized * 1 * Time.deltaTime, ForceMode.VelocityChange);

	}

//	private IEnumerator FireUpdate () {
//		while (true) {
//			if (Vector3.Distance (swarmTarget.transform.position, transform.position) < fireRadius) {
//				GameObject bullet = Instantiate (bulletTemplate, transform.position, Quaternion.identity) as GameObject;
//				bullet.GetComponent<Rigidbody> ().AddForce ((swarmTarget.transform.position - transform.position).normalized * 10, ForceMode.VelocityChange);
//			}
//			yield return new WaitForSeconds (0.5f);
//		}
//	}

	void OnTriggerEnter (Collider col) {
		if (col.gameObject.tag == "Player"){
			print ("HitPlayer");
			col.gameObject.GetComponent<Player_Script> ().DamagePlayer (damage);
			Destroy (this.gameObject);
		}
	}
}
