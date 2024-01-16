//----------------------------------------------
//           		 Stunt Crasher
//
// Copyright © 2014 - 2020 BoneCracker Games
// http://www.bonecrackergames.com
// Buğra Özdoğanlar
//
//----------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Main car controller with everything.
/// </summary>
public class C_CarController : MonoBehaviour{

	internal Rigidbody rigid;		//	Rigidbody of the vehicle.
	public Transform COM;		//	Centre of mass position of the vehicle.

	public bool isControllable = true;		//	Is vehicle now controllable now?
	public bool isCrashed = false;		//	 Is vehicle crashed now?

	/// <summary>
	/// Wheel class.
	/// </summary>
	[System.Serializable]
	public class Wheel{

		public WheelCollider wheelCollider;		//	WheelCollider of the wheel.
		public Transform wheelModel;		//	WheelModel of the wheel.

		public bool canSteering = false;		//	Is this wheel steerable?
		public bool canTorque = true;		//	Is this wheel power wheel?
		public bool canBrake = true;		//	Is this wheel can brake?

		internal float wheelRotation;		//	Rotation of the wheel (X).
		internal float slip;		//	Current slip amount of the wheel.
		internal int lastSkidmark = -1;		//	Used for ending skidmarks.

		// List for all particle systems.
		internal ParticleSystem particles;		//	Wheel particles.
		internal ParticleSystem.EmissionModule emission;		//	And emission used for enabling / disabling.

	}

	private C_Skidmarks skidmarks;		//	Skidmarks Manager.

	[Header("Wheels")]
	public Wheel[] wheels;

	[Header("Torques")]
	public float motorTorque = 1000f;		//	Maximum motor torque.
	public float boostTorque = 1000f;		//	Maximum boost torque.
	public float brakeTorque = 1000f;		//	Maximum brake torque.

	public float motorInput = 0f;		//	Current motor input clamped between 0f- 1f.
	public float boostTime = 3f;		//	How many seconds will apply boost torque?

	public float speed = 0f;		//	Current speed of the vehicle.
	public int crashScore = 0;		//	Current destruction score of the vehicle.
	public float totalSlip = 0f;		//	Current total slip of the vehicle.

	#region Particles

	[Header("Particles")]
	/// <summary>
	/// All particles.
	/// </summary>
	public ParticleSystem[] boostParticles;
	public ParticleSystem[] contactParticles;
	public ParticleSystem wheelParticles;
	public ParticleSystem speedingParticles;

	#endregion

	#region Parts

	/// <summary>
	/// All detachable parts.
	/// </summary>
	[System.Serializable]
	public class DetachableParts{

		public GameObject part;

	}

	[Header("Parts")]
	public DetachableParts[] detachableParts;

	[System.Serializable]
	public class DetachableJoints{

		public FixedJoint joint;

	}
		
	public DetachableJoints[] detachableFixedJoints;

	[System.Serializable]
	public class DetachableJoint{

		public ConfigurableJoint joint;

		internal ConfigurableJointMotion jointMotionAngularX;
		internal ConfigurableJointMotion jointMotionAngularY;
		internal ConfigurableJointMotion jointMotionAngularZ;

		internal ConfigurableJointMotion jointMotionX;
		internal ConfigurableJointMotion jointMotionY;
		internal ConfigurableJointMotion jointMotionZ;

		public int strength = 25000;		//	Strength of the part.

	}

	public DetachableJoint[] detachableJoints;

	#endregion

	#region Audio

	[Header("Audio")]
	private AudioSource engineSource;		// Audiosource for engine SFX.
	public AudioClip engineClip;				// Audioclip for engine SFX.

	private AudioSource crashClipSource;		// Audiosource for crash SFX.
	public AudioClip[] crashClip;				// Audioclip for crash SFX.

	private AudioSource windClipSource;		// Audiosource for wind SFX.
	public AudioClip windClip;				// Audioclip for wind SFX.

	private AudioSource engineStartSource;		// Audiosource for engine start SFX.
	public AudioClip engineStartClip;		// Audioclip for engine start SFX.

