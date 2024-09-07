using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public int sceneIndex;

    public bool isLeaveTrigger;

    public GameObject Loading;
    public LeaveConfirmationPanel leaveConfirmationPanel;
    private void OnTriggerEnter(Collider other)
    {
        //if(other.gameObject.tag == "Player")
        //{
        //    if (isLeaveTrigger)
        //    {
        //        leaveConfirmationPanel.leaveConfirmationPanel.SetActive(true);
        //    }
        //    else
        //    {
        //        Loading.SetActive(true);
        //        SceneManager.LoadSceneAsync(GameManager.instance.gameplayScene.sceneName);
        //        Debug.Log("ChangingScene " + GameManager.instance.gameplayScene.sceneName);
        //    }
        //}
        if (other.gameObject.tag == "Cart")
        {
            Loading.SetActive(true);
            SceneManager.LoadSceneAsync(GameManager.instance.gameplayScene.sceneName);
            Debug.Log("ChangingScene " + GameManager.instance.gameplayScene.sceneName);
        }
    }

    //public void B()
    //{
    //    Loading.SetActive(true);
    //    SceneManager.LoadSceneAsync(GameManager.instance.gameplayScene.sceneName);
    //}

}
