using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LeaveConfirmationPanel : MonoBehaviour
{
    public GameObject Loading;
    public GameObject leaveConfirmationPanel;


    public void ConfirmLeave()
    {
        Loading.SetActive(true);
        leaveConfirmationPanel.SetActive(false);
        SceneManager.LoadSceneAsync(GameManager.instance.metaverseScene.sceneName);
    }

    public void CancelLeave()
    {
        leaveConfirmationPanel.SetActive(false);
    }
}
