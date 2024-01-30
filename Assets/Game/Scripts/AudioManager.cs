using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using DG.Tweening;
using System;

public class AudioManager : Singleton<AudioManager>
{

    //GameManager gameManager;
    //[HideInInspector] public List<SongInfo> songInfos = new List<SongInfo>();
    //[HideInInspector] public SongInfo curSong;
    public AudioClip MainMenuBGM, LevelSelectShopBGM;
    [Space(20)]

    public AudioClip[] soundEffects;
    public AudioClip[] soundUI;
    public AudioClip[] footSteps;
    
    public AudioClip[] SunraySound;
    public AudioClip[] UFOSound;
    public AudioClip[] DinasaurSound;
   
    public AudioClip[] detachSounds;
    public AudioClip[] coinsSound;
    public AudioClip[] ingameBGM;
    public AudioClip[] winLoseSound;
    public AudioClip[] gunSound;
    public AudioClip RainSound;
    Coroutine sunraySoundCo;
    Coroutine DinasaurFootstepCo;
    Coroutine MamutFootstepCo;
    Coroutine GodzillaFootstepCo;
    Coroutine KongFootstepCo;

    //RacingSound
    [Header("Racing")]
    public AudioClip[] ClickSounds;
    public AudioClip PlayingMusic;
    public AudioClip[] CarEngineLoopingSound;
    public AudioClip[] ChangeCarAudio;
    public AudioClip EngineStartUpAudio;

    public AudioSource CarAcceleration;
    public AudioSource music;
    public AudioSource sound;
    public AudioSource CarEngine;

    private Coroutine demo;
    public AudioClip[] boostAudio;
    public bool IsCarEnginePlaying;

    Action<object> _playSoundOneShot;
    Action<object> _playDetachSound;
    Action<object> _fadeStopSound;

    //public bool isVbration;

    void Start()
    {
        IsCarEnginePlaying = false;
        //gameManager = GameManager.Instance;
        Register();
        //isVbration = (PlayerPrefs.GetInt("Vibration") == 1) ? true : false;
        //#if !UNITY_EDITOR
        //        Vibration.Init();
        //#endif
        StartCoroutine(SetUpSound());
    }
    public void Register()
    {
      
    }
    private void Update()
    {
        //if (CarController.Ins.IsMoving)
        //{
        //    PlayCarEngine();
        //}
    }
    IEnumerator SetUpSound()
    {
        yield return new WaitForSeconds(0.01f);
        //if (GameDataPref.MusicData == 1)
        //{
        //    music.mute = false;
        //}
        //else
        //{
        //    music.mute = true;
        //}
        //if (GameDataPref.SoundData == 1)
        //{
        //    sound.mute = false;
        //}
        //else
        //{
        //    sound.mute = true;
        //}
    }
    public void PlayMainMenuBGM()
    {
        music.clip = MainMenuBGM;
        music.volume = 0f;
        music.loop = true;
        music.Play();
        music.DOFade(1f, 0.5f);
    }
    public void NitroSound()
    {
        sound.clip = CarEngineLoopingSound[0];
        sound.Play();
    }

    public void PlayStageBGM()
    {
        music.clip = LevelSelectShopBGM;
        music.volume = 0f;
        music.Play();
        music.DOFade(0.8f, 0.5f);
    }

    public void PlayIngameBGM(int id)
    {
        //int m = UnityEngine.Random.Range(0, ingameBGM.Length);
        music.clip = ingameBGM[id];
        music.volume = 0f;
        music.Play();
        music.DOFade(0.8f, 0f);
    }

    public void PlayWinLoseSound(int id)
    {
        sound.PlayOneShot(winLoseSound[id]);
        music.Stop();
    }

    public void PlayFootStepSound()
    {
        int m = UnityEngine.Random.Range(0, footSteps.Length);
        sound.PlayOneShot(footSteps[m]);
    }

    public void PlayDetachSound()
    {
        int m = UnityEngine.Random.Range(0, detachSounds.Length);
        sound.PlayOneShot(detachSounds[m]);
    }

    public void PlayCoinsSound()
    {
        int m = UnityEngine.Random.Range(0, coinsSound.Length);
        sound.PlayOneShot(coinsSound[m]);
    }

    public void Play(int id, float time)
    {
        //curSong = songInfos[id];
        //if (curSong == null)
        //    return;
        //song.clip = curSong.audioClip;
        music.clip = soundEffects[id];
        music.volume = 0f;
        music.Play();
        music.DOFade(1f, 3f);
        music.time = time;
    }


