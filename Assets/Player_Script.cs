using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.ImageEffects;
using UnityEngine.UI;
using System.Collections;
using System.Linq;
[RequireComponent(typeof(Rigidbody))]
public class Player_Script : MonoBehaviour {

	public static GameObject playerObj, passCube;
	Rigidbody rb;
	public float forwardSpeed, horizontalSpeed, verticalSpeed, sidePulseSpeed;
	public float camRotationSpeed, torque, correctionStrength;

	private float speedMult;

	public Text boostText;
	public Slider boostSlider;
	public float boostTimeMax, boostTimeCurrent;

	private bool boostActive;

	public GameObject[] allEnemies;
	private int totalEnemies;
//	private MotionBlur mb;

	private bool playerHasControl;


	//Player Statistics

	public float health; 
	public Slider healthSlider;
	//Weapons
	public GameObject gatlingBulletTemplate, railBulletTemplate, shotBulletTemplate;
	public float gatlingBulletForce, railBulletForce, shotBulletForce;
	private float primaryTimer, secondaryTimer;

	public enum weaponTypes {GATLING, RAIL, SHOT, SAW, NONE};
	public weaponTypes primaryWeapon, secondaryWeapon;

	public float gatlingCool, railCool, shotCool, sawCool;
	public float gatlingSpread, shotSpread;
	public float sawReach;
	public GameObject primaryPoint, secondaryPoint;
	public float primaryHeat, secondaryHeat;

	public Text primaryGunText, secondaryGunText;
	public Slider primaryHeatSlider, secondaryHeatSlider;

	//Inventory

	public GameObject inventoryPanel;
	public Button pGatButton, pRailButton, pShotButton, pSawButton, sGatButton, sRailButton, sShotButton, sSawButton;
	private ColorBlock selectedBlock, normalBlock;

	//Home
	public GameObject homeObj;
	private bool homeBoundActive;

	//UI
	public Text enemyCounterText;

	//weapons stats
	public int shotCount;

	public GameObject leftGatGO, rightGatGO, leftRailGO, rightRailGO, leftShotGO, rightShotGO, leftSawGO, rightSawGO;

	void Awake () {
		playerObj = this.gameObject;
		rb = gameObject.GetComponent<Rigidbody> ();
		speedMult = 1;
		boostSlider.minValue = 0;
		boostSlider.maxValue = boostTimeMax;
		boostSlider.value = boostTimeMax;
		boostTimeCurrent = boostTimeMax;

		healthSlider.minValue = 0;
		healthSlider.maxValue = 100;
		healthSlider.value = health;
		primaryTimer = 0;
		secondaryTimer = 0;

		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;

		homeObj = GameObject.FindGameObjectWithTag ("Home");
		passCube = GameObject.FindGameObjectWithTag ("Pass Cube");

		pGatButton.onClick.AddListener (() => ChoosePrimary (weaponTypes.GATLING));
		pRailButton.onClick.AddListener (() => ChoosePrimary (weaponTypes.RAIL));
		pShotButton.onClick.AddListener (() => ChoosePrimary (weaponTypes.SHOT));
		pSawButton.onClick.AddListener (() => ChoosePrimary (weaponTypes.SAW));

		sGatButton.onClick.AddListener (() => ChooseSecondary (weaponTypes.GATLING));
		sRailButton.onClick.AddListener (() => ChooseSecondary (weaponTypes.RAIL));
		sShotButton.onClick.AddListener (() => ChooseSecondary (weaponTypes.SHOT));
		sSawButton.onClick.AddListener (() => ChooseSecondary (weaponTypes.SAW));

	}

	// Use this for initialization
	void Start () {
//		mb = Camera.main.GetComponent<MotionBlur> ();
		ChoosePrimary (weaponTypes.GATLING);
		ChooseSecondary (weaponTypes.RAIL);
		playerHasControl = true;
		health = 100;
		allEnemies = DetectEnemies ();
		Invoke ("ConstructEnemyCounter", 0.5f);
	}
	
