using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameMenu : MonoBehaviour
{
    public Toggle musicButton;

    private void Start()
    {
        musicButton.isOn = UIController.instance.musicStatus;
    }

    public void GoToHome()
    {
        MainMenuManager.instance.EnterHome();
    }

    public void MusicOnOff(bool off)
    {
        SoundManager.Instance.ToggleMusic(!off);
        UIController.instance.musicToggle.isOn = off;
    }

    public void OpenSettings(GameObject panel)
    {
        panel.SetActive(true);
    }

    public void CloseSetting(GameObject panel)
    {
        panel.SetActive(false);
    }
}