	private AudioSource nosClipSource;		// Audiosource for boost SFX.
	public AudioClip nosClip;		// Audioclip for boost SFX.

	private AudioSource skidSource;		// Audiosource for tire skid SFX.
	public AudioClip skidClip;				// Audioclip for tire skid SFX.

	public float minEngineSoundPitch = .25f;		//	Minimum engine sound pitch.
	public float maxEngineSoundPitch = 1.25f;		//	Maximum engine sound pitch.

	#endregion

	#region Damage

	[Header("Damage")]
	public MeshFilter[] deformableMeshFilters;		// Deformable Meshes.
	public float randomizeVertices = 1f;		// Randomize Verticies on Collisions for more complex deforms.
	public float damageRadius = .5f;		// Verticies in this radius will be effected on collisions.
	public float damageMultiplier = 1f;		// Damage multiplier.
	private Vector3 localVector;
	struct originalMeshVerts{public Vector3[] meshVerts;}		// Struct for Original Mesh Verticies positions.
	private originalMeshVerts[] originalMeshData;		// Array for struct above.

	#endregion

	#region Upgrades

	/// <summary>
	/// Gets the current engine level.
	/// </summary>
	/// <value>The current engine level.</value>
	public int currentEngineLevel{

		get{

			return PlayerPrefs.GetInt (transform.name + "_EngineLevel", 0);

		}

	}

	/// <summary>
	/// Gets the current boost level.
	/// </summary>
	/// <value>The current boost level.</value>
	public int currentBoostLevel{

		get{

			return PlayerPrefs.GetInt (transform.name + "_BoostLevel", 0);

		}

	}

	/// <summary>
	/// Gets the current bonus level.
	/// </summary>
	/// <value>The current bonus level.</value>
	public int currentBonusLevel{

		get{

			return PlayerPrefs.GetInt (transform.name + "_BonusLevel", 0);

		}

	}

	[Header("Upgrades")]
	public int maximumPriceForEngineUpgrade = 10000;		//	Maximum purchase price of this upgrade.
	public int maximumPriceForBoostUpgrade = 10000;		//	Maximum purchase price of this upgrade.
	public int maximumPriceForBonusUpgrade = 10000;		//	Maximum purchase price of this upgrade.

	/// <summary>
	/// Gets the next price for engine upgrade.
	/// </summary>
	/// <value>The next price for engine upgrade.</value>
	public int nextPriceForEngineUpgrade{

		get
		{
			return GetPrice(currentEngineLevel);
			//return (int)(maximumPriceForEngineUpgrade * Mathf.Lerp (.1f, 1f, (float)currentEngineLevel / 100f));

		}

	}

	private int GetPrice(int level)
	{
		return 500 + level * 250;
	}

	/// <summary>
	/// Gets the next price for boost upgrade.
	/// </summary>
	/// <value>The next price for boost upgrade.</value>
	public int nextPriceForBoostUpgrade{

		get{
			return GetPrice(currentBoostLevel);

			return (int)(maximumPriceForBoostUpgrade * Mathf.Lerp (.1f, 1f, (float)currentBoostLevel / 100f));

		}

	}

	/// <summary>
	/// Gets the next price for bonus upgrade.
	/// </summary>
	/// <value>The next price for bonus upgrade.</value>
	public int nextPriceForBonusUpgrade{

		get{
			return GetPrice(currentBonusLevel);

			return (int)(maximumPriceForBonusUpgrade * Mathf.Lerp (.1f, 1f, (float)currentBonusLevel / 100f));

		}

	}

	#endregion

    void Awake(){

		rigid = GetComponent<Rigidbody> ();		//	Getting rigidbody of the vehicle.
		rigid.centerOfMass = transform.InverseTransformPoint(COM.transform.position);		//	And setting centre of mass of this rigidbody.

		CheckLayers ();		//	Checks all the layers.
		CheckWheelColliders ();		//	Checks all wheelcolliders at start.
		CreateAudioSources ();		//	Creates and checks all audiosources at start.
		CreateParticles ();		//	Creates and checks all particles at start.
		CreateParts ();		//	Checks all detachable parts at start.
		CreateSkidmarks ();		//	Creates skidmarks manager at start.
		DamageInit();		//	Initializes all deformable meshes at start.
        
    }

