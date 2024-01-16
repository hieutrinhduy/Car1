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
/// Finish trigger of the level.
/// </summary>
public class C_Finisher : MonoBehaviour{
	
	void OnTriggerEnter(Collider col){

		C_CarController player = col.gameObject.GetComponentInParent<C_CarController> ();

		if (!player)
			return;

		C_GameManager.Instance.gameCompleted = true;

	}

}
