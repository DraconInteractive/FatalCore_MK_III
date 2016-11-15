// Requires
@script RequireComponent(Rigidbody)

// Public variables
// Roughly how long between updates
var flockUpdatesPerSecond : float = 5;
var spread : float = 5;
// How far do the Boids look for their kin?
var sightRadius : float = 10;
//  How much spread does the sight have?
var sightAngle : float = 90;

var maxAcceleration : float = 5;

private var steerToCentre : Vector3 = Vector3.zero;
private var steerToSeparate : Vector3 = Vector3.zero;

private var neighbourhood : Collider[];

var sTCPriority : float = 5.0;
var sTSPriority : float = 5.0;
var id : int;

// Init
function Awake() {

	id = Random.Range(0, 1000);

}

// Set off the coroutine
function Start() {

	DoFlock();

}

// Coroutine
function DoFlock() {

	while (true) {
	
		neighbourhood = Physics.OverlapSphere( transform.position, sightRadius );
		
		// Wait. We've already used a lot of processing power this update.
		yield WaitForSeconds(SpreadUpdates());
		
		DoSeparationAndCohesion();
		
		// We're done with flocking for this update! Come back next update!
		yield WaitForSeconds(SpreadUpdates());
	
	} // end while (true)

} // End function DoFlock()

function SpreadUpdates() {

	// We're going to take targetTime and return a number binomially distributed about it.
	var offset = ( Random.value - Random.value ) / spread;
	return  ( 1 / flockUpdatesPerSecond ) + offset;
	
}

function DoSeparationAndCohesion() {
	
	var flockDirection : Vector3 = Vector3.zero;
	var closestFlockerDirection : Vector3 = Vector3.zero;
	var flockCentre : Vector3 = Vector3.zero;
	
	var shortestDistance : float = Mathf.Infinity;
	var closestLocation : Vector3;
	
	for ( var otherFlocker in neighbourhood ) {
		
	
		if (otherFlocker.GetComponent.<Rigidbody>() && otherFlocker.GetComponent(Flock).id != id) {
		
			flockDirection += otherFlocker.transform.forward;
			flockCentre += otherFlocker.transform.position;
			var testDist = Vector3.Distance( transform.position, otherFlocker.transform.position );
			if (testDist < shortestDistance ) {
			
				shortestDistance = testDist;
				closestLocation = otherFlocker.transform.position;
			
			}
		}
	}	
	
	flockDirection /= neighbourhood.length - 1;
	
	flockCentre /= neighbourhood.length - 1;
	
	if (shortestDistance != Mathf.Infinity) {
		// Get a steering force away from the closest otherFlocker
		steerToSeparate = transform.position - closestLocation;
	}
	else {
		steerToSeparate = Vector3.zero;
	}
	
	// Steer towards flockCentre
	steerToCentre = Vector3.Normalize(flockCentre - transform.position) * maxAcceleration;
	steerToSeparate = Vector3.Normalize(steerToSeparate) * maxAcceleration;
	
}

function FixedUpdate() {

	var accelToAdd : Vector3 = steerToCentre * sTCPriority + steerToSeparate * sTSPriority;
	accelToAdd = Vector3.Normalize(accelToAdd) * maxAcceleration;

	GetComponent.<Rigidbody>().AddForce(accelToAdd, ForceMode.Acceleration);

}