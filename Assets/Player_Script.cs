using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.ImageEffects;
using Kino;
using UnityEngine.UI;
using System.Collections;
using System.Linq;
using FMODUnity;
using FMOD;
using FMOD.Studio;
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

	private bool playerHasControl;

	bool effectActive, kinoActive;

	public AnalogGlitch ag;
	public DigitalGlitch dg;
	//Player Statistics

	public float health, shield; 
	public Slider healthSlider, shieldSlider;
	//Weapons
	public GameObject gatlingBulletTemplate, railBulletTemplate, shotBulletTemplate;
	public float gatlingBulletForce, railBulletForce, shotBulletForce;
	private float primaryTimer, secondaryTimer;

	public enum weaponTypes {GATLING, RAIL, SHOT, SAW, NONE};
	public weaponTypes primaryWeapon, secondaryWeapon;

	public float gatlingCool, railCool, shotCool, sawCool;
	public float gatlingSpread, shotSpread;
	public float sawReach;
	public int sawDamage;
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
	public Animator leftGatAnim, leftRailAnim, leftShotAnim, leftSawAnim, rightGatAnim, rightRailAnim, rightShotAnim, rightSawAnim;

	public WeaponModification gatMod, railMod, shotMod, sawMod;

	public StudioEventEmitter fmodMovementEmitter;

	public GatlingLaserScript gatLeftLaser, gatRightLaser;
	public RailLaserScript railLeftLaser, railRightLaser;

	public AudioClip gatClip, railClip, shotClip, sawClip;
	public AudioClip shieldDamageClip, hullDamageClip;
	public AudioClip deathClip;

	public AudioSource pWAS, sWAS, dadAS;

	void Awake () {
		playerObj = this.gameObject;
		rb = gameObject.GetComponent<Rigidbody> ();
		ag = Camera.main.gameObject.GetComponent<AnalogGlitch> ();
		dg = Camera.main.gameObject.GetComponent<DigitalGlitch> ();
//		weaponAS.enabled = false;
//		dadAS.enabled = false;
		speedMult = 1;
		boostSlider.minValue = 0;
		boostSlider.maxValue = boostTimeMax;
		boostSlider.value = boostTimeMax;
		boostTimeCurrent = boostTimeMax;

		healthSlider.minValue = 0;
		healthSlider.maxValue = 100;
		healthSlider.value = health;
		shieldSlider.minValue = 0;
		shieldSlider.maxValue = 100;
		shieldSlider.value = shield;
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
		shield = 100;
		allEnemies = DetectEnemies ();
		Invoke ("ConstructEnemyCounter", 0.5f);

		DamagePlayer (0);
	
		//		print ("rightGat: " + rightGatAnim.avatar.isValid);
//		print ("leftGat: " + leftGatAnim.avatar.isValid);
//		print ("rightRail: " + rightRailAnim.avatar.isValid);
//		print ("leftRail: " + leftRailAnim.avatar.isValid);
//		print ("rightShot: " + rightShotAnim.avatar.isValid);
//		print ("leftShot: " + leftShotAnim.avatar.isValid);
//		print ("rightDrill: " + rightSawAnim.avatar.isValid);
//		print ("leftDrill: " + leftSawAnim.avatar.isValid);

	}
	
	// Update is called once per frame
	void Update () {
		PlayerInput();
		UpdateUI();
		BoostUpdate();
		HeatUpdate ();
		MovementFMOD ();
	}

	void FixedUpdate () {
		if (playerHasControl){
			PlayerMovement();
			CameraRotation ();
		}
	}

	void OnEnable () {
		ag = Camera.main.GetComponent<AnalogGlitch> ();
		dg = Camera.main.GetComponent<DigitalGlitch> ();
	}

	#region EnemyFunctions
	private void ConstructEnemyCounter () {
		totalEnemies = allEnemies.Length;
		enemyCounterText.text = "Enemies:" + "\n" + totalEnemies + "/" + totalEnemies;
	}

	public void UpdateEnemyCounter () {
		enemyCounterText.text = "Enemies: " + "\n" + DetectEnemies ().Length + "/" + totalEnemies;
		if (DetectEnemies ().Length <= 0) {
			passCube.SetActive (false);
		} else if (DetectEnemies ().Length <= 10 && DetectEnemies ().Length >= 0) {
//			LightEmUp ();
		}
	}

