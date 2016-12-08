using UnityEngine;
using System.Collections;

public class RailLaserScript : MonoBehaviour {
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
			player.railLeftLaser = GetComponent<RailLaserScript> ();
		} else if (right) {
			player.railRightLaser = GetComponent<RailLaserScript> ();
		}

	}

	public IEnumerator FireLaser () {

		yield return new WaitForSeconds (1f);
		line.enabled = true;

		Ray ray = new Ray (transform.position, transform.forward);

		line.SetPosition (0, ray.origin);
		line.SetPosition (1, ray.GetPoint (100));

		float t = 0;
		bool hasHit = false;
		while (t < 1) {
			ray = new Ray (transform.position, transform.forward);

			line.SetPosition (0, ray.origin);
			line.SetPosition (1, ray.GetPoint (100));
			RaycastHit hit;
			if (!hasHit) {
				if (Physics.Raycast(ray, out hit, 100)) {
					GameObject g = hit.collider.gameObject;
					switch (g.tag) {
					case "Swarm":
						g.GetComponent<Swarm_Script_02> ().DamageAI (damage);
						hasHit = true;
						break;
					case "Elite":
						g.GetComponent<AI_Elite_01_Script> ().DamageAI (damage);
						hasHit = true;
						break;
					case "EliteChild":
						g.GetComponent<Golem_Child_Collider_Script> ().PassDamage (damage);
						hasHit = true;
						break;
					case "Tower":
						g.GetComponent<AI_Tower_Script> ().DamageAI (damage);
						hasHit = true;
						break;
					case "BossPlate":
						g.GetComponent<BOSS_Plate_Script>().Damage(damage);
						hasHit = true;
						break;
					}
				}
			}

			t += Time.deltaTime;
			yield return null;
		}

		line.enabled = false;
		yield break;
	}
}
