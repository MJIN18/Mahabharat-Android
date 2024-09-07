using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WelcomePageMobile : MonoBehaviour
{
    public GameObject loader;

    private void Start()
    {
        Screen.orientation = ScreenOrientation.Portrait;
    }
    public void ExploreMetaverse()
    {
        SceneManager.LoadSceneAsync(1);
        loader.SetActive(true);
    }
}
