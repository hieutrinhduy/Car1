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

public class SC_ButtonSlideAnimation : MonoBehaviour {

	public SlideFrom slideFrom;
	public enum SlideFrom{Left, Right, Top, Buttom}
	public bool actWhenEnabled = false;
	public bool playSound = true;

	private RectTransform getRect;
	private Vector2 originalPosition;
	public bool actNow = false;
	public bool endedAnimation = false;
	public SC_ButtonSlideAnimation playWhenThisEnds;

	private AudioSource slidingAudioSource;

	void Awake () {

		getRect = GetComponent<RectTransform>();
		originalPosition = GetComponent<RectTransform>().anchoredPosition;

		SetOffset();

	}

	void SetOffset(){

		switch(slideFrom){

		case SlideFrom.Left:
			GetComponent<RectTransform>().anchoredPosition = new Vector2(-3000f, originalPosition.y);
			break;
		case SlideFrom.Right:
			GetComponent<RectTransform>().anchoredPosition = new Vector2(3000f, originalPosition.y);
			break;
		case SlideFrom.Top:
			GetComponent<RectTransform>().anchoredPosition = new Vector2(originalPosition.x, 1500f);
			break;
		case SlideFrom.Buttom:
			GetComponent<RectTransform>().anchoredPosition = new Vector2(originalPosition.x, -1500f);
			break;

		}

	}

	void OnEnable(){

		if(actWhenEnabled){
			SetOffset();
			endedAnimation = false;
			Animate();
		}

	}

	public void Animate () {

		slidingAudioSource = C_AudioSource.NewAudioSource(Camera.main.gameObject, C_Settings.Instance.labelSlideAudioClip.name, 0f, 0f, 1f, C_Settings.Instance.labelSlideAudioClip, false, true, true );
		slidingAudioSource.ignoreListenerPause = true;
		slidingAudioSource.ignoreListenerVolume = true;

		actNow = true;

	}

	void Update(){

		if(!actNow || endedAnimation)
			return;

		if(playWhenThisEnds != null && !playWhenThisEnds.endedAnimation)
			return;

		if(slidingAudioSource && !slidingAudioSource.isPlaying && playSound)
			slidingAudioSource.Play();

		getRect.anchoredPosition = Vector2.MoveTowards(getRect.anchoredPosition, originalPosition, Time.unscaledDeltaTime * 5000f);

		if(Vector2.Distance(GetComponent<RectTransform>().anchoredPosition, originalPosition) < .05f){

			if(slidingAudioSource && slidingAudioSource.isPlaying && playSound)
				slidingAudioSource.Stop();

			GetComponent<RectTransform>().anchoredPosition = originalPosition;

			SC_CountAnimation countAnimation = GetComponentInChildren<SC_CountAnimation> ();

			if(countAnimation){
				
				if(!countAnimation.actNow)
					countAnimation.Count();
				
			}else{
				
				endedAnimation = true;

			}

		}

		if(endedAnimation && !actWhenEnabled)
			enabled = false;

	}

}
