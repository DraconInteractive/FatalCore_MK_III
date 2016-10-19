using UnityEngine;
using System.Collections;

public class Geyser : MonoBehaviour 
{
	//geyser height 
	public Transform geyserHeight;

	//set damage
	public int damage;

	//timers
	float timer = 2f;
	float lifeTimer = 10f;

	//player script reference
	Player_Script ps;

	void Start () 
	{
		//get player scripts
		ps = Player_Script.playerObj.GetComponent<Player_Script> ();
	}

	// Update is called once per frame
	void Update () 
	{
		//start timers
		timer -= Time.deltaTime;
		lifeTimer -= Time.deltaTime;

		//if timer <= 0
		if (timer <= 0f)
		{
			//move geyser to geyser hight
			transform.position = Vector3.Lerp (transform.position, geyserHeight.position, 1 * Time.deltaTime);
		}

		//if life timer <= 0
		if (lifeTimer <= 0f)
		{
			// destroy geyser
			Destroy (this.gameObject);
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
