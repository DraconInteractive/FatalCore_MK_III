using UnityEngine;
using System.Collections;

public class Boss_Core_Script : MonoBehaviour {

	public int currentHealth, maxHealth;

	private ParticleSystem ps;

	void Awake () {
		ps = GetComponent<ParticleSystem> ();
	}
	void Start () {
		ps.Stop ();
	}
	public void DamageCore (int damage) {
		currentHealth -= damage;
		if (currentHealth <= 0) {
			StartCoroutine (Die ());
		}
	}

	IEnumerator Die () {
		ps.Play ();
//		public float fadeOutDelay, fadeOutTime;

//		IEnumerator FadeOut () {
//			Renderer[] renderers = GetComponentsInChildren<Renderer> ();
//
//			yield return new WaitForSeconds (fadeOutDelay);
//			float a = fadeOutTime;
//
//			while (a > 0) {
//				for (int i = 0; i < renderers.Length; i++) {
//					Color colorRef = renderers [i].material.color;
//					colorRef.a -= Time.deltaTime / fadeOutTime;
//					colorRef.a = Mathf.Clamp (colorRef.a, 0.0f, 1.0f);
//					renderers [i].material.color = colorRef;
//				}
//				a -= Time.deltaTime;
//				yield return null;
//			}
//			Invoke ("Death", 1);
//			yield break;
//
//
//		}
		Renderer render = GetComponent<Renderer> ();

		float a = 2;

		while (a > 0) {
			Color colorRef = Color.white;
			render.material.color = Color.Lerp (colorRef, render.material.color, 0.25f);
			a -= Time.deltaTime;
			yield return null;
		}
		yield return new WaitForSeconds (10);
		Player_Script.playerObj.GetComponent<Player_Script> ().StartCoroutine ("CoreDeath");
		yield break;
	}
}