	// Update is called once per frame
	void Update () {
		PlayerInput();
		UpdateUI();
		BoostUpdate();
		HeatUpdate ();
	}

	void FixedUpdate () {
		if (playerHasControl){
			PlayerMovement();
			CameraRotation ();

		}
		if (homeBoundActive){
			GoHome ();
		}
	}

	private void ConstructEnemyCounter () {
		totalEnemies = allEnemies.Length;
		enemyCounterText.text = "Enemies:" + "\n" + totalEnemies + "/" + totalEnemies;
	}

	public void UpdateEnemyCounter () {
		enemyCounterText.text = "Enemies: " + "\n" + DetectEnemies ().Length + "/" + totalEnemies;
		if (DetectEnemies ().Length <= 0) {
			passCube.SetActive (false);
		} else if (DetectEnemies ().Length <= 10 && DetectEnemies ().Length >= 0) {
			LightEmUp ();
		}
	}

	private void LightEmUp () {
		for (int i = 0; i < DetectEnemies ().Length; i++) {
			
		}
	}

	private GameObject[] DetectEnemies () {
		GameObject[] enemies = GameObject.FindGameObjectsWithTag ("Enemy");
		GameObject[] otherEnemies = GameObject.FindGameObjectsWithTag ("Swarm");
		GameObject[] combinedEnemies = enemies.Concat (otherEnemies).ToArray();
		return combinedEnemies;
	}

	void OnDrawGizmos () {
		Gizmos.color = Color.red;
		Gizmos.DrawWireCube(primaryPoint.transform.position + transform.forward, new Vector3(sawReach, sawReach, sawReach));
		Gizmos.DrawWireCube(secondaryPoint.transform.position + transform.forward, new Vector3(sawReach, sawReach, sawReach));
	}

	private void PlayerMovement () {
		Vector3 moveX = transform.right * Input.GetAxis("Horizontal") * horizontalSpeed;
//		Vector3 moveX = Vector3.zero;
		Vector3 moveY = transform.up * Input.GetAxis("Vertical") * verticalSpeed;
		Vector3 moveZ = transform.forward * Input.GetAxis("Forward") * forwardSpeed * speedMult;

		Vector3 moveDirection = moveX + moveY + moveZ;
		if (moveDirection.magnitude < 1){
			rb.velocity = Vector3.Lerp (rb.velocity, Vector3.zero, 0.1f);
		}
		rb.AddForce (moveDirection, ForceMode.Force);

//		if (Input.GetButtonDown("Horizontal") && Input.GetAxis("Horizontal") > 0){
//			rb.AddForce (transform.right * sidePulseSpeed , ForceMode.Impulse);
//		}
//
//		if (Input.GetButtonDown("Horizontal") && Input.GetAxis("Horizontal") < 0){
//			rb.AddForce (transform.right * -sidePulseSpeed , ForceMode.Impulse);
//		}
	}

	private void CameraRotation () {
//		Quaternion g = Quaternion.LookRotation((transform.forward + (transform.right * camRotationSpeed * Input.GetAxis("Mouse X") * Time.deltaTime)), Vector3.up);
//		transform.rotation = g;
//
//		Quaternion r = Quaternion.LookRotation((transform.forward + (transform.up * camRotationSpeed * Input.GetAxis("Mouse Y") * Time.deltaTime)), Vector3.up);
//		transform.rotation = r;

//		float turn = Input.GetAxis("Horizontal");
//		rb.AddTorque(transform.up * torque * turn);

		float mxT = Input.GetAxis ("Mouse X") * torque;
		float myT = Input.GetAxis ("Mouse Y") * torque * 1.1f;
		float mrT = Input.GetAxis ("Roll") * torque;

		rb.AddRelativeTorque (0, mxT, 0);
		rb.AddRelativeTorque (-myT,0,0);
		rb.AddRelativeTorque (0, 0, mrT);

		Vector3 properRight = Quaternion.Euler (0, 0, -transform.localEulerAngles.z) * transform.right;
		Vector3 uprightCorrection = Vector3.Cross (transform.right, properRight);
		rb.AddRelativeTorque (uprightCorrection * correctionStrength);

	}
		
