using UnityEngine;
using System.Collections;

public class MenuScript : MonoBehaviour {

	public GameObject mainMenu;
	public GameObject cheatsMenu;
	public GameObject startMenu;

	public bool startMenuActive;

	// Use this for initialization
	void Start () {
		startMenuActive = true;
		mainMenu.SetActive (false);
		cheatsMenu.SetActive (false);
		startMenu.SetActive (true);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Return)) {
			if (startMenuActive == true) {
				LoadMainMenu ();
			}
		}
	}

	public void LoadMainMenu(){
		startMenu.SetActive (false);
		cheatsMenu.SetActive (false);
		mainMenu.SetActive (true);
		startMenuActive = false;
	}

	public void LoadCheatsMenu(){
		mainMenu.SetActive (false);
		cheatsMenu.SetActive (true);
	}

	public void LoadOptionsMenu(){

	}

	public void StartGame(){

	}

	public void Quit(){
		Application.Quit();
	}
}
