using UnityEngine;
using System.Collections;

public class StalactiteController : MonoBehaviour 
{
	public GameObject[] stalactites;

	Collider[] hitColliders;
	public float checkRadius = 10;

	float checkTimer = 0.5f;

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		checkTimer -= Time.deltaTime;

		if (checkTimer <= 0)
		{
			Check();
			//checkTimer = 0.5f;
		}
	}

	public void Check()
	{
		//do overlapsphere
		//check for player
		//if inside set stelectites playerClose = true
		hitColliders = Physics.OverlapSphere(transform.position, checkRadius);

		int i = 0;
		while (i < stalactites.Length)
		{
			stalactites[i].GetComponent<Stalactite>().playerClose = true;
			i++;
		}

		Destroy(gameObject);
			
	}
}