	private void PlayerInput () {

		if (playerHasControl){
			
			if (Input.GetButtonDown("Boost")){
				boostActive = true;
			}

			if (Input.GetButton("Fire1")){
				FirePrimary ();
				UpdateEnemyCounter ();
			}

			if (Input.GetButton("Fire2")){
				FireSecondary ();
				UpdateEnemyCounter ();
			}
		}

		if (Input.GetButtonUp("Boost")){
			boostActive = false;
		}

//		if (Input.GetKeyDown(KeyCode.Escape)){
//			UnityEditor.EditorApplication.isPlaying = false;
//		}
//
		if (Input.GetKeyDown(KeyCode.I)){
			ToggleInventory ();
		}

		if (Input.GetKeyDown(KeyCode.H)){
			ToggleHomeBound ();
		}

		if (primaryTimer > 0){
			primaryTimer -= Time.deltaTime;
		}

		if (secondaryTimer > 0){
			secondaryTimer -= Time.deltaTime;
		}
	}

	private void ChoosePrimary (weaponTypes weapon){
		primaryWeapon = weapon;

		switch (weapon)
		{
		case weaponTypes.GATLING:
			pGatButton.interactable = false;
				

			pRailButton.interactable = true;
			pShotButton.interactable = true;
			pSawButton.interactable = true;

			leftGatGO.SetActive (true);

			leftRailGO.SetActive (false);
			leftShotGO.SetActive (false);
			leftSawGO.SetActive (false);
			break;
		case weaponTypes.RAIL:
			pRailButton.interactable = false;

			pGatButton.interactable = true;
			pShotButton.interactable = true;
			pSawButton.interactable = true;

			leftRailGO.SetActive (true);

			leftGatGO.SetActive (false);
			leftShotGO.SetActive (false);
			leftSawGO.SetActive (false);
			break;
		case weaponTypes.SHOT:
			pShotButton.interactable = false;

			pGatButton.interactable = true;
			pRailButton.interactable = true;
			pSawButton.interactable = true;

			leftSawGO.SetActive (true);

			leftGatGO.SetActive (false);
			leftRailGO.SetActive (false);
			leftSawGO.SetActive (false);
			break;
		case weaponTypes.SAW:
			pSawButton.interactable = false;

			pGatButton.interactable = true;
			pRailButton.interactable = true;
			pShotButton.interactable = true;

			leftSawGO.SetActive (true);

			leftGatGO.SetActive (false);
			leftRailGO.SetActive (false);
			leftShotGO.SetActive (false);
			break;
		}
	}

	private void ChooseSecondary (weaponTypes weapon){
		secondaryWeapon = weapon;

		switch (weapon)
		{
		case weaponTypes.GATLING:
			sGatButton.interactable = false;

			sRailButton.interactable = true;
			sShotButton.interactable = true;
			sSawButton.interactable = true;

			rightGatGO.SetActive (true);

			rightRailGO.SetActive (false);
			rightShotGO.SetActive (false);
			rightSawGO.SetActive (false);
			break;
		case weaponTypes.RAIL:
			sRailButton.interactable = false;

			sGatButton.interactable = true;
			sShotButton.interactable = true;
			sSawButton.interactable = true;

			rightRailGO.SetActive (true);

			rightGatGO.SetActive (false);
			rightShotGO.SetActive (false);
			rightSawGO.SetActive (false);
			break;
		case weaponTypes.SHOT:
			sShotButton.interactable = false;

			sGatButton.interactable = true;
			sRailButton.interactable = true;
			sSawButton.interactable = true;

			rightShotGO.SetActive (true);

			rightGatGO.SetActive (false);
			rightRailGO.SetActive (false);
			rightSawGO.SetActive (false);
			break;
		case weaponTypes.SAW:
			sSawButton.interactable = false;

			sGatButton.interactable = true;
			sRailButton.interactable = true;
			sShotButton.interactable = true;

			rightSawGO.SetActive (true);

			rightGatGO.SetActive (false);
			rightRailGO.SetActive (false);
			rightShotGO.SetActive (false);
			break;
		}
	}

