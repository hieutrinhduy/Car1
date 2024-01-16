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
/// All selectable vehicles are stored here with prices.
/// </summary>
public class C_PlayerVehicles : ScriptableObject {

	[System.Serializable]
	public class Vehicles{

		public C_CarController carController;
		public int price;

	}

	public Vehicles[] vehicles;

	#region singleton
	private static C_PlayerVehicles instance;
	public static C_PlayerVehicles Instance{	get{if(instance == null) instance = Resources.Load("ScriptableObjects/C_PlayerVehicles") as C_PlayerVehicles; return instance;}}
	#endregion

}
