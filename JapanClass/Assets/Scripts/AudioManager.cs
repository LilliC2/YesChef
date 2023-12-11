using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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


    public void ButtonClickSound()
    {
        buttonClick.Play();
    }
}
