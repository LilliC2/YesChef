using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : Singleton<AudioManager>
{
    [Header("Background Music")]
    AudioSource bgmusic;

    [Header("Sound FX")]
    [SerializeField]
    AudioClip[] uiAudioClips;
    [SerializeField]
    AudioSource uiAudioSourceButtonClick, uiAudioSource;

    public enum UIAudioClips { ButtonClick, ErrorPurchase, SuccessfulPurchase };



    [SerializeField]
    AudioClip[] chefWorkingAudioClips;

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
        uiAudioSourceButtonClick.Play();
    }

    public void PlayUIAudio(UIAudioClips _clip)
    {
        //Find corresponding clip in list
        AudioClip _targetClip = uiAudioClips.Where(clip => clip.name == _clip.ToString()).SingleOrDefault();

        uiAudioSource.clip = _targetClip;
        uiAudioSource.Play();

    }

    public AudioClip ReturnChefWorkingAudioClip(ChefData.WorkingOnSkill _skill)
    {
        switch(_skill)
        {
            case ChefData.WorkingOnSkill.Cooking:
                return chefWorkingAudioClips[0];
            case ChefData.WorkingOnSkill.Kneading:
                return chefWorkingAudioClips[1];
            case ChefData.WorkingOnSkill.Cutting:
                return chefWorkingAudioClips[2];
            case ChefData.WorkingOnSkill.Mixing:
                return chefWorkingAudioClips[3];
        }

        return null;
    }
}