	/// <summary>
	/// Checks all the layers.
	/// </summary>
	private void CheckLayers(){

		if (string.IsNullOrEmpty (C_Settings.Instance.playerLayer)) {
			Debug.LogError ("Player Layer is missing in Settings. Go to Stunt Crasher --> Edit Settings, and set the layer of Player.");
			return;
		}

		if (string.IsNullOrEmpty (C_Settings.Instance.playerTag)) {
			Debug.LogError ("Player Tag is missing in Settings. Go to Stunt Crasher --> Edit Settings, and set the tag of Player.");
			return;
		}

		Transform[] allTransforms = gameObject.GetComponentsInChildren<Transform>(true);

		foreach (Transform t in allTransforms) {

			int layerInt = LayerMask.NameToLayer (C_Settings.Instance.playerLayer);

			if (layerInt >= 0 && layerInt <= 31) {

				t.gameObject.layer = LayerMask.NameToLayer (C_Settings.Instance.playerLayer);
				transform.gameObject.tag = C_Settings.Instance.playerTag;

			} else {

				Debug.LogError ("Player Layer selected in Settings doesn't exist on your Tags & Layers. Go to Edit --> Project Settings --> Tags & Layers, and create a new layer named ''" + C_Settings.Instance.playerLayer + "''.");

				foreach (Transform tr in allTransforms)
					tr.gameObject.layer = LayerMask.NameToLayer ("Default");

				return;

			}

		}

		WheelCollider[] allWheelColliders = gameObject.GetComponentsInChildren<WheelCollider>(true);

		foreach (WheelCollider t in allWheelColliders) {

			int layerInt = LayerMask.NameToLayer (C_Settings.Instance.wheelColliderLayer);

			if (layerInt >= 0 && layerInt <= 31) {

				t.gameObject.layer = LayerMask.NameToLayer (C_Settings.Instance.wheelColliderLayer);

			} else {

				Debug.LogError ("WheelCollider Layer selected in Settings doesn't exist on your Tags & Layers. Go to Edit --> Project Settings --> Tags & Layers, and create a new layer named ''" + C_Settings.Instance.wheelColliderLayer + "''.");

				foreach (WheelCollider tr in allWheelColliders)
					tr.gameObject.layer = LayerMask.NameToLayer ("Default");

				return;

			}

		}

	}

	/// <summary>
	/// Make sure your wheelcolliders are stable and stable.
	/// </summary>
	private void CheckWheelColliders(){

		for (int i = 0; i < wheels.Length; i++) {

			wheels [i].wheelCollider.ConfigureVehicleSubsteps (10f, 5, 5);
			wheels [i].wheelCollider.mass = 100f;

		}

	}

	/// <summary>
	/// Creates the audio sources.
	/// </summary>
	private void CreateAudioSources(){

		if(engineClip)
			engineSource = C_AudioSource.NewAudioSource (gameObject, engineClip.name, 5f, 50f, 0f, engineClip, true, true, false);

		if(windClip)
			windClipSource = C_AudioSource.NewAudioSource (gameObject, windClip.name, 5f, 50f, 0f, windClip, true, true, false);

		if(nosClip)
			nosClipSource = C_AudioSource.NewAudioSource (gameObject, nosClip.name, 5f, 50f, 0f, nosClip, true, true, false);

		if(skidClip)
			skidSource = C_AudioSource.NewAudioSource(gameObject, "Skid Sound AudioSource", 5f, 50f, 0, skidClip, true, true, false);
		 
	}

