using UnityEngine;
using System.Collections;

public class ST_PRESENTATION_TESTING : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {
		transform.Translate (-Vector3.forward * 10 * Time.deltaTime);
	}
}
