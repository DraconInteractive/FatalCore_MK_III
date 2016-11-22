using UnityEngine;
using System.Collections;
using Kino;

public class Main_Camera_Script : MonoBehaviour {

	public Player_Script ps;

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
