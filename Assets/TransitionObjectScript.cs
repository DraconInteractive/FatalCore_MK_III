using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class TransitionObjectScript : MonoBehaviour {


	// Use this for initialization
	void Start () {
	
	}

	void OnCollisionEnter (Collision col) {
		if (col.gameObject.tag == "Player") {
			switch (SceneManager.GetActiveScene().name)
			{
			case "Level 1":
				SceneManager.LoadScene ("Settlement One");
				break;
			case "Level 2":
				SceneManager.LoadScene ("Settlement Two");
				break;
			case "Level 3":
				SceneManager.LoadScene ("Settlement Three");
				break;
			case "Settlement One":
				SceneManager.LoadScene ("Level 2");
				break;
			case "Settlement Two":
				SceneManager.LoadScene ("Level 3");
				break;
			case "Settlement Three":
				SceneManager.LoadScene ("Level 4");
				break;
			}
		}
	}
}
