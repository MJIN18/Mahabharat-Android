using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController instance;

    public string musicKey = "MusicStatus";
    public string musicVolumeKey = "MusicVolume";
    public string sfxKey = "SfxStatus";
    public string sfxVolumeKey = "SfxVolume";

    public bool musicStatus;
    public bool sfxStatus;

    public Toggle musicToggle;
    public Toggle sfxToggle;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        GameManager.instance.ShowDubug(PlayerPrefs.GetInt(musicKey));
        GameManager.instance.ShowDubug(PlayerPrefs.GetInt(sfxKey));

        UpdateAudioStatus();
    }

    private void UpdateAudioStatus()
    {
        int musicValue = 0;
        int sfxValue = 0;

        if (PlayerPrefs.HasKey(musicKey)) { musicValue = PlayerPrefs.GetInt(musicKey); }
        if(PlayerPrefs.HasKey(musicVolumeKey)) { _musicSlider.value = PlayerPrefs.GetFloat(musicVolumeKey); }
        if (PlayerPrefs.HasKey(sfxKey)) { sfxValue = PlayerPrefs.GetInt(sfxKey); }
        if (PlayerPrefs.HasKey(sfxVolumeKey)) { _SfxSlider.value = PlayerPrefs.GetFloat(sfxVolumeKey); }

        musicStatus = (musicValue == 1);
        sfxStatus = (sfxValue == 1);

        SoundManager soundManager = SoundManager.Instance;

        soundManager.musicSource.mute = !musicStatus;
        soundManager.sfxSource.mute = !sfxStatus;

        musicToggle.isOn = musicStatus;
        sfxToggle.isOn = sfxStatus;
    }

    public Slider _musicSlider, _SfxSlider;

    public void ToggleMusicc(bool on)
    {
        SoundManager.Instance.ToggleMusic(!on);
    }

    public void ToggleSFXx(bool on)
    {
        SoundManager.Instance.ToggleSFX(!on);
    }

    public void MusicVolume()
    {
        SoundManager.Instance.MusicVolume(_musicSlider.value);
    }

    public void SFXVolume()
    {
        SoundManager.Instance.SFXVolume(_SfxSlider.value);
    }

    public void ButtonSound()
    {
        SoundManager.Instance.PlaySFX("Button");
        //UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = new Color32(56, 4, 4, 255);
        //StartCoroutine(ChangeColorToDefault());
    }

    IEnumerator ChangeColorToDefault()
    {
        yield return new WaitForSeconds(1f);
        UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = new Color32(255, 255, 255, 255);
    }
}