	/// <summary>
	/// Creates the particles.
	/// </summary>
	private void CreateParticles(){

		for (int i = 0; i < wheels.Length; i++) {

			if (!wheelParticles)
				break;

			GameObject ps = (GameObject)Instantiate (wheelParticles.gameObject, wheels[i].wheelCollider.transform.position, wheels[i].wheelCollider.transform.rotation) as GameObject;
			wheels [i].particles = ps.GetComponent<ParticleSystem> ();
			wheels[i].emission = ps.GetComponent<ParticleSystem> ().emission;
			wheels[i].emission.enabled = false;
			ps.transform.SetParent (wheels[i].wheelCollider.transform, false);
			ps.transform.localPosition = Vector3.zero;
			ps.transform.localRotation = Quaternion.identity;

		}

	}

	/// <summary>
	/// Creates the parts.
	/// </summary>
	private void CreateParts(){

		//	Getting default settings of the part.
		for (int i = 0; i < detachableJoints.Length; i++) {

			detachableJoints [i].jointMotionAngularX = detachableJoints [i].joint.angularXMotion;
			detachableJoints [i].jointMotionAngularY = detachableJoints [i].joint.angularYMotion;
			detachableJoints [i].jointMotionAngularZ = detachableJoints [i].joint.angularZMotion;

			detachableJoints [i].jointMotionX = detachableJoints [i].joint.xMotion;
			detachableJoints [i].jointMotionY = detachableJoints [i].joint.yMotion;
			detachableJoints [i].jointMotionZ = detachableJoints [i].joint.zMotion;

		}

		//	Locking the part.
		for (int i = 0; i < detachableJoints.Length; i++) {

			detachableJoints [i].joint.angularXMotion = ConfigurableJointMotion.Locked;
			detachableJoints [i].joint.angularYMotion = ConfigurableJointMotion.Locked;
			detachableJoints [i].joint.angularZMotion = ConfigurableJointMotion.Locked;

			detachableJoints [i].joint.xMotion = ConfigurableJointMotion.Locked;
			detachableJoints [i].joint.yMotion = ConfigurableJointMotion.Locked;
			detachableJoints [i].joint.zMotion = ConfigurableJointMotion.Locked;

			detachableJoints [i].joint.breakForce = detachableJoints [i].strength;
			detachableJoints [i].joint.breakTorque = detachableJoints [i].strength;

		}

	}

	/// <summary>
	/// Damages the parts.
	/// </summary>
	private void DamageParts(){

		// Unlocking the parts and set their joint configuration to default.
		for (int i = 0; i < detachableJoints.Length; i++) {

			if (detachableJoints [i].joint) {

				detachableJoints [i].joint.angularXMotion = detachableJoints [i].jointMotionAngularX;
				detachableJoints [i].joint.angularYMotion = detachableJoints [i].jointMotionAngularY;
				detachableJoints [i].joint.angularZMotion = detachableJoints [i].jointMotionAngularZ;

				detachableJoints [i].joint.xMotion = detachableJoints [i].jointMotionX;
				detachableJoints [i].joint.yMotion = detachableJoints [i].jointMotionY;
				detachableJoints [i].joint.zMotion = detachableJoints [i].jointMotionZ;

			}

		}

	}

	/// <summary>
	/// Creates the skidmarks.
	/// </summary>
	private void CreateSkidmarks(){

		// Getting skidmarks manager if exits.
		skidmarks = GameObject.FindObjectOfType<C_Skidmarks> ();

		// If there are no skidmarks manager, create it.
		if (!skidmarks) 
			skidmarks = ((GameObject)Instantiate (C_Settings.Instance.SkidmarksManager.gameObject, Vector3.zero, Quaternion.identity)).gameObject.GetComponent<C_Skidmarks>();

	}

    void Update(){

		Inputs ();		//	Processing inputs received from player.
		Audio ();		//	Processing audio of the all audiosources.
		Wheels ();		//	Processing wheels.
		Particles ();		//	Processing particles.
        
    }

	void FixedUpdate(){

		speed = rigid.velocity.magnitude * 3.6f;		//	Getting speed of the vehicle.

		Torques ();		//	Processing torques.
		Skidmarks ();		//	Processing skidmarks.
		Stability ();		//	Stability.

	}