    public void PlayDemo(int id)
    {
        Stop();
        //curSong = songInfos[id];
        //if (curSong == null)
        //    return;
        //song.clip = curSong.audioClip;
        demo = StartCoroutine(DemoSong(15f));
    }
    IEnumerator DemoSong(float demoTime)
    {
        while (true)
        {
            music.volume = 0f;
            music.Play();
            music.DOFade(1f, 3f);
            music.time = 0;
            yield return new WaitForSeconds(demoTime);
            music.DOFade(0f, 3f);
            yield return new WaitForSeconds(3f);
        }
    }

    public void Pause(float time)
    {
        music.DOFade(0f, time).SetUpdate(true);
        StartCoroutine(WaitPause(time));
    }
    IEnumerator WaitPause(float time)
    {
        yield return new WaitForSecondsRealtime(time);
        music.Pause();
    }
    public void Resume()
    {
        music.UnPause();
        music.DOFade(1f, 2f).SetUpdate(true);
        //song.time = time;
    }
    public void Stop()
    {
        music.pitch = 1f;
        music.Stop();
        if (demo != null)
        {
            StopCoroutine(demo);
        }
    }
    public void SoundPlayOneShot(int id)
    {
        sound.PlayOneShot(soundEffects[id]);
    }

    public void SoundUIPlay(int id)
    {
        sound.PlayOneShot(soundUI[id]);
    }



    public void FadeStopSound()
    {
        sound.DOFade(0, .2f);
        //music.DOFade(0, 1f);
        StartCoroutine(StopSound());
    }

    public void FadeStopMusic()
    {
        music.DOFade(0, 1f);
        //music.DOFade(0, 1f);
        //StartCoroutine(StopMusic());

    }

    IEnumerator StopMusic()
    {
        yield return new WaitForSeconds(1.5f);
        music.Stop();
        music.volume = 1;
    }

    IEnumerator StopSound()
    {
        yield return new WaitForSeconds(0.2f);
        sound.Stop();
        sound.volume = 1;
        sound.loop = false;
        sound.clip = null;
    }

    public void PlaySunRaySound()
    {
        sunraySoundCo = StartCoroutine(SunRayDelay());
    }

    public void StopSunRaySound()
    {
        StopCoroutine(sunraySoundCo);
        sound.Stop();
        sound.volume = 1;
        sound.loop = false;
        sound.clip = null;
        sound.PlayOneShot(SunraySound[2]);
    }

    IEnumerator SunRayDelay()
    {
        sound.PlayOneShot(SunraySound[0]);
        yield return new WaitForSeconds(1f);
        sound.loop = true;
        sound.clip = SunraySound[1];
        sound.Play();
    }

    public void PlayUFOSound()
    {
        StartCoroutine(UFOSoundDelay());
    }

    public void StopUFOSound()
    {
        sound.Stop();
        sound.volume = 1;
        sound.loop = false;
        sound.clip = null;
    }

    IEnumerator UFOSoundDelay()
    {
        yield return new WaitForSeconds(.5f);
        sound.loop = true;
        sound.clip = UFOSound[0];
        sound.Play();
    }

    //Dinasaur Footsteps
    public void PlayDinasaurFootstepSound()
    {
         DinasaurFootstepCo = StartCoroutine(DinasaurFootstepsSoundDelay());
    }

    public void StopDinasaurFootstepSound()
    {
        StopCoroutine(DinasaurFootstepCo);
        FadeStopSound();
        sound.clip = null;
    }

    IEnumerator DinasaurFootstepsSoundDelay()
    {
        int count = 0;
        while (count<19)
        {
            yield return new WaitForSeconds(.5f);
            sound.PlayOneShot(footSteps[0]);
            count++;
        }
    }

    //MamutFootsteps
    public void PlayMamutFootstepSound()
    {
        MamutFootstepCo = StartCoroutine(MamutFootstepsSoundDelay());
    }

    public void StopMamutFootstepSound()
    {
        StopCoroutine(MamutFootstepCo);
        FadeStopSound();
        sound.clip = null;
    }

    IEnumerator MamutFootstepsSoundDelay()
    {
        while (true)
        {
            yield return new WaitForSeconds(.7f);
            sound.PlayOneShot(footSteps[0]);
        }

    }

    //GodzillaFootstep
    public void PlayGodzillaFootstepSound()
    {
        GodzillaFootstepCo = StartCoroutine(GodzillaFootstepsSoundDelay());
    }

    public void StopGodzillaFootstepSound()
    {
        StopCoroutine(GodzillaFootstepCo);
        FadeStopSound();
        sound.clip = null;
    }

