using UnityEngine;
using System.Collections;

public class Golem_Death_Fade_Out : MonoBehaviour {

	public float fadeDelay = 0.0f;
	public float fadeTime = 0.5f;
	public bool fadeOutOnStart = false;
	private bool logInitialFadeSequence = false;

	private Color [] colors;

	IEnumerator Start () {
		yield return new WaitForSeconds (fadeDelay);

		if (fadeOutOnStart) {
			StartCoroutine (FadeOut (fadeTime));
		}
	}

	float MaxAlpha () {
		float maxAlpha = 0.0f;
		Renderer[] rendererObjects = GetComponentsInChildren<Renderer> ();
		foreach (Renderer item in rendererObjects) {
			maxAlpha = Mathf.Max (maxAlpha, item.material.color.a);
		}
		return maxAlpha;
	}

	IEnumerator FadeSequence (float fadingOutTime) {
		
	}
}
