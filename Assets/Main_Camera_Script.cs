using UnityEngine;
using System.Collections;
using Kino;

public class Main_Camera_Script : MonoBehaviour {

	Player_Script ps;

	void Awake () {
		ps = Player_Script.playerObj.GetComponent<Player_Script> ();
	}
	// Use this for initialization
	void Start () {
		ps.ag = GetComponent<AnalogGlitch> ();
		ps.dg = GetComponent<DigitalGlitch> ();
	}

	void OnEnable () {
		ps.ag = GetComponent<AnalogGlitch> ();
		ps.dg = GetComponent<DigitalGlitch> ();
	}
}
