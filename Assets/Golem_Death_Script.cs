using UnityEngine;
using System.Collections;

public class Golem_Death_Script : MonoBehaviour {
	public GameObject[] children;
	private Rigidbody[] childbodies;
	private Material[] childmats;

	public float deathForce, upMod, forwardMod;
	// Use this for initialization
	void Start () {
		childbodies = new Rigidbody[children.Length];
		childmats = new Material[children.Length];
		for (int i = 0; i < children.Length; i++) {
			childbodies [i] = children [i].GetComponent<Rigidbody> ();
		}
		foreach (Rigidbody r in childbodies) {
			r.AddExplosionForce (deathForce, transform.position + transform.forward * forwardMod, 10, upMod, ForceMode.Impulse);
		}
	}
	

}
