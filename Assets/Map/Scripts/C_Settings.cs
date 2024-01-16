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
/// All shared general settings, resources, first time setups, etc... 
/// </summary>
public class C_Settings : ScriptableObject {

	#region singleton
	private static C_Settings instance;
	public static C_Settings Instance{	get{if(instance == null) instance = Resources.Load("ScriptableObjects/C_Settings") as C_Settings; return instance;}}
	#endregion

	public bool isMobileController = false;
    public int targetFrameRate = 60;

	#region Resources

	[Header("Resources")]
	public C_Skidmarks SkidmarksManager;
	public GameObject collisionParticles;
	public GameObject speedingParticles;
	public GameObject wheelParticles;
	public GameObject boostParticles;

	[Space()]

	#endregion

	#region Sounds

	[Header("Sounds")]
	public AudioClip engineClip;
	public AudioClip engineStartClip;
	public AudioClip boostClip;
	public AudioClip wheelSkidClip;
	public AudioClip windClip;
	public AudioClip[] crashClip;
	public AudioClip purchaseClip;
	public AudioClip countingPointsAudioClip;
	public AudioClip labelSlideAudioClip;

	[Space()]

	#endregion

	#region Layers

	[Header("Layers")]
	public string playerTag = "Player";
	public string playerLayer = "PlayerVehicle";
	public string obstacleLayer = "Obstacle";
	public string wheelColliderLayer = "WheelCollider";

	#endregion

}
