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
using UnityEngine.SceneManagement;

/// <summary>
/// API of the package.
/// </summary>
public class C_API : MonoBehaviour{

	public delegate void onPlayerCoinsChanged(int changeAmount);
	public static event onPlayerCoinsChanged OnPlayerCoinsChanged;
	
	public static int GetCurrency () {

		return PlayerPrefs.GetInt ("Currency", 0);

	}

	public static void ConsumeCurrency (int consume) {

		int current = GetCurrency();

		PlayerPrefs.SetInt ("Currency", current - consume);

		if(OnPlayerCoinsChanged != null)
			OnPlayerCoinsChanged (-consume);

	}

	public static void AddCurrency (int add) {
		
		int current = GetCurrency();

		PlayerPrefs.SetInt ("Currency", current + add);

		if(OnPlayerCoinsChanged != null)
			OnPlayerCoinsChanged (add);

	}

	public static bool CheckLevel(int sceneIndex){

		return C_PlayerPrefsX.GetBool ("UnlockedLevel" + sceneIndex.ToString(), false);

	}

	public static void UnlockLevel(int sceneIndex){

		int currentSceneIndex = SceneManager.GetActiveScene ().buildIndex;

		if(sceneIndex < SceneManager.sceneCountInBuildSettings)
			C_PlayerPrefsX.SetBool ("UnlockedLevel" + sceneIndex.ToString(), true);

	}

	public static void NextLevel(int sceneIndex){

		if (sceneIndex < SceneManager.sceneCountInBuildSettings){

            PlayerPrefs.SetInt("SelectedScene", sceneIndex);
            SceneManager.LoadScene (sceneIndex);

        }

    }

}
