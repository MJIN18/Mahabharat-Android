using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuScreenBtnController : MonoBehaviour
{
    public GameObject MainMenuPnl;
    public GameObject StorePnl;
    public GameObject SettingPnl;

    private void Start()
    {
        Screen.orientation = ScreenOrientation.Portrait;
        
        MainMenuPnl.SetActive(true);
        StorePnl.SetActive(false);
        SettingPnl.SetActive(false);
    }
    public void PlayGame()
    {
        MainMenuManager.instance.gameLoadingWindow.SetActive(true);
        StartCoroutine(StartGame());
    }

    IEnumerator StartGame()
    {
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadSceneAsync(3);
    }

    public void OnClickCharacterButton()
    {
        SceneManager.LoadSceneAsync(2);
    }

}
