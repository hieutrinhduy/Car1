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
using UnityEngine.UI;

/// <summary>
/// UI points.
/// </summary>
public class C_UIPoints : MonoBehaviour {

	private Text text;		//	UI text.
	public float lifeTime = 1f;		//	Active life time.

	void Awake () {

		text = GetComponentInChildren<Text> ();		//	Getting Text.

	}

	void OnEnable(){

		Destroy (gameObject, lifeTime);		//	Destroying object after life time.

	}

	public void Display (int points) {

		text.text = points.ToString ();		//	Displaying points.

	}

}