	private void Inputs(){

		// If vehicle is controllable now, take the inputs from C_InputManager. If not, make the input 0.
		if (isControllable) {

			motorInput = C_InputManager.Instance.motorInput;
			motorInput = Mathf.Clamp01 (motorInput);

		} else {

			motorInput = 0f;

		}

	}

	/// <summary>
	/// Audio this instance.
	/// </summary>
	private void Audio(){

		float targetVolume = isControllable ? 1f : 0f;

		if (engineSource) {
			
			engineSource.volume = Mathf.Lerp (engineSource.volume, targetVolume, Time.deltaTime);
			engineSource.pitch = Mathf.Lerp (minEngineSoundPitch, maxEngineSoundPitch, speed / 150f);

		}

		if(windClipSource)
			windClipSource.volume = Mathf.Lerp (0f, 1f, speed / 150f);

		if(skidSource)
			skidSource.volume = totalSlip;

	}

	private void Torques(){

		// Processing wheels.

		for (int i = 0; i < wheels.Length; i++) {

			if (wheels [i].canTorque)
				wheels [i].wheelCollider.motorTorque = (motorTorque * motorInput) * Mathf.Lerp (1f, 10f, currentEngineLevel / 100f);
			else
				wheels [i].wheelCollider.motorTorque = 0f;

			if(wheels[i].canBrake)
				wheels [i].wheelCollider.brakeTorque = brakeTorque * Mathf.Lerp(1f, 0f, motorInput);
			else
				wheels [i].wheelCollider.brakeTorque = 0;

		}

	}

	private void Skidmarks(){

		// If scene has skidmarks manager...
		if(skidmarks){

			for (int i = 0; i < wheels.Length; i++) {

				WheelHit GroundHit;
				wheels [i].wheelCollider.GetGroundHit (out GroundHit);

				if (wheels [i].slip > .25f) {

					Vector3 skidPoint = GroundHit.point + 2f * (rigid.velocity) * Time.deltaTime;

					if (rigid.velocity.magnitude > 1f) {
						wheels[i].lastSkidmark = skidmarks.AddSkidMark (skidPoint, GroundHit.normal, wheels [i].slip, wheels[i].lastSkidmark);
					} else {
						wheels[i].lastSkidmark = -1;
					}

				} else {

					wheels[i].lastSkidmark = -1;

				}

			}

		}

	}

	private void Wheels(){

		// First, we are getting groundhit data.
		RaycastHit hit;
		WheelHit CorrespondingGroundHit;

        float totalWheelSlip = 0f;

		for (int i = 0; i < wheels.Length; i++) {

			// Taking WheelCollider center position.
			Vector3 ColliderCenterPoint = wheels[i].wheelCollider.transform.TransformPoint(wheels[i].wheelCollider.center);
			wheels[i].wheelCollider.GetGroundHit(out CorrespondingGroundHit);

			// Here we are raycasting to downwards.
			if(Physics.Raycast(ColliderCenterPoint, -wheels[i].wheelCollider.transform.up, out hit, (wheels[i].wheelCollider.suspensionDistance + wheels[i].wheelCollider.radius) * transform.localScale.y) && !hit.transform.IsChildOf(transform) && !hit.collider.isTrigger){
				
				// Assigning position of the wheel if we have hit.
				wheels[i].wheelModel.transform.position = hit.point + (wheels[i].wheelCollider.transform.up * wheels[i].wheelCollider.radius) * transform.localScale.y;

			}else{
				
				// Assigning position of the wheel to default position if we don't have hit.
				wheels[i].wheelModel.transform.position = ColliderCenterPoint - (wheels[i].wheelCollider.transform.up * wheels[i].wheelCollider.suspensionDistance) * transform.localScale.y;

			}

			// X axis rotation of the wheel.
			wheels[i].wheelRotation += wheels[i].wheelCollider.rpm * 6 * Time.deltaTime;

			// Assigning rotation of the wheel (X and Y axises).
			wheels[i].wheelModel.transform.rotation = wheels[i].wheelCollider.transform.rotation * Quaternion.Euler(wheels[i].wheelRotation, wheels[i].wheelCollider.steerAngle, wheels[i].wheelCollider.transform.rotation.z);

			wheels [i].slip = Mathf.Abs(CorrespondingGroundHit.forwardSlip) + Mathf.Abs(CorrespondingGroundHit.sidewaysSlip);
			wheels [i].slip = Mathf.Clamp01 (wheels[i].slip);

            totalWheelSlip += wheels[i].slip;

		}

        totalWheelSlip /= wheels.Length;

        totalSlip = totalWheelSlip;
        totalSlip = Mathf.Clamp01(totalSlip);

	}

