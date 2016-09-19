using UnityEngine;
using System.Collections;

public class AI_Tower_Script : MonoBehaviour {

	private GameObject turretPoint;
	private GameObject player;

	public GameObject bulletTemplate;

	public float fireRadius;
	// Use this for initialization
	void Start () {
		turretPoint = transform.GetChild (0).gameObject;
		player = Player_Script.playerObj;

		StartCoroutine ("DoYoThang");
	}

//	void OnDrawGizmos () {
//		Gizmos.color = Color.green;
//		Gizmos.DrawWireSphere (transform.position, fireRadius);
//	}

	private IEnumerator DoYoThang () {
		while (true) {
			if (Vector3.Distance(player.transform.position, transform.position) < fireRadius) {
				GameObject bullet = Instantiate (bulletTemplate, turretPoint.transform.position, Quaternion.identity) as GameObject;
				bullet.GetComponent<Rigidbody> ().AddForce (/*(player.transform.position - transform.position).normalized * 10*/ transform.forward * 10, ForceMode.VelocityChange);
			}

			yield return new WaitForSeconds (0.3f);
		}
	}
		
}
