//----------------------------------------------
//           		 Stunt Crasher
//
// Copyright © 2014 - 2020 BoneCracker Games
// http://www.bonecrackergames.com
// Buğra Özdoğanlar
//
//----------------------------------------------

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

/// <summary>
/// UI input (float) receiver from UI Button. 
/// </summary>
public class C_UIController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {

	private Button button;

	internal float input;
	public bool pressing;

	void Awake(){

		button = GetComponent<Button> ();

	}

	/// <summary>
	/// Raises the pointer down event.
	/// </summary>
	/// <param name="eventData">Event data.</param>
	public void OnPointerDown(PointerEventData eventData){

		pressing = true;

	}

	/// <summary>
	/// Raises the pointer up event.
	/// </summary>
	/// <param name="eventData">Event data.</param>
	public void OnPointerUp(PointerEventData eventData){

		pressing = false;

	}

	/// <summary>
	/// Raises the press event.
	/// </summary>
	/// <param name="isPressed">If set to <c>true</c> is pressed.</param>
	void OnPress (bool isPressed){

		if(isPressed)
			pressing = true;
		else
			pressing = false;

	}

	void Update(){

		//	If button is not interactable, input is 0.
		if (button && !button.interactable) {

			pressing = false;
			input = 0f;
			return;

		}

		//	If button is interactable, change input.
		if (pressing)
			input += Time.deltaTime * 3f;
		else
			input -= Time.deltaTime * 3f;

		// Clamping input between 0f - 1f.
		if(input < 0f)
			input = 0f;

		if(input > 1f)
			input = 1f;

	}

	void OnDisable(){

		input = 0f;
		pressing = false;

	}

}
