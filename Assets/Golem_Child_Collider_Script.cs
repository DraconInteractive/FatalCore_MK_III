using UnityEngine;
using System.Collections;

public class Golem_Child_Collider_Script : MonoBehaviour {

	public AI_Elite_01_Script elite;

	public void PassDamage(int damage) {
		elite.DamageAI (damage);
	}
}
