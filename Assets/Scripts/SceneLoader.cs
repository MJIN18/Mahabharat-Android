using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public string sceneToLoad;
    public GameObject Loading;
    
    // Start is called before the first frame update
    void Start()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        gameObject.GetComponent<GameObject>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            // Load the specified scene when the trigger is entered
            Loading.SetActive(true);


            ThirdPersonController tps = other.GetComponent<ThirdPersonController>();
            if (tps != null)
            {
                if (tps.touchField.isCursorLocked)
                {
                    tps.touchField.starterInput.SetCursorState(false);
                }
            }

            SceneManager.LoadSceneAsync(GameManager.instance.warZoneScene.sceneName);
            GameManager.instance.ShowDubug("Player Entered");
        }
    }

    public void A()
    {
        Loading.SetActive(true);
        SceneManager.LoadSceneAsync(GameManager.instance.warZoneScene.sceneName);
    }

}
