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
/// Obstacle with score, name, etc...   
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class C_Obstable : MonoBehaviour{

	public string obstacleName = "Obstacle";	//	 Name of the obstacle.
	public int points = 10;	//	Points of the obstacle.
	public bool isCrashed = false;	//	Is this obstacle crashed before?
	public bool isSleeping = false;

	void Awake(){

		if (isSleeping)
			GetComponentInChildren<Rigidbody> ().Sleep ();

	}

	void Start(){

		CheckLayers ();

	}

	/// <summary>
	/// Checks all the layers.
	/// </summary>
	private void CheckLayers(){

		if (string.IsNullOrEmpty (C_Settings.Instance.obstacleLayer)) {
			Debug.LogError ("Obstacle Layer is missing in Settings. Go to Stunt Crasher --> Edit Settings, and set the layer of Obstacle.");
			return;
		}

		Transform[] allTransforms = gameObject.GetComponentsInChildren<Transform>(true);

		foreach (Transform t in allTransforms) {

			int layerInt = LayerMask.NameToLayer (C_Settings.Instance.obstacleLayer);

			if (layerInt >= 0 && layerInt <= 31) {

				t.gameObject.layer = LayerMask.NameToLayer (C_Settings.Instance.obstacleLayer);

			} else {

				Debug.LogError ("Obstacle Layer selected in Settings doesn't exist on your Tags & Layers. Go to Edit --> Project Settings --> Tags & Layers, and create a new layer named ''" + C_Settings.Instance.obstacleLayer + "''.");
				Debug.LogError ("From now on, ''Setting Tags and Layers'' disabled! You can enable this when you created this layer.");

				foreach (Transform tr in allTransforms)
					tr.gameObject.layer = LayerMask.NameToLayer ("Default");

				return;

			}

		}

	}

}
