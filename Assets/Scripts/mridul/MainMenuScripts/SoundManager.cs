using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    public Sound[] musicSounds, sfxSounds;
    public AudioSource musicSource, sfxSource;
    #region Awake Start 
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        PlayMusic("Theme");
    }
    #endregion

    #region Music Setting Functions

    public void PlayMusic(String name)
    {
        Sound s = Array.Find(musicSounds, x=> x.name == name);

        if(s != null)
        {
            musicSource.clip = s.clip;
            musicSource.Play();
        }
        else
        {
            Debug.Log("Sound not found");
        }
    }

    public void PlaySFX(String name)
    {
        Sound s = Array.Find(sfxSounds, x => x.name == name);

        if (s != null)
        {
            sfxSource.PlayOneShot(s.clip);
        }
        else
        {
            Debug.Log("Sound not found");
        }
    }

    public void ToggleMusic(bool on)
    {
        musicSource.mute = on;

        UIController.instance.musicStatus = !on;

        if (on)
        {
            PlayerPrefs.SetInt(UIController.instance.musicKey, 0);
        }
        else
        {
            PlayerPrefs.SetInt(UIController.instance.musicKey, 1);
        }
    }

    public void ToggleSFX(bool on)
    {
        sfxSource.mute = on;

        UIController.instance.sfxStatus = !on;

        if (on)
        {
            PlayerPrefs.SetInt(UIController.instance.sfxKey, 0);
        }
        else
        {
            PlayerPrefs.SetInt(UIController.instance.sfxKey, 1);
        }
    }

    public void MusicVolume(float volume)
    {
        musicSource.volume = volume;
        PlayerPrefs.SetFloat(UIController.instance.musicVolumeKey, UIController.instance._musicSlider.value);
    }
    
    public void SFXVolume(float volume)
    {
        sfxSource.volume = volume;
        PlayerPrefs.SetFloat(UIController.instance.sfxVolumeKey, UIController.instance._SfxSlider.value);
    }

    #endregion
}

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;

}