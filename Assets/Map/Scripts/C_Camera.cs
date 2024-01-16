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
/// Player camera that follows player vehicle with desired settings.
/// </summary>
public class C_Camera : MonoBehaviour{

	private Camera cam;	//	Actual camera.
	public C_CarController playerCar;	//	Player vehicle.
	private GameObject targetTransform;	//	Target transform to follow.

	public bool canTrack = true;	//	Can follow player vehicle now?
	public bool canOrbit = false;	//	Can orbit now?

	public float distance = 10f;	//	Distance to player vehicle.
	public float height = 2.5f;	//	Height to player vehicle.

	//	Rotation of the camera.
	public Vector3 rotation = new Vector3(20f, 0f, 0f);

	// Field of view.
	public float targetFOV = 45f;
	public float minFOV = 50f;
	public float maxFov = 90f;

    private Vector3 refVelocity = Vector3.zero;

    // Start is called before the first frame update
    void Start(){

		// Getting actual camera parented to pivot.
		cam = GetComponentInChildren<Camera> ();
		//	Creating a new transform for following.
		targetTransform = new GameObject ("Camera Target");
         
    }

	void OnEnable(){

		C_GameManager.OnGameStarted += C_GameManager_OnGameStarted;
		C_GameManager.OnGameFinished += C_GameManager_OnGameFinished;

	}

	/// <summary>
	/// When the game started.
	/// </summary>
	void C_GameManager_OnGameStarted (){

		canTrack = true;	//	Track player vehicle now.
		canOrbit = false;	//	And make sure orbit is disabled too.
		
	}

	/// <summary>
	/// When the game finished.
	/// </summary>
	void C_GameManager_OnGameFinished (){

//		canTrack = false;
		canOrbit = true;	//	Orbit the camera when player vehicle was totally crashed.

	}

    void FixedUpdate(){

		//	If not tracking, return.
		if (!canTrack)
			return;

		//	If there are no player vehicle, return.
		if (!playerCar)
			return;

		// If orbiting...
		if (canOrbit) {

			targetTransform.transform.position = playerCar.transform.position;
//			targetTransform.transform.rotation = playerCar.transform.rotation;

			targetTransform.transform.position -= transform.rotation * Vector3.forward * distance;
			targetTransform.transform.position += transform.rotation * Vector3.up;

			targetTransform.transform.RotateAround (targetTransform.transform.position, Vector3.up, Time.deltaTime * 10f);

			cam.fieldOfView = Mathf.Lerp (cam.fieldOfView, 30f, Time.deltaTime * 1f);

            transform.position = Vector3.SmoothDamp(transform.position, targetTransform.transform.position, ref refVelocity, .05f);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetTransform.transform.rotation, Time.deltaTime * 5f);

            return;

		}

		// Setting target transform same with player vehicle transform.
		targetTransform.transform.position = playerCar.transform.position;
		targetTransform.transform.rotation = playerCar.transform.rotation;

		//	Setting distance and height of the target transform.
		targetTransform.transform.position += transform.rotation * Vector3.up * height;
		targetTransform.transform.position -= transform.rotation * Vector3.forward * distance;

        // Rotation of the target transform.
        targetTransform.transform.rotation = Quaternion.Euler(rotation);

		// Assigning field of view.
		targetFOV = Mathf.Lerp (minFOV, maxFov, playerCar.speed / 200f);
		cam.fieldOfView = Mathf.Lerp (cam.fieldOfView, targetFOV, Time.deltaTime * 1f);

        transform.position = Vector3.SmoothDamp(transform.position, targetTransform.transform.position, ref refVelocity, .05f);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetTransform.transform.rotation, Time.deltaTime * 5f);

    }

	void OnDisable(){

		C_GameManager.OnGameStarted -= C_GameManager_OnGameStarted;
		C_GameManager.OnGameFinished -= C_GameManager_OnGameFinished;

	}

}