	private void Particles(){

		// Processing particles.
		for (int i = 0; i < wheels.Length; i++) {

			if (wheels [i].particles != null) {

				if (wheels [i].slip > .5f)
					wheels [i].emission.enabled = true;
				else
					wheels [i].emission.enabled = false;

			}

		}

		if (speedingParticles) {
			
			ParticleSystem.EmissionModule em = speedingParticles.emission;
			em.rateOverTime = Mathf.Lerp (0f, 50f, speed / 200f);

		}

	}

	private void Stability(){

		// If vehicle is controllable, freeze rotation of Y and Z angles.
		if (isControllable) {
			
			transform.rotation = Quaternion.Euler (transform.eulerAngles.x, 0f, 0f);

		}

	}

	void OnTriggerEnter(Collider col){

		//	If vehicle triggers with launcher...
		C_Launcher launcher = col.transform.gameObject.GetComponent<C_Launcher> ();

		if (!launcher)
			return;

		// Call "Launch()" function in C_GameManager.
		if(C_GameManager.Instance)
			C_GameManager.Instance.Launch ();

		// Initiate boost.
		StartCoroutine(Boost ());

	}

	IEnumerator Boost(){

		// If timer is bigger than 0...
		while (boostTime > 0f) {

			boostTime -= Time.deltaTime;		//	Reduce boost time.
			rigid.AddRelativeForce ((Vector3.forward * boostTorque * boostTime) * Mathf.Lerp(1f, 10f, currentBoostLevel / 100f), ForceMode.Force);		//	Apply force.

			// Enabling boost particles.
			foreach (var item in boostParticles) {

				ParticleSystem.EmissionModule em = item.emission;
				em.enabled = true;

			}

			//	Enabling boost volume.
			if(nosClipSource)
				nosClipSource.volume = 1f;
			
			yield return null;

		}

		// Boost time is 0 now.
		// Disabling boost particles.
		foreach (var item in boostParticles) {

			ParticleSystem.EmissionModule em = item.emission;
			em.enabled = false;

		}

		//	Disabling boost volume.
		if(nosClipSource)
			nosClipSource.volume = 0f;

	}

	/// <summary>
	/// Reduces the wheel friction after crash.
	/// </summary>
	private void ReduceWheelFriction(){

		for (int i = 0; i < wheels.Length; i++) {

			WheelFrictionCurve curve = wheels[i].wheelCollider.forwardFriction;
			curve.stiffness = .5f;
			wheels[i].wheelCollider.forwardFriction = curve;

			curve = wheels[i].wheelCollider.sidewaysFriction;
			curve.stiffness = .5f;
			wheels[i].wheelCollider.sidewaysFriction = curve;

		}

	}

	/// <summary>
	/// Steers the wheel after crash.
	/// </summary>
	private void SteerWheel(){

		float randomAngle = Random.Range (-45f, 45f);

		for (int i = 0; i < wheels.Length; i++) {

			if (wheels [i].canSteering && wheels[i].wheelCollider.steerAngle != 0)
				wheels [i].wheelCollider.steerAngle = randomAngle;

		}

	}