//	private void LightEmUp () {
//		for (int i = 0; i < DetectEnemies ().Length; i++) {
//			
//		}
//	}


	private GameObject[] DetectEnemies () {
		GameObject [] towers = GameObject.FindGameObjectsWithTag ("Tower");
		GameObject [] swarms = GameObject.FindGameObjectsWithTag ("Swarm");
		GameObject[] elites = GameObject.FindGameObjectsWithTag ("Elite");
		GameObject[] towersAndSwarms = towers.Concat (swarms).ToArray();
		GameObject[] combinedEnemies = towersAndSwarms.Concat (elites).ToArray ();
		return combinedEnemies;
	}
	#endregion

//	void OnDrawGizmos () {
//		Gizmos.color = Color.red;
//		Gizmos.DrawWireCube(primaryPoint.transform.position + transform.forward, new Vector3(sawReach, sawReach, sawReach));
//		Gizmos.DrawWireCube(secondaryPoint.transform.position + transform.forward, new Vector3(sawReach, sawReach, sawReach));
//	}

	#region locomotion
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
	#endregion	

	private void PlayerInput () {

		if (playerHasControl){
			
			if (Input.GetButtonDown("Boost")){
				boostActive = true;
			}
				
			if (Input.GetButton("Fire1")){
				FirePrimary ();
			} else {
				if (leftGatGO.activeSelf) {
					leftGatAnim.SetBool ("firing", false);
				}
				if (leftRailGO.activeSelf) {
					leftRailAnim.SetBool ("firing", false);
				}
				if (leftShotGO.activeSelf) {
					leftShotAnim.SetBool ("firing", false);
				}
				if (leftSawGO.activeSelf) {
					leftSawAnim.SetBool ("firing", false);
				}
			}

			if (Input.GetButton("Fire2")){
				FireSecondary ();
			} else {
				if (rightGatGO.activeSelf) {
					rightGatAnim.SetBool ("firing", false);
				}
				if (rightRailGO.activeSelf) {
					rightRailAnim.SetBool ("firing", false);
				}
				if (rightShotGO.activeSelf) {
					rightShotAnim.SetBool ("firing", false);
				}
				if (rightSawGO.activeSelf) {
					rightSawAnim.SetBool ("firing", false);
				}
			}
		}

		if (Input.GetButtonUp("Boost")){
			boostActive = false;
		}

		if (Input.GetKeyDown(KeyCode.Escape)){
//			UnityEditor.EditorApplication.isPlaying = false;
			Application.Quit ();
		}

		if (Input.GetKeyDown(KeyCode.I)){
			ToggleInventory ();
		}

		if (primaryTimer > 0){
			primaryTimer -= Time.deltaTime;
		}

		if (secondaryTimer > 0){
			secondaryTimer -= Time.deltaTime;
		}

		if (Input.GetKeyDown(KeyCode.F2)) {
			SceneManager.LoadScene (1);
		}

		if (Input.GetKeyDown(KeyCode.F3)) {
			SceneManager.LoadScene (2);
		}
	}

	#region weapons
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
			
//			primarySound.setParameterValue ("Weapon Select", 2);
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

//			primarySound.setParameterValue ("Weapon Select", 4);

			break;
		case weaponTypes.SHOT:
			pShotButton.interactable = false;

			pGatButton.interactable = true;
			pRailButton.interactable = true;
			pSawButton.interactable = true;


			leftShotGO.SetActive (true);
			
			leftGatGO.SetActive (false);
			
			leftRailGO.SetActive (false);
			
			leftSawGO.SetActive (false);

//			primarySound.setParameterValue ("Weapon Select", 3);

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

//			primarySound.setParameterValue ("Weapon Select", 1);

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
			
//			secondarySound.setParameterValue ("Weapon Select", 2);

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
		
//			secondarySound.setParameterValue ("Weapon Select", 4);

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
			
//			secondarySound.setParameterValue ("Weapon Select", 3);

			break;
		case weaponTypes.SAW:
			sSawButton.interactable = false;

			sGatButton.interactable = true;
			sRailButton.interactable = true;
			sShotButton.interactable = true;



			rightSawGO.SetActive (true);
			
			rightGatGO.SetActive (false);
			
			rightShotGO.SetActive (false);
			
			rightRailGO.SetActive (false);

