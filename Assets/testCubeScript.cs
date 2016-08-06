using UnityEngine;
using System.Collections;

public class testCubeScript : MonoBehaviour {

	Behaviour halob;
	bool haloOn;
	// Use this for initialization

	void Awake () {
		halob = GetComponent ("Halo") as Behaviour;
	}

	void Start () {
		Invoke ("ToggleHalo", 1);
	}


	private void ToggleHalo(){
		haloOn = !haloOn;
		halob.enabled = haloOn;
		Invoke ("ToggleHalo", 1);
	}
}