	void OnCollisionEnter(Collision col){

		// Creating randomly crash SFX.
		if (crashClip != null && crashClip.Length >= 1) {
			
			int randomizedCrashClip = Random.Range (0, crashClip.Length - 1);
			C_AudioSource.NewAudioSource (gameObject, crashClip [randomizedCrashClip].name, 5f, 50f, 1f, crashClip [randomizedCrashClip], false, true, true);

		}

		//	Creating contact particles.
		foreach (var item in contactParticles) {

			item.transform.position = col.GetContact (0).point;
			item.Emit (10);

		}

		Vector3 colVelocity = rigid.velocity;		//	Velocity of the vehicle.

		//	Damping velocity of the X axis.
		if (colVelocity.x >= 1f)
			rigid.velocity = new Vector3 (1f, rigid.velocity.y, rigid.velocity.z);

		//	Damping velocity of the X axis.
		if (colVelocity.x <= -1f)
			rigid.velocity = new Vector3 (-1f, rigid.velocity.y, rigid.velocity.z);

		//	Crashed now.
		if (!isControllable)
			isCrashed = true;

		C_Obstable obstacleHit = col.collider.transform.gameObject.GetComponentInParent<C_Obstable> ();		//	If vehicle collides with an obstacle...

		// Enabling isCrashed boolean of the obstacle, and increase obstacle score.
		if (obstacleHit && !obstacleHit.isCrashed) {

			crashScore += (obstacleHit.points);
			obstacleHit.isCrashed = true;

		}

		//	If vehicle is crashed...
		if (isCrashed) {

			ReduceWheelFriction ();		//	Reduce wheel frictions.
			SteerWheel ();		//	Steer randomly.
			DamageParts ();		//	Damage parts.

//			for (int i = 0; i < detachableFixedJoints.Length; i++) {
//
//				detachableFixedJoints [i].joint.transform.SetParent (null);
////				detachableFixedJoints [i].joint.GetComponent<Collider> ().enabled = true;
//			
//			}
//
//			for (int i = 0; i < detachableFixedJoints.Length; i++) {
//
//				if(detachableFixedJoints[i] != null)
//					Destroy (detachableFixedJoints[i].joint);
//
//			}

		}

		Vector3 colRelVel = col.relativeVelocity;
		colRelVel *= 1f - Mathf.Abs(Vector3.Dot(transform.up,col.contacts[0].normal));

		float cos = Mathf.Abs(Vector3.Dot(col.contacts[0].normal, colRelVel.normalized));

		if (colRelVel.magnitude * cos >= 5f){

			localVector = transform.InverseTransformDirection(colRelVel) * (damageMultiplier / 50f);

			if (originalMeshData == null)
				LoadOriginalMeshData();

			for (int i = 0; i < deformableMeshFilters.Length; i++){
				DeformMesh(deformableMeshFilters[i].mesh, originalMeshData[i].meshVerts, col, cos, deformableMeshFilters[i].transform, Quaternion.identity);
			}

		}

	}

	/// <summary>
	/// Collecting all meshes for damage.
	/// </summary>
	private void DamageInit (){

		if (deformableMeshFilters.Length == 0){

			MeshFilter[] allMeshFilters = GetComponentsInChildren<MeshFilter>();
			List <MeshFilter> properMeshFilters = new List<MeshFilter>();

			foreach (MeshFilter mf in allMeshFilters) {

				properMeshFilters.Add (mf);

			}

			deformableMeshFilters = properMeshFilters.ToArray();

		}

		LoadOriginalMeshData();

	}

	/// <summary>
	/// Loads the original mesh data. Default mesh vertices positions. Used for repairing the car.
	/// </summary>
	private void LoadOriginalMeshData(){

		originalMeshData = new originalMeshVerts[deformableMeshFilters.Length];

		for (int i = 0; i < deformableMeshFilters.Length; i++)
			originalMeshData[i].meshVerts = deformableMeshFilters[i].mesh.vertices;

	}

