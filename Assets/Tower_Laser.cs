using UnityEngine;
using System.Collections;

public class Tower_Laser : MonoBehaviour {
	LineRenderer line;
	public AI_Tower_Script tower;
	public Player_Script player;

	void Awake () {
		line = GetComponent<LineRenderer> ();
		line.enabled = false;
		tower.laser = this.gameObject.GetComponent<Tower_Laser> ();
	}
	// Use this for initialization
	void Start () {
		player = Player_Script.playerObj.GetComponent<Player_Script> ();
	}

	public IEnumerator FireLaser () {
		line.enabled = true;

		Ray ray = new Ray (transform.position, transform.forward);

		line.SetPosition (0, ray.origin);
		line.SetPosition (1, ray.GetPoint (50));

		float t = 0;

		while (t < 1) {
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit, 50)) {
				if (hit.collider.tag == "Player") {
					player.DamagePlayer (20);
					line.enabled = false;
					yield break;
				}
			}
			t += Time.deltaTime;
			yield return null;
		}

		line.enabled = false;
		yield break;
	}

//	RaycastHit hit;
//	if (Physics.Raycast(transform.position, transform.forward, out hit, 100)) {
//		if (hit.collider.tag == "Player") {
//			player.DamagePlayer (20);
//		}
//	}
}
