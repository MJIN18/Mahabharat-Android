using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterPnlController : MonoBehaviour
{
    
    public void OnClickNextBtn()
    {
        CharacterManager.Instance.NextOption();
    }

    public void OnClickBackBtn()
    {
        CharacterManager.Instance.BackOption();
    }

    public void OnClickSelect()
    {
        SceneManager.LoadSceneAsync(0);
    }
}