	/// <summary>
	/// Deforms the mesh.
	/// </summary>
	/// <param name="mesh">Mesh.</param>
	/// <param name="originalMesh">Original mesh.</param>
	/// <param name="collision">Collision.</param>
	/// <param name="cos">Cos.</param>
	/// <param name="meshTransform">Mesh transform.</param>
	/// <param name="rot">Rot.</param>
	void DeformMesh(Mesh mesh, Vector3[] originalMesh, Collision collision, float cos, Transform meshTransform, Quaternion rot){

		Vector3[] vertices = mesh.vertices;

		foreach (ContactPoint contact in collision.contacts){

			Vector3 point = meshTransform.InverseTransformPoint(contact.point);

			for (int i = 0; i < vertices.Length; i++){

				if ((point - vertices[i]).magnitude < damageRadius){
					vertices[i] += rot * ((localVector * (damageRadius - (point - vertices[i]).magnitude) / damageRadius) * cos + (new Vector3(Mathf.Sin(vertices[i].y * 1000), Mathf.Sin(vertices[i].z * 1000), Mathf.Sin(vertices[i].x * 1000)).normalized * (randomizeVertices / 500f)));

					if (.5f > 0 && ((vertices[i] - originalMesh[i]).magnitude) > .5f){
						vertices[i] = originalMesh[i] + (vertices[i] - originalMesh[i]).normalized * (.5f);
					}
					
				}

			}

		}

		mesh.vertices = vertices;
		mesh.RecalculateNormals();
		mesh.RecalculateBounds();
		;

	}

	void Reset(){

		FirstTimeSetup ();

	}

	/// <summary>
	/// Firsts time setup.
	/// </summary>
	private void FirstTimeSetup(){

		print ("First time setup for " + transform.name);

		bool newCOMFound = GameObject.Find ("COM");

		if (!newCOMFound) {
			
			GameObject newCOM = new GameObject ("COM");
			newCOM.transform.SetParent (transform, false);
			newCOM.transform.localPosition = Vector3.zero;
			newCOM.transform.localRotation = Quaternion.identity;
			newCOM.transform.localScale = Vector3.one;
			COM = newCOM.transform;

		} else {

			COM = GameObject.Find ("COM").transform;

		}

		boostParticles = new ParticleSystem[2];

		for (int i = 0; i < boostParticles.Length; i++) {

			GameObject newBoostPartices = GameObject.Instantiate (C_Settings.Instance.boostParticles, transform.position, transform.rotation, transform) as GameObject;
			newBoostPartices.transform.localRotation = transform.rotation * Quaternion.Euler (0f, 180f, 0f);
			newBoostPartices.transform.localPosition += newBoostPartices.transform.forward * 3f;
			boostParticles[i] = newBoostPartices.GetComponent<ParticleSystem> ();

		}

		contactParticles = new ParticleSystem[2];

		for (int i = 0; i < contactParticles.Length; i++) {

			GameObject newContactParticles = GameObject.Instantiate (C_Settings.Instance.collisionParticles, transform.position, transform.rotation, transform) as GameObject;
			contactParticles[i] = newContactParticles.GetComponent<ParticleSystem> ();

		}

		speedingParticles = new ParticleSystem();

		GameObject newSpeedingParticles = GameObject.Instantiate (C_Settings.Instance.speedingParticles, transform.position, transform.rotation, transform) as GameObject;
		newSpeedingParticles.transform.localRotation = transform.rotation * Quaternion.Euler (0f, 180f, 0f);
		speedingParticles = newSpeedingParticles.GetComponent<ParticleSystem> ();

		GameObject newWheelSlipPartices = C_Settings.Instance.wheelParticles;
		wheelParticles = newWheelSlipPartices.GetComponent<ParticleSystem> ();

		engineClip = C_Settings.Instance.engineClip;
		engineStartClip = C_Settings.Instance.engineStartClip;
		nosClip = C_Settings.Instance.boostClip;
		skidClip = C_Settings.Instance.wheelSkidClip;
		windClip = C_Settings.Instance.windClip;
		crashClip = new AudioClip[C_Settings.Instance.crashClip.Length];

		for (int i = 0; i < crashClip.Length; i++)
			crashClip [i] = C_Settings.Instance.crashClip [i];

		rigid = GetComponent<Rigidbody> ();

		if (!rigid) {

			rigid = gameObject.AddComponent<Rigidbody> ();
			rigid.interpolation = RigidbodyInterpolation.Interpolate;
			rigid.mass = 1500f;
			rigid.drag = 0f;
			rigid.angularDrag = 0f;

		}

	}

}