    IEnumerator GodzillaFootstepsSoundDelay()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            sound.PlayOneShot(footSteps[0]);
        }

    }

    //KingKong FootStep
     public void PlayKongFootstepSound()
    {
        KongFootstepCo = StartCoroutine(KongFootstepsSoundDelay());
    }

    public void StopKongFootstepSound()
    {
        StopCoroutine(KongFootstepCo);
        FadeStopSound();
        sound.clip = null;
    }

    IEnumerator KongFootstepsSoundDelay()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            sound.PlayOneShot(footSteps[0]);
        }

    }


    //Dinasaur
    public void PlayDinasaurSound()
    {
        StartCoroutine(DinasaurSoundDelay());
    }

    public void StopDinasaurSound()
    {
        FadeStopSound();
        sound.clip = null;
    }
    IEnumerator DinasaurSoundDelay()
    {
        yield return new WaitForSeconds(.5f);
        sound.loop = true;
        sound.clip = DinasaurSound[0];
        sound.Play();
    }



    //flyDinasaur
    //public void PlayFlyDinaSound()
    //{
    //    StartCoroutine(flyDinaSoundDelay());
    //}

    //IEnumerator flyDinaSoundDelay()
    //{
    //    int r = UnityEngine.Random.Range(0, 1);
    //    yield return new WaitForSeconds(.3f);
    //    sound.loop = false;
    //    sound.clip = flyDinasaurSound[r];
    //    sound.Play();
    //}

    public void SetPlayingMusic() 
    {
        music.loop = true;
        music.clip = PlayingMusic;
        music.Play();
    }

    public void StopPlayingSound()
    {
        music.Stop();
    }


    public void PlayGunSound(int id)
    {
        sound.PlayOneShot(gunSound[id]);
        //        if (isVbration)
        //        {
        //#if !UNITY_EDITOR
        //            Vibration.VibratePop();
        //#endif
        //        }
    }

    public void VibrateStart()
    {
        //        if (isVbration)
        //        {
        //#if !UNITY_EDITOR
        //            Vibration.VibratePop();
        //#endif
        //        }
    }

    public void PlayGunDrawSound(int id)
    {
        //sound.PlayOneShot(gunDrawSound[id]);
    }
    public void PlayTest(AudioClip audio)
    {
        music.PlayOneShot(audio);
    }
    private void OnPlayerPause()
    {
        Pause(0);
    }
    private void OnPlayerResume()
    {
        Resume();
    }
    private void OnPlayerNearDeath()
    {
        Pause(0);
    }
    private void OnPlayerStop()
    {
        Stop();
    }
    private void OnPlayerHitCoin()
    {
        //SoundPlayOneShot(3);
        //        if (isVbration)
        //        {
        //#if !UNITY_EDITOR
        //            Vibration.VibratePop();
        //#endif
        //        }
    }

    //ClickSoundEffect
    public void PlayClickSoundEffect()
    {
        int randomNumber = UnityEngine.Random.Range(0, ClickSounds.Length);
        sound.loop = false;
        sound.clip = ClickSounds[randomNumber];
        sound.Play();
    }

    IEnumerator PlayClickSounds()
    {
        int randomNumber = UnityEngine.Random.Range(0, ClickSounds.Length);
        yield return new WaitForSeconds(0f);
        sound.loop = false;
        sound.clip = ClickSounds[randomNumber];
        sound.Play();
        Debug.Log(randomNumber);
    }
    public void PlayCarLoopSound()
    {
        StartCoroutine(PlayCarLoop());
    }
    IEnumerator PlayCarLoop()
    {
            if (CarController.Ins.IsMoving == true)
            {
                CarEngine.Play();
                yield return new WaitForSeconds(2f);
            }

    }

    //Change car sound
    public void PlayChangeCarSound()
    {
        int index = UnityEngine.Random.Range(0, ChangeCarAudio.Length);
        sound.loop = false;
        sound.clip = ChangeCarAudio[index];
        sound.volume = 0.5f;
        sound.Play();
        sound.DOFade(1f, 0.5f);
        sound.volume = 1f;
    }
    public void PlayNitroSound()
    {
        StartCoroutine(PlayNitro());
    }
    IEnumerator PlayNitro()
    {
        if(CarController.Ins.NitroTimer <= 0)
        {
            sound.clip = boostAudio[2];
            sound.Play();
        }
        else
        {
            CarAcceleration.Play();
            UIManager.Ins.NitroBtn.interactable = false;
            for (int i = 0; i < 3; i++)
            {
                sound.clip = boostAudio[i];
                sound.Play();
                if (i == 1)
                {
                    while(CarController.Ins.NitroTimer > 0)
                    {
                        sound.clip = boostAudio[i];
                        sound.Play();
                        yield return new WaitForSeconds(0.3f);
                    }
                }
                yield return new WaitForSeconds(0.25f);
            }
            UIManager.Ins.NitroBtn.interactable = true;
        }
    }
    public void PlayEngineStart()
    {
        sound.clip = EngineStartUpAudio;
        sound.Play();
    }
}