//			secondarySound.setParameterValue ("Weapon Select", 1);

			break;
		}
	}

	private void FirePrimary(){

		Vector3 targetPosition = Camera.main.ScreenToWorldPoint (new Vector3(Screen.width / 2, Screen.height / 2, 75));

		if (primaryTimer <= 0){
			switch (primaryWeapon)
			{
			case weaponTypes.GATLING:
				primaryTimer = gatlingCool - gatMod.fireRateMod;
//				GameObject gatlingBullet = Instantiate (gatlingBulletTemplate, primaryPoint.transform.position + transform.forward * 1.0f, Quaternion.identity) as GameObject;
//				Vector3 bulletTarget = new Vector3 (Random.Range (targetPosition.x - gatlingSpread, targetPosition.x + gatlingSpread), Random.Range (targetPosition.y - gatlingSpread, targetPosition.y + gatlingSpread), Random.Range (targetPosition.z - gatlingSpread, targetPosition.z + gatlingSpread));
//				gatlingBullet.transform.LookAt (bulletTarget);
//				Rigidbody gRB = gatlingBullet.GetComponent<Rigidbody> ();
//				gRB.velocity = rb.velocity;
//				gRB.AddForce (gatlingBullet.transform.forward * gatlingBulletForce, ForceMode.Impulse);
//				gatlingBullet.GetComponent<BulletScript> ().damage += (int)gatMod.damageMod;
				gatLeftLaser.StopCoroutine ("FireLaser");
				gatLeftLaser.StartCoroutine ("FireLaser");
				primaryHeat += 1.0f;
				leftGatAnim.SetBool ("firing", true);
				StartCoroutine (GatlingSoundFire (0));

				break;
			case weaponTypes.RAIL:
				primaryTimer = railCool - railMod.fireRateMod;
//				GameObject railBullet = Instantiate (railBulletTemplate, primaryPoint.transform.position + transform.forward * 1.0f, Quaternion.identity) as GameObject;
//				Vector3 railTargetPosition = Camera.main.ScreenToWorldPoint (new Vector3 (Screen.width / 2, Screen.height / 2, 1000));
//				railBullet.transform.LookAt (railTargetPosition);
//				Rigidbody rRB = railBullet.GetComponent<Rigidbody> ();
//				rRB.velocity = rb.velocity;
//				rRB.AddForce (railBullet.transform.forward * railBulletForce, ForceMode.Impulse);
//				railBullet.transform.GetChild (0).gameObject.GetComponent<Rail_Bullet_Script> ().damage += (int)railMod.damageMod;
				railLeftLaser.StopCoroutine ("FireLaser");
				railLeftLaser.StartCoroutine ("FireLaser");
				primaryHeat += 15;
				leftRailAnim.SetTrigger ("Fire");

				StartCoroutine (RailSoundFire (0));

				break;
			case weaponTypes.SHOT:
				primaryTimer = shotCool - shotMod.fireRateMod;
				float forwardOffset = 0.5f;
				int i = 0;
				for (i = 0; i < shotCount; i++) {
					GameObject shot = Instantiate (shotBulletTemplate, primaryPoint.transform.position + transform.forward * forwardOffset, Quaternion.identity) as GameObject;
					Vector3 shotBulletTarget = new Vector3 (Random.Range (targetPosition.x - shotSpread, targetPosition.x + shotSpread), Random.Range (targetPosition.y - shotSpread, targetPosition.y + shotSpread), Random.Range (targetPosition.z - shotSpread, targetPosition.z + shotSpread));
					shot.transform.LookAt (shotBulletTarget);
					Rigidbody sRB = shot.GetComponent<Rigidbody> ();
//					sRB.velocity = rb.velocity;
					sRB.AddForce (shot.transform.forward * shotBulletForce);
					sRB.velocity = Vector3.ClampMagnitude (sRB.velocity, 10);
					shot.GetComponent<BulletScript> ().damage += (int)shotMod.damageMod;
				}

				primaryHeat += 30;
				leftShotAnim.SetBool ("firing", true);

				StartCoroutine (ShotgunSoundFire (0));
				break;
			case weaponTypes.SAW:
				primaryTimer = sawCool - sawMod.fireRateMod;
				Collider[] boxCol = Physics.OverlapBox (primaryPoint.transform.position + (transform.forward * 2), new Vector3 (sawReach / 2, sawReach / 2, sawReach / 2));


				foreach (Collider c in boxCol) {

					switch (c.gameObject.tag) {
					case "Enemy":
						if (c.gameObject.GetComponent<Swarm_Script_02> ()) {
							c.gameObject.GetComponent<Swarm_Script_02> ().DamageAI (sawDamage + (int)sawMod.damageMod);
						}
						if (c.gameObject.GetComponent<AI_Tower_Script> ()) {
							c.gameObject.GetComponent<AI_Tower_Script> ().DamageAI (sawDamage + (int)sawMod.damageMod);
						}
						break;
					}
				}

				leftSawAnim.SetBool ("firing", true);
				StartCoroutine (DrillSoundFire (0));
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
				secondaryTimer = gatlingCool - gatMod.fireRateMod;
//				GameObject bullet = Instantiate (gatlingBulletTemplate, secondaryPoint.transform.position + transform.forward * 1.0f, Quaternion.identity) as GameObject;
//				Vector3 bulletTarget = new Vector3 (Random.Range (targetPosition.x - (gatlingSpread + (gatlingSpread * 0.1f)), targetPosition.x + gatlingSpread), Random.Range (targetPosition.y - (gatlingSpread + (gatlingSpread * 0.1f)), targetPosition.y + gatlingSpread), targetPosition.z);
//				bullet.transform.LookAt (bulletTarget);
//				Rigidbody gRB = bullet.GetComponent<Rigidbody> ();
//				gRB.velocity = rb.velocity;
//				gRB.AddForce (bullet.transform.forward * gatlingBulletForce, ForceMode.Impulse);
//				bullet.GetComponent<BulletScript> ().damage += (int)gatMod.damageMod;
				gatRightLaser.StopCoroutine ("FireLaser");
				gatRightLaser.StartCoroutine ("FireLaser");
				secondaryHeat += 1;

				rightGatAnim.SetBool ("firing", true);
				StartCoroutine (GatlingSoundFire (1));
				break;
			case weaponTypes.RAIL:
				secondaryTimer = railCool - railMod.fireRateMod;
//				GameObject railBullet = Instantiate (railBulletTemplate, secondaryPoint.transform.position + transform.forward * 1.0f, Quaternion.identity) as GameObject;
//				Vector3 railTargetPosition = Camera.main.ScreenToWorldPoint (new Vector3 (Screen.width / 2, Screen.height / 2, 1000));
//				railBullet.transform.LookAt (railTargetPosition);
//				Rigidbody rRB = railBullet.GetComponent<Rigidbody> ();
//				rRB.velocity = rb.velocity;
//				rRB.AddForce (railBullet.transform.forward * railBulletForce, ForceMode.Impulse);
//				railBullet.transform.GetChild (0).gameObject.GetComponent<Rail_Bullet_Script> ().damage += (int)railMod.damageMod;
				railRightLaser.StopCoroutine ("FireLaser");
				railRightLaser.StartCoroutine ("FireLaser");
				secondaryHeat += 15;

				rightRailAnim.SetTrigger ("Fire");
				StartCoroutine (RailSoundFire (1));
				break;
			case weaponTypes.SHOT:
				secondaryTimer = shotCool - shotMod.fireRateMod;
				float forwardOffset = 0.5f;
				int i = 0;
				for (i = 0; i < shotCount; i++) {
					GameObject shot = Instantiate (shotBulletTemplate, secondaryPoint.transform.position + transform.forward * forwardOffset, Quaternion.identity) as GameObject;
					Vector3 shotBulletTarget = new Vector3 (Random.Range (targetPosition.x - shotSpread, targetPosition.x + shotSpread), Random.Range (targetPosition.y - shotSpread, targetPosition.y + shotSpread), Random.Range (targetPosition.z - shotSpread, targetPosition.z + shotSpread));
					shot.transform.LookAt (shotBulletTarget);
					Rigidbody sRB = shot.GetComponent<Rigidbody> ();
					sRB.velocity = rb.velocity;
					sRB.AddForce (shot.transform.forward * shotBulletForce);
					shot.GetComponent<BulletScript> ().damage += (int)shotMod.damageMod;
				
				}

				secondaryHeat += 30;

				rightShotAnim.SetBool ("firing", true);
				StartCoroutine (ShotgunSoundFire (1));
				break;
			case weaponTypes.SAW:
				secondaryTimer = sawCool - sawMod.fireRateMod;
				Collider[] boxCol = Physics.OverlapBox (secondaryPoint.transform.position + (transform.forward * 2), new Vector3 (sawReach / 2, sawReach / 2, sawReach / 2));


				foreach (Collider c in boxCol) {
					if (c.gameObject.tag == "Enemy") {
						if (c.gameObject.GetComponent<Swarm_Script_02> ()) {
							c.gameObject.GetComponent<Swarm_Script_02> ().DamageAI (sawDamage + (int)sawMod.damageMod);
						}
						if (c.gameObject.GetComponent<AI_Tower_Script> ()) {
							c.gameObject.GetComponent<AI_Tower_Script> ().DamageAI (sawDamage + (int)sawMod.damageMod);
						}
					}
				}

				rightSawAnim.SetBool ("firing", true);
				StartCoroutine (DrillSoundFire (1));
				break;
			}
		}
	}

	IEnumerator RailSoundFire (int gun) {
		yield return new WaitForSeconds (1);
		if (gun == 0) {
			pWAS.clip = railClip;
			pWAS.Play ();
		} else {
			sWAS.clip = railClip;
			sWAS.Play ();
		}
		pWAS.clip = railClip;
		pWAS.Play ();
		yield break;
	}

	IEnumerator GatlingSoundFire (int gun) {
		if (gun == 0) {
			pWAS.clip = gatClip;
			pWAS.Play ();
		} else {
			sWAS.clip = gatClip;
			sWAS.Play ();
		}

		yield return new WaitForSeconds (gatlingCool - 0.01f);
		if (gun == 0) {
			pWAS.Stop ();
		} else {
			sWAS.Stop ();
		}
		yield break;
	}

	IEnumerator ShotgunSoundFire (int gun) {
		if (gun == 0) {
			pWAS.clip = shotClip;
			pWAS.Play ();
		} else {
			sWAS.clip = shotClip;
			sWAS.Play ();
		}
		yield break;
	}

	IEnumerator DrillSoundFire (int gun) {
		if (gun == 0) {
			pWAS.clip = sawClip;
			pWAS.Play ();
		} else {
			sWAS.clip = sawClip;
			sWAS.Play ();
		}

		yield return new WaitForSeconds (sawCool - 0.01f);
		if (gun == 0) {
			pWAS.Stop ();
		} else {
			sWAS.Stop ();
		}
		yield break;
	}
	#endregion

	#region stat updates
	private void UpdateUI(){
		boostSlider.value = boostTimeCurrent;
		primaryHeatSlider.value = primaryHeat;
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

		DamagePlayer (0);
	}

	private void ToggleHomeBound(){
		homeBoundActive = !homeBoundActive;

		if (homeBoundActive){
			playerHasControl = false;
		} else {
			playerHasControl = true;
		}
	}

	public void DamagePlayer (int damage) {
		
		if (!effectActive) {
			StartCoroutine (PlayerDamageEffect ());
		}
		bool s;
		if (shield > 0) {
			shield -= damage;
			s = true;
		} else {
			health -= damage;
			s = false;
		}
			
		if (shield < 0) {
			health -= (0 - shield);
			shield = 0;
		}
			
		if (health <= 0) {
			StartCoroutine (PlayerDeath ());
		}

		healthSlider.value = health;
		shieldSlider.value = shield;

		StartCoroutine (DamageSoundFire (s));
	}

	IEnumerator DamageSoundFire (bool shield) {
		if (shield) {
			dadAS.clip = shieldDamageClip;
		} else {
			dadAS.clip = hullDamageClip;
		}
		dadAS.Play ();
		yield break;
	}
	IEnumerator PlayerDamageEffect () {
		effectActive = true;
		print ("DEActive");
		ag.colorDrift += 0.25f;
		ag.scanLineJitter += 0.5f;
		yield return new WaitForSeconds (0.25f);
		ag.colorDrift -= 0.25f;
		ag.scanLineJitter -= 0.5f;
		effectActive = false;
		yield break;
	}

	IEnumerator PlayerDeath () {
		dadAS.clip = deathClip;
		dadAS.Play ();
		while (dg.intensity < 1) {
			dg.intensity += 0.1f;
			yield return new WaitForSeconds (0.1f);
		}

		yield return new WaitForSeconds (1.0f);
		SceneManager.LoadScene (SceneManager.GetActiveScene().name);
		dg.intensity = 0;
		yield break;
	}
	#endregion
	#region FMOD

	void MovementFMOD () {
//		fmodMovementEmitter.Params [0].Value = Input.GetAxis ("Vertical") * 100;
		float value = Mathf.Abs (Input.GetAxis ("Forward") * 100);
		fmodMovementEmitter.SetParameter ("Speed",value);
	}
		
	#endregion
}
