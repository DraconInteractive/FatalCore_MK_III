using UnityEngine;
using System.Collections;

public class SporeController : MonoBehaviour 
{
	public ParticleSystem spores;
	float timer;

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		timer -= Time.deltaTime;

	}
}
