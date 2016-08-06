// Size of box
var boxSize : float = 500;

function Awake() {

	transform.localScale = Vector3(boxSize, boxSize, boxSize);

}

function OnTriggerExit( escapee : Collider ) {

	// If an object escapes from the box, we move it to the far side of the boundaries!

	if ( Mathf.Abs(escapee.transform.position.x) > boxSize / 2 ) {
		escapee.gameObject.transform.position.x *= -0.95;
	}
	if ( Mathf.Abs(escapee.transform.position.y) > boxSize / 2 ) {
		escapee.gameObject.transform.position.y *= -0.95;
	}
	if ( Mathf.Abs(escapee.transform.position.z) > boxSize / 2 ) {
		escapee.gameObject.transform.position.z *= -0.95;
	}
	Debug.Log("Escapee: " + escapee.GetComponent(Flock).id );

}