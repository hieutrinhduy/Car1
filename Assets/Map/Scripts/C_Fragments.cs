//----------------------------------------------
//           		 Stunt Crasher
//
// Copyright © 2014 - 2020 BoneCracker Games
// http://www.bonecrackergames.com
// Buğra Özdoğanlar
//
//----------------------------------------------

using UnityEngine;
using System.Collections;

/// <summary>
/// Fragments used for breakable walls.
/// </summary>
public class C_Fragments : MonoBehaviour {

	private Rigidbody rigid;		//	Rigid.

	private bool broken = false;		//	Is broken?

	void Awake () {

		//	Getting rigid, enabling isKinematic to make it static, and forcing it to sleep.
		rigid = GetComponent<Rigidbody> ();
//		rigid.isKinematic = true;
		rigid.Sleep ();

	}

	void FixedUpdate () {
	
		// If isn't broken, check the structure.
		if(!broken)
			Checking();

	}

	void Checking(){

		// If rigid is sleeping, return.
		if (rigid.IsSleeping())
			return;

	}

	void OnCollisionEnter (Collision collision) {

		// If collision is minimal, return.
		if(collision.relativeVelocity.magnitude < .05f)
			return;

		//	If collision is not fragment, make it non static by disabling isKinematic of the rigid.
		if(collision.transform.gameObject.layer != LayerMask.NameToLayer("Fragment"))
			rigid.isKinematic = false;

	}

}
