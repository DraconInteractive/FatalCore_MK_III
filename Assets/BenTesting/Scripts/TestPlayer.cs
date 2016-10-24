using UnityEngine;
using System.Collections;

public class TestPlayer : MonoBehaviour 
{
	float speed = 5;
	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		transform.Translate(Vector3.forward * speed * Time.deltaTime);
	}
}
