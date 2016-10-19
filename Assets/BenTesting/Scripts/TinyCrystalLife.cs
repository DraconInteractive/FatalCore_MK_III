using UnityEngine;
using System.Collections;

public class TinyCrystalLife : MonoBehaviour 
{
	float lifeTimer = 2;
	
	// Update is called once per frame
	void Update () 
	{
		//activate timer
		lifeTimer -= Time.deltaTime;

		//if timer hits 0
		if (lifeTimer <= 0)
		{
			//destroy object
			Destroy(gameObject);
		}
	}
}

//Xblivior
