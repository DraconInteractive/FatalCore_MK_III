using UnityEngine;
using System.Collections;

public class Golem_Death_Fade_Out : MonoBehaviour {
	
	public float fadeOutDelay, fadeOutTime;

	void Start () {
		StartCoroutine (FadeOut ());
	}
	IEnumerator FadeOut () {
		Renderer[] renderers = GetComponentsInChildren<Renderer> ();

		yield return new WaitForSeconds (fadeOutDelay);
		float a = fadeOutTime;

		while (a > 0) {
			for (int i = 0; i < renderers.Length; i++) {
				Color colorRef = renderers [i].material.color;
				colorRef.a -= Time.deltaTime / fadeOutTime;
				colorRef.a = Mathf.Clamp (colorRef.a, 0.0f, 1.0f);
				renderers [i].material.color = colorRef;
			}
			a -= Time.deltaTime;
			yield return null;
		}
		Invoke ("Death", 1);
		yield break;


	}

	void Death () {
		Destroy (this.gameObject);
	}
}
