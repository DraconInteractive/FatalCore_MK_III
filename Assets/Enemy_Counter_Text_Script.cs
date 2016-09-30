using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Enemy_Counter_Text_Script : MonoBehaviour {
	private Player_Script ps;
	// Use this for initialization
	void Start () {
		Player_Script ps = Player_Script.playerObj.GetComponent<Player_Script> ();
		ps.enemyCounterText = this.gameObject.GetComponent<Text> ();
	}
}