	private void FirePrimary(){

		Vector3 targetPosition = Camera.main.ScreenToWorldPoint (new Vector3(Screen.width / 2, Screen.height / 2, 75));

		if (primaryTimer <= 0){
			switch (primaryWeapon)
			{
			case weaponTypes.GATLING:
				primaryTimer = gatlingCool;
				GameObject gatlingBullet = Instantiate (gatlingBulletTemplate, primaryPoint.transform.position + transform.forward * 1.0f, Quaternion.identity) as GameObject;
				Vector3 bulletTarget = new Vector3 (Random.Range (targetPosition.x - gatlingSpread, targetPosition.x + gatlingSpread), Random.Range (targetPosition.y - gatlingSpread, targetPosition.y + gatlingSpread), Random.Range (targetPosition.z - gatlingSpread, targetPosition.z + gatlingSpread));
				gatlingBullet.transform.LookAt (bulletTarget);
				gatlingBullet.GetComponent<Rigidbody> ().AddForce (gatlingBullet.transform.forward * gatlingBulletForce, ForceMode.Impulse);

				primaryHeat += 1;
				break;
			case weaponTypes.RAIL:
				primaryTimer = railCool;
				GameObject railBullet = Instantiate (railBulletTemplate, primaryPoint.transform.position + transform.forward * 1.0f, Quaternion.identity) as GameObject;
				Vector3 railTargetPosition = Camera.main.ScreenToWorldPoint (new Vector3 (Screen.width / 2, Screen.height / 2, 1000));
				railBullet.transform.LookAt (railTargetPosition);
				railBullet.GetComponent<Rigidbody> ().AddForce (railBullet.transform.forward * railBulletForce, ForceMode.Impulse);

				primaryHeat += 15;
				break;
			case weaponTypes.SHOT:
				primaryTimer = shotCool;
				float forwardOffset = 0.5f;
				int i = 0;
				for (i = 0; i < shotCount; i++){
					GameObject shot = Instantiate (shotBulletTemplate, primaryPoint.transform.position + transform.forward * forwardOffset, Quaternion.identity) as GameObject;
					Vector3 shotBulletTarget = new Vector3 (Random.Range (targetPosition.x - shotSpread, targetPosition.x + shotSpread), Random.Range (targetPosition.y - shotSpread, targetPosition.y + shotSpread), Random.Range(targetPosition.z - shotSpread, targetPosition.z + shotSpread));
					shot.transform.LookAt (shotBulletTarget);
					shot.GetComponent<Rigidbody> ().AddForce (shot.transform.forward * shotBulletForce);
				}

				primaryHeat += 30;
				break;
			case weaponTypes.SAW:
				primaryTimer = shotCool;
				Collider[] boxCol = Physics.OverlapBox (primaryPoint.transform.position + (transform.forward * 2), new Vector3(sawReach / 2, sawReach / 2, sawReach / 2));


				foreach (Collider c in boxCol){

					switch (c.gameObject.tag)
					{
					case "Enemy":
						print ("Enemy " + c.gameObject.name + " hit");
						break;
					}
				}
				break;
			}

		}
	}

