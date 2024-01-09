using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : Singleton<AudioManager>
{
    [Header("Background Music")]
    AudioSource bgmusic;

    [Header("Sound FX")]
    [SerializeField]
    AudioSource buttonClick;
    public AudioSource 
        errorPurchase;
    public AudioSource successfulPurchase;
    public AudioSource slowDown;
    public AudioSource speedUp;
    public AudioSource placingChef;
    public AudioSource successfulFood;
    public AudioSource unsuccessfulFood;

    [Header("Mixers")]
    public AudioMixer masterMixer;

    public void SetSFXLvl(float _sfxLvl)
    {
        masterMixer.SetFloat("sfxVol", _sfxLvl);
    }
    
    public void SetMusicLvl(float _musicLvl)
    {
        masterMixer.SetFloat("musicVol", _musicLvl);
    }

    public void ButtonClickSound()
    {
        buttonClick.Play();
    }
}
