using UnityEngine;
using System.Collections;

public class AI_01_Script : AIScript {


	void FixedUpdate () {
		MoveToPlayer ();
	}

	public void MoveToPlayer(){
		if (Vector3.Distance(playerObj.transform.position, transform.position) > 5){
			transform.LookAt (playerObj.transform.position);
			rb.AddForce (transform.forward * speed * Vector3.Distance(transform.position, playerObj.transform.position) * Time.deltaTime);
		} else {
			rb.velocity = Vector3.Lerp(rb.velocity, Vector3.zero, 0.05f);
		}
	}

	//	private Vector3 FindPointToMove(){
	//		ptm = new Vector3 (Random.Range (transform.position.x - 10, transform.position.x + 10), Random.Range (transform.position.y - 10, transform.position.y + 10), Random.Range (transform.position.z - 10, transform.position.z + 10));
	//		RaycastHit hit;
	//		if (Physics.Raycast(transform.position, (ptm - transform.position), out hit, 20)){
	//			ptm = hit.point - (hit.point - transform.position);
	//		}
	//		print ("returning point");
	//		return ptm;
	//	}
	//
}
