using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class MenuScript : MonoBehaviour {

	public GameObject mainMenu;
	public GameObject cheatsMenu;
	public GameObject startMenu;
	public GameObject levelSelect;

	public Button levelOneButton, levelTwoButton, levelThreeButton, levelFourButton;

	public bool startMenuActive;

	// Use this for initialization
	void Start () {
		startMenuActive = true;
		mainMenu.SetActive (false);
		cheatsMenu.SetActive (false);
		levelSelect.SetActive (false);
		startMenu.SetActive (true);

		levelOneButton.onClick.AddListener (() => SceneManager.LoadScene ("Level 1 Jamo"));
		levelTwoButton.onClick.AddListener (() => SceneManager.LoadScene ("Eugene Level 2 Testing"));
		levelThreeButton.onClick.AddListener (() => SceneManager.LoadScene("Level 3"));
		levelFourButton.onClick.AddListener (() => SceneManager.LoadScene("Level 4 Boss"));

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
		levelSelect.SetActive (false);
	}

	public void LoadCheatsMenu(){
		mainMenu.SetActive (false);
		cheatsMenu.SetActive (true);
	}

	public void LoadOptionsMenu(){

	}

	public void StartGame(){
		mainMenu.SetActive (false);
		levelSelect.SetActive (true);

	}

	public void Quit(){
		Application.Quit();
	}

	public void LoadLevel1(){
		SceneManager.LoadScene (1);
	}

	public void LoadLevel2(){
		SceneManager.LoadScene (2);
	}

	public void LoadLevel3(){
		SceneManager.LoadScene (3);
	}

	public void LoadLevel4(){
		SceneManager.LoadScene (4);
	}
}
