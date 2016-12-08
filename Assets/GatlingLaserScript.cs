using UnityEngine;
using System.Collections;

public class GatlingLaserScript : MonoBehaviour {
	LineRenderer line;
	Player_Script player;
	public int damage;
	public bool left, right;

	void Awake () {
		line = GetComponent<LineRenderer> ();
		line.enabled = false;
	}

	void Start () {
		player = Player_Script.playerObj.GetComponent<Player_Script> ();
		if (left) {
			player.gatLeftLaser = GetComponent<GatlingLaserScript> ();
		} else if (right) {
			player.gatRightLaser = GetComponent<GatlingLaserScript> ();
		}

	}

	public IEnumerator FireLaser () {
		line.enabled = true;

		Ray ray = new Ray (transform.position, transform.forward);

		line.SetPosition (0, ray.origin);
		line.SetPosition (1, ray.GetPoint (100));

		float t = 0;

		while (t < 0.015f) {
			ray = new Ray (transform.position, transform.forward);

			line.SetPosition (0, ray.origin);
			line.SetPosition (1, ray.GetPoint (100));
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit, 100)) {
				GameObject g = hit.collider.gameObject;
				switch (g.tag) {
				case "Swarm":
					g.GetComponent<Swarm_Script_02> ().DamageAI (damage);
					break;
				case "Elite":
					g.GetComponent<AI_Elite_01_Script> ().DamageAI (damage);
					break;
				case "EliteChild":
					g.GetComponent<Golem_Child_Collider_Script> ().PassDamage (damage);
					break;
				case "Tower":
					g.GetComponent<AI_Tower_Script> ().DamageAI (damage);
					break;
				case "BossPlate":
					g.GetComponent<BOSS_Plate_Script>().Damage(damage);
					break;
				}
			}
			t += Time.deltaTime;
			yield return null;
		}

		line.enabled = false;
		yield break;
	}
}
