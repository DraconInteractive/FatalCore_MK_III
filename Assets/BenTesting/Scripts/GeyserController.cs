using UnityEngine;
using System.Collections;

public class GeyserController : MonoBehaviour 
{
	//NOTES TO BEN

	//Just going to make this a bit more visual for the vid, simple touch ups. Ill comment so you can get rid of if you want

	//geyser spawn references
	public Transform[] spawnPoints;
	public GameObject geyser;

	float timer = 4f;

	//*Adding a current point so it doesnt spawn in the same place
	int currentpoint;

	// Use this for initialization
//	void Start () 
//	{
//	
//	}
	
	// Update is called once per frame
	void Update () 
	{
		//start timer
		timer -= Time.deltaTime;

		//if timer <= 0
		if (timer <= 0)
		{
			//spawn geyser()
			SpawnGeyser ();

			//reset timer
			timer = 4f;
		}
	}

	public void SpawnGeyser()
	{
		//instantiate geyser at random location
		//*adding random int to check for current point. Note, doing it this way means it will infinite loop if there is only one spawn point, so dont do that. 
		int random = Random.Range (0, spawnPoints.Length);
		while (random == currentpoint) {
			random = Random.Range (0, spawnPoints.Length);
		}
		currentpoint = random;
		Instantiate (geyser, spawnPoints [currentpoint].position, Quaternion.identity);
	}
}

//Xblivior
