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
using UnityEngine.EventSystems;

public class C_ButtonSound : MonoBehaviour, IPointerDownHandler{

	public Button button;

	void Awake(){

		button = GetComponent<Button> ();

	}

	public void OnPointerDown(PointerEventData eventData){

		if (!button.interactable)
			return;

		C_AudioSource.NewAudioSource (null, C_Settings.Instance.purchaseClip.name, 0f, 0f, 1f, C_Settings.Instance.purchaseClip, false, true, true);

	}

}
