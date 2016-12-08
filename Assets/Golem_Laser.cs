using UnityEngine;
using System.Collections;

public class Golem_Laser : MonoBehaviour {
	LineRenderer line;
	public AI_Elite_01_Script elite;
	public Player_Script player;
	void Awake () {
		line = GetComponent<LineRenderer> ();
		line.enabled = false;
		elite.laser = this.gameObject.GetComponent<Golem_Laser> ();
	}

	void Start () {
		player = Player_Script.playerObj.GetComponent<Player_Script> ();
	}

	IEnumerator FireLaser () {
		yield return new WaitForSeconds (1.25f);
		line.enabled = true;

		Ray ray = new Ray (transform.position, transform.forward);

		line.SetPosition (0, ray.origin);
		line.SetPosition (1, ray.GetPoint (100));

		RaycastHit hit;
		if (Physics.Raycast(ray, out hit, 100)) {
			if (hit.collider.tag == "Player") {
				player.DamagePlayer (20);
			}
		}


		yield return new WaitForSeconds (0.5f);

		line.enabled = false;
		yield break;
	}
}
