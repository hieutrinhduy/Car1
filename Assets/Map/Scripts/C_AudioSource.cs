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
/// Used for creating new audiosources at runtime.
/// </summary>
public class C_AudioSource : MonoBehaviour{
	
	/// <summary>
	/// Creates new audiosource with specified settings.
	/// </summary>
	public static AudioSource NewAudioSource(GameObject go, string audioName, float minDistance, float maxDistance, float volume, AudioClip audioClip, bool loop, bool playNow, bool destroyAfterFinished){

		GameObject audioSourceObject = new GameObject(audioName);
		audioSourceObject.AddComponent<AudioSource>();
		AudioSource source = audioSourceObject.GetComponent<AudioSource> ();

        if (go){

            source.transform.position = go.transform.position;
            source.transform.rotation = go.transform.rotation;
            source.transform.parent = go.transform;

        }else{

            source.transform.position = Vector3.zero;
            source.transform.rotation = Quaternion.identity;
            source.transform.parent = null;

        }

        //audioSource.GetComponent<AudioSource>().priority =1;
        source.minDistance = minDistance;
		source.maxDistance = maxDistance;
		source.volume = volume;
		source.clip = audioClip;
		source.loop = loop;
		source.dopplerLevel = .5f;

		if(minDistance == 0 && maxDistance == 0)
			source.spatialBlend = 0f;
		else
			source.spatialBlend = 1f;

		if (playNow) {
			source.playOnAwake = true;
			source.Play ();
		} else {
			source.playOnAwake = false;
		}

		if(destroyAfterFinished){
			if(audioClip)
				Destroy(audioSourceObject, audioClip.length);
			else
				Destroy(audioSourceObject);
		}

        if (go){

            if (go.transform.Find("All Audio Sources")){

                audioSourceObject.transform.SetParent(go.transform.Find("All Audio Sources"));

            }else{

                GameObject allAudioSources = new GameObject("All Audio Sources");
                allAudioSources.transform.SetParent(go.transform, false);
                audioSourceObject.transform.SetParent(allAudioSources.transform, false);

            }

        }

		return source;

	}

}
