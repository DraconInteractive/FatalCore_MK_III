using UnityEngine;
using System.Collections;

public class MC_PRESENTATION_SCRIPT : MonoBehaviour {
	public GameObject looktarget;

	// Update is called once per frame
	void Update () {
		transform.LookAt (looktarget.transform.position);
	}
}
