using UnityEngine;
using System.Collections;

public class SporeController : MonoBehaviour 
{
	public ParticleSystem spores;
	public GameObject target;

	public float checkDistance;
	float checkTimer = 0.5f;

	 public GameObject player;

	// Use this for initialization
	void Start () 
	{
		//set player object
		//player = Player_Script.playerObj;

	}
	
	// Update is called once per frame
	void Update () 
	{
		checkTimer -= Time.deltaTime;
		if (checkTimer <= 0)
		{
			//if distance between player and controller is <= checkDistance
			if(Vector3.Distance(player.transform.position, transform.position) <= checkDistance)
			{
				spores.Play();
				target = player.gameObject;
			}

			else 
			{
				spores.Stop();
				target = null;
			}
		}

//		if (target != null)
//		{
//			transform.LookAt(target.transform);
//		}

	}
}

//Xblivior
