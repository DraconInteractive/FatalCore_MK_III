using UnityEngine;
using System.Collections;

public class StalactiteController : MonoBehaviour 
{
	public GameObject[] stalactites;
	public float checkDistance = 10;

	GameObject player;
	float checkTimer = 0.5f;

	// Use this for initialization
	void Start () 
	{
		//set player object
		player = Player_Script.playerObj;
	}
	
	// Update is called once per frame
	void Update () 
	{
		//start timer
		checkTimer -= Time.deltaTime;

		//if timer hits 0
		if (checkTimer <= 0)
		{
			//call Drop
			Drop();

			//reset timer
			checkTimer = 0.5f;
		}
			
	}

	public void Drop()
	{
		//if distance between player and controller is <= checkDistance
		if(Vector3.Distance(player.transform.position, transform.position) <= checkDistance)
		{
			//int
			int i = 0;

			//while i < number of stalactites
			while (i < stalactites.Length)
			{
				//tell stalactite[i] to set playerclose to true
				stalactites[i].GetComponent<Stalactite>().playerClose = true;

				//add to i
				i++;
			}

			//destroy controller
			Destroy(gameObject);

		}	

	}
}

//Xblivior