	private void FireSecondary(){

		Vector3 targetPosition = Camera.main.ScreenToWorldPoint (new Vector3(Screen.width / 2, Screen.height / 2, 100));

		if (secondaryTimer <= 0){
			switch (secondaryWeapon)
			{
			case weaponTypes.GATLING:
				secondaryTimer = gatlingCool;
				GameObject bullet = Instantiate (gatlingBulletTemplate, secondaryPoint.transform.position + transform.forward * 1.0f, Quaternion.identity) as GameObject;
				Vector3 bulletTarget = new Vector3 (Random.Range (targetPosition.x - (gatlingSpread + (gatlingSpread * 0.1f)), targetPosition.x + gatlingSpread), Random.Range (targetPosition.y - (gatlingSpread + (gatlingSpread * 0.1f)), targetPosition.y + gatlingSpread), targetPosition.z);
				bullet.transform.LookAt (bulletTarget);
				bullet.GetComponent<Rigidbody> ().AddForce (bullet.transform.forward * gatlingBulletForce, ForceMode.Impulse);

				secondaryHeat += 1;
				break;
			case weaponTypes.RAIL:
				secondaryTimer = railCool;
				GameObject railBullet = Instantiate (railBulletTemplate, secondaryPoint.transform.position + transform.forward * 1.0f, Quaternion.identity) as GameObject;
				Vector3 railTargetPosition = Camera.main.ScreenToWorldPoint (new Vector3 (Screen.width / 2, Screen.height / 2, 1000));
				railBullet.transform.LookAt (railTargetPosition);
				railBullet.GetComponent<Rigidbody> ().AddForce (railBullet.transform.forward * railBulletForce, ForceMode.Impulse);

				secondaryHeat += 15;
				break;
			case weaponTypes.SHOT:
				secondaryTimer = shotCool;
				float forwardOffset = 0.5f;
				int i = 0;
				for (i = 0; i < shotCount; i++){
					GameObject shot = Instantiate (shotBulletTemplate, secondaryPoint.transform.position + transform.forward * forwardOffset, Quaternion.identity) as GameObject;
					Vector3 shotBulletTarget = new Vector3 (Random.Range (targetPosition.x - shotSpread, targetPosition.x + shotSpread), Random.Range (targetPosition.y - shotSpread, targetPosition.y + shotSpread), Random.Range(targetPosition.z - shotSpread, targetPosition.z + shotSpread));
					shot.transform.LookAt (shotBulletTarget);
					shot.GetComponent<Rigidbody> ().AddForce (shot.transform.forward * shotBulletForce);
				}

				secondaryHeat += 30;
				break;
			case weaponTypes.SAW:
				secondaryTimer = shotCool;
				Collider[] boxCol = Physics.OverlapBox (secondaryPoint.transform.position + (transform.forward * 2), new Vector3(sawReach / 2, sawReach / 2, sawReach / 2));


				foreach (Collider c in boxCol){
					if (c.gameObject.tag == "Enemy"){
						print ("Enemy " + c.gameObject.name + " detected");
					}
				}
				break;
			}
		}
	}

	private void UpdateUI(){
		boostSlider.value = boostTimeCurrent;

		primaryGunText.text = primaryWeapon.ToString ();
		primaryHeatSlider.value = primaryHeat;
		secondaryGunText.text = secondaryWeapon.ToString ();
		secondaryHeatSlider.value = secondaryHeat;
	}

	private void BoostUpdate(){
		if (boostActive){
//			mb.enabled = true;
			if (boostTimeCurrent > 0){
				speedMult = 2.5f;
				boostTimeCurrent -= Time.deltaTime;
			} else {
				boostActive = false;
			}
		} else {
//			mb.enabled = false;
			speedMult = 1;
			boostTimeCurrent += Time.deltaTime * 0.5f;
		}

		boostTimeCurrent = Mathf.Clamp(boostTimeCurrent, 0, boostTimeMax);
	}

	private void HeatUpdate(){
		if (primaryHeat > 0){
			primaryHeat -= Time.deltaTime * 10;
		}
		if (secondaryHeat > 0){
			secondaryHeat -= Time.deltaTime * 10;
		}
	}

	private void ToggleInventory(){
		inventoryPanel.SetActive (!inventoryPanel.activeSelf);

		if (inventoryPanel.activeSelf){
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;

			Time.timeScale = 0.5f;
		} else {
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;

			Time.timeScale = 1;
		}
	}

	private void ToggleHomeBound(){
		homeBoundActive = !homeBoundActive;

		if (homeBoundActive){
			playerHasControl = false;
		} else {
			playerHasControl = true;
		}
	}

	private void GoHome(){
		
	}

	public void DamagePlayer (int damage) {
		health -= damage;

		if (health <= 0) {
			SceneManager.LoadScene ("Level 1");
		}

		healthSlider.value = health;
	}
}
