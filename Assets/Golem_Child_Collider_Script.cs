using UnityEngine;
using System.Collections;

public class Golem_Child_Collider_Script : MonoBehaviour {

	public AI_Elite_01_Script elite;

	public bool frame;

	public void PassDamage(int damage) {
		elite.DamageAI (damage);
		StartCoroutine (OnHit ());
	}

	IEnumerator OnHit () {
		Renderer r;
		if (frame) {
			r = transform.parent.gameObject.GetComponent<Renderer> ();
		} else {
			r = GetComponent<Renderer> ();
		}

		Color colorRef = Color.red;
		Color saveC = r.material.color;

		r.material.color = colorRef;

		yield return new WaitForSeconds (0.25f);
		r.material.color = saveC;

		yield break;
	}
}
