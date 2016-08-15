using UnityEngine;
using System.Collections;
[RequireComponent (typeof(Rigidbody))]
public class Swarm_AI_Script : MonoBehaviour {

	public float swarmUpdatesPerSecond = 5;
	public float timeSpread = 5;
	public float sightRadius = 500;
	public float sightAngle = 90;
	public float maxAccelleration = 5;

	private Vector3 steerToCentre = Vector3.zero;
	private Vector3 steerToSeperate = Vector3.zero;
	private Vector3 steerToTarget = Vector3.zero;

	private Collider[] neighbourhood;

	public float sTCPriority = 5;
	public float sTSPriority = 3;
	public float targetPriority = 5;
	public int id;

	public GameObject swarmTarget;	public GameObject swarmHome;

	public bool isMovingToTarget = false;

	void Awake () {
		GameObject[] allSwarm = GameObject.FindGameObjectsWithTag ("Swarm");
		for (int i = 0; i < allSwarm.Length; i++){
			allSwarm [i].GetComponent<Swarm_AI_Script> ().id = i;
		}

		swarmTarget = GameObject.FindGameObjectWithTag ("Player");
	}

	void Start () {
		StartCoroutine ("DoSwarm");
	}

	private IEnumerator DoSwarm () {
		while (true) {
			neighbourhood = Physics.OverlapSphere (transform.position, sightRadius);

//			yield return new WaitForSeconds (SpreadUpdates ());

			DoSeperationAndCohesion ();

			yield return new WaitForSeconds (SpreadUpdates ());
		}
	}

	private float SpreadUpdates () {
		float offset = (Random.value - Random.value) / timeSpread;
		return (1 / swarmUpdatesPerSecond) + offset;
	}

	private void DoSeperationAndCohesion () {

		Vector3 swarmDirection = Vector3.zero;
		Vector3 closestSwarmerDirection = Vector3.zero;
		Vector3 swarmCenter = Vector3.zero;

		float shortestDistance = Mathf.Infinity;
		Vector3 closestLocation = transform.position;

		for (int i = 0; i < neighbourhood.Length; i++){
			if (neighbourhood[i].GetComponent<Swarm_AI_Script>()){
				if (neighbourhood[i].GetComponent<Rigidbody>() && neighbourhood[i].GetComponent<Swarm_AI_Script>().id != id){
					swarmDirection += neighbourhood [i].transform.forward;
					swarmCenter += neighbourhood [i].transform.position;
					float testDist = Vector3.Distance (transform.position, neighbourhood [i].transform.position);
					if (testDist < shortestDistance) {
						shortestDistance = testDist;
						closestLocation = neighbourhood [i].transform.position;
					}
				}
			}
		}

		swarmDirection /= neighbourhood.Length - 1;
		swarmCenter /= neighbourhood.Length - 1;

		if (shortestDistance != Mathf.Infinity) {
			steerToSeperate = transform.position - closestLocation;
		} else {
			steerToSeperate = Vector3.zero;
		}

//		Debug.ClearDeveloperConsole ();
		if (isMovingToTarget){
			if (Vector3.Distance(transform.position, swarmTarget.transform.position) < 10){
				steerToTarget = Vector3.Normalize (transform.position - swarmTarget.transform.position) * maxAccelleration;
			} else {
				steerToTarget = Vector3.Normalize (swarmTarget.transform.position - transform.position) * maxAccelleration;
			}

			if (Vector3.Distance(transform.position, swarmTarget.transform.position) > 100) {
				steerToTarget = Vector3.Normalize (swarmHome.transform.position - transform.position) * maxAccelleration;
			}
		}




		steerToCentre = Vector3.Normalize (swarmCenter - transform.position) * maxAccelleration;
		steerToSeperate = Vector3.Normalize (steerToSeperate) * maxAccelleration;

	}

	void FixedUpdate () {
		Vector3 accelToAdd = steerToCentre * sTCPriority + steerToSeperate * sTSPriority + steerToTarget * targetPriority;
		accelToAdd = Vector3.Normalize (accelToAdd) * maxAccelleration;
		GetComponent<Rigidbody> ().AddForce (accelToAdd, ForceMode.Acceleration);
	}



}
