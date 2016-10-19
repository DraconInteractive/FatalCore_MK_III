using UnityEngine;
using System.Collections;

public class GeyserController : MonoBehaviour 
{
	//geyser spawn references
	public Transform[] spawnPoints;
	public GameObject geyser;

	float timer = 4f;

	// Use this for initialization
	void Start () 
	{
	
	}
	
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
		Instantiate (geyser, spawnPoints [Random.Range (0, spawnPoints.Length)].position, Quaternion.identity);
	}
}

//Xblivior
