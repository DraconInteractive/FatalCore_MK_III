using UnityEngine;
using System.Collections;

public class CrystalController : MonoBehaviour 
{
	//Health
	public int maxHP = 10;
	public int currentHP;

	//spawn references
	public GameObject tinyCrystal;
	public int maxNumber = 5;
	int currentNumber;

	float timer = 1;
	// Use this for initialization
	void Start () 
	{
		//make currentHP = maxHP
		currentHP = maxHP;
	}
	
	// Update is called once per frame
	void Update () 
	{
		timer -= Time.deltaTime;

		if (timer <= 0)
		{
			TakeDamage(1);

			timer = 1;
		}

	}

	public void TakeDamage(int damage)
	{
		//take damage from currentHP
		currentHP -= damage;

		//if currentHP hits 0
		if (currentHP <= 0)
		{
			//set currentNumber to 0
			currentNumber = 0;

			//while currentNumber < maxNumber
			while(currentNumber < maxNumber)
			{
				//instantiate tiny crystals at the crystal
				Instantiate (tinyCrystal, transform.position, transform.rotation);

				//increase currentNumber
				currentNumber ++;
			}

			//destroy crystal
			Destroy (gameObject);
		}
	}
}


//Xblivior