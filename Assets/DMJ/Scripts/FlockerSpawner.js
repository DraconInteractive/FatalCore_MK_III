var newFlockerDelay : float = 3.0;
var spawnRadius : float = 75.0;
var flocker : GameObject;

function Start() {

	CreateSomeFlockers();

}

function CreateSomeFlockers() {

	while (true) {
	
		SpawnFlocker();
		
		yield WaitForSeconds(newFlockerDelay);
	
	}
}

function SpawnFlocker() {

	Instantiate( flocker, Random.onUnitSphere * spawnRadius, transform.rotation );

}