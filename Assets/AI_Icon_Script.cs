using UnityEngine;
using System.Collections;

public class AI_Icon_Script : MonoBehaviour {
	private GameObject player;
	private Player_Script ps;
	// Use this for initialization
	void Start () {
		player = Player_Script.playerObj;
		ps = player.GetComponent<Player_Script> ();
	}
	
	// Update is called once per frame
	void Update () {
		transform.LookAt (player.transform.position);
	}
}
