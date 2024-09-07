using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{

    #region Singleton
    public static LevelManager instance;

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
    #endregion

    public float speedUpValue;

    public GameObject difficultyPopup;
    public Animator difficultyAnim;

    public PlayerControllerNew player;

    public Coroutine SpeedUP;

    public Stage stage;

    public void Start()
    {
        SpeedUP = StartCoroutine(SpeedIncrease());
    }

    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.P))
    //    {
    //        SlowDownGame();
    //    }
    //    if (Input.GetKeyDown(KeyCode.L))
    //    {
    //        SpeedUpGame();
    //    }
    //}


    public IEnumerator SpeedIncrease()
    {
        yield return new WaitForSeconds(90f);

        if(stage != Stage.STAGE3)
        {
            SpeedUpGame();
            if (difficultyPopup != null)
            {
                difficultyPopup.SetActive(true);
                difficultyAnim.SetTrigger("Pop");
                StartCoroutine(DisablePopUP());
            }
            SpeedUP = StartCoroutine(SpeedIncrease());
        }
    }

    public IEnumerator DisablePopUP()
    {
        yield return new WaitForSeconds(1f);

        if(difficultyPopup != null && difficultyPopup.activeInHierarchy)
        {
            difficultyPopup.SetActive(false);
        }
    }

    public void SpeedUpGame()
    {
        Time.timeScale += speedUpValue;
        stage = stage + 1;

        Debug.Log("Speed Increased");
    }

    public void SlowDownGame()
    {
        Time.timeScale -= speedUpValue;

        Debug.Log("Speed Decreased");
    }

    public void ResetSpeed()
    {
        if(SpeedUP != null)
        {
            StopCoroutine(SpeedIncrease());
            SpeedUP = null;
        }
        stage = Stage.STAGE1;
        Time.timeScale = 1f;
    }

    public void OnRevive()
    {
        player.Revive();
    }
}
