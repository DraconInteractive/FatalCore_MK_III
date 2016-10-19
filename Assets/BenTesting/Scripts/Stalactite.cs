using UnityEngine;
using System.Collections;

public class Stalactite : MonoBehaviour 
{
	public bool playerClose = false;

	public int damage;

	//timers
	float dropTimer;
	float lifeTimer = 2;

	//reference to rigidbody
	public Rigidbody rB;

	//player script reference
	Player_Script ps;

	// Use this for initialization
	void Start () 
	{
		//get player scripts
		ps = Player_Script.playerObj.GetComponent<Player_Script> ();
	
	}

	void Awake()
	{
		//set random drop time
		dropTimer = Random.Range(1f, 2f);
	}
	
	// Update is called once per frame
	void Update () 
	{
		//if playerClose == true
		if (playerClose)
		{
			//start drop timer
			dropTimer -= Time.deltaTime;

			//if droptimer hits 0
			if (dropTimer <= 0)
			{
				//enable gravity and disable kinematic
				rB.useGravity = true;
				rB.isKinematic = false;

				//start life timer
				lifeTimer -= Time.deltaTime;
			}
		}

		//if life timer hits 0
		if (lifeTimer <= 0)
		{
			//destroy gameobject
			Destroy(gameObject);
		}
	}

	void OnTriggerEnter(Collider other)
	{
		//if it hits the player
		if (other.gameObject == Player_Script.playerObj)
		{
			//damage player
			ps.DamagePlayer (damage);
		}
	}
}

//Xblivior
