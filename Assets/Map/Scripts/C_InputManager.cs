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
/// Input manager. Gets input from axis or UI button. C_CarController is using "motorInput" variable in this script.
/// </summary>
public class C_InputManager : MonoBehaviour{

	#region Singleton
	private static C_InputManager instance;
	public static C_InputManager Instance{	get{if(instance == null) instance = GameObject.FindObjectOfType<C_InputManager>(); return instance;}}
	#endregion

	// Mobile gas button.
	public C_UIController gas;

	// Actual input of motor.
	public float motorInput = 0f;

	void Awake(){

		//	If mobile controller is not enabled in C_Settings, destroy mobile UI controller.
		if (!C_Settings.Instance.isMobileController) {

			Destroy (gas.gameObject);
			return;

		}

	}

    public void Update(){

		//	If mobile controller enabled, take the input from the button. If not, take it from axis.
		if (C_Settings.Instance.isMobileController) 
			motorInput = gas.input;
		else
			motorInput = Input.GetAxis ("Vertical");
        
    }

}
