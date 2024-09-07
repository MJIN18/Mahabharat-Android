using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using Unity.Services.Samples.VirtualShop;
using System.Threading.Tasks;
using Unity.Services.Economy;
using System;

public class UIScript : Singleton<UIScript>
{
    [SerializeField] public GameObject gameOverPanel;

    //Particle Effects
    public GameObject pickUpEffect;
    public GameObject smallExplosion;
    public GameObject shieldBreak;
    public GameObject coinCollector;
    public GameObject buff;
    public GameObject buffMagnet;
    //   public ParticleSystem p;

    public GameObject[] stars;

    public GameObject fistButton;

    public AudioSource coinAudioSource;
    public AudioClip[] coinAudioClip;

 //   public AudioClip warClip;

    public TextMeshProUGUI coins;
    public TextMeshProUGUI score;

    public TextMeshProUGUI scoreGOP;
    int newScore = 0;
    int n;

    [Header("Collectibles")]
    public Currency[] collectedCurrency;
    public int gainedScore;

    public TextMeshProUGUI fistText;
    public int f = 0;

    public int powers;

    [Header("Pause Menu")]
    public float previousTimeScale;
    public bool isPaused = false;
    bool scoreUpdate = false;

    public GameObject pauseScreen;

    [Header("Revivals")]
    public GameObject revivePanel;
    public GameObject revivePanelWeb;
    public GameObject _warningPop;

    public TextMeshProUGUI reviveCostText;
    public TextMeshProUGUI reviveCostTextWeb;

    public string[] revivePurchaseIDs;
    public string[] revivalCosts;

    public int currentPurchaseIndex = 0;
    private int k_EconomyPurchaseCostsNotMetStatusCode;

    public GameObject[] playerPosDots;

    GameObject currentPosDot;

    private void Start()
    {
        Screen.orientation = ScreenOrientation.Portrait;

        scoreUpdate = false;
        StartCoroutine(StartScoreUpdate());

   //     GameObject.Find("MusicSource").GetComponent<AudioSource>().clip = warClip;
    }

    IEnumerator StartScoreUpdate()
    {
        yield return new WaitForSeconds(4.5f);

        scoreUpdate = true;
    }

    private void Update()
    {

        if (PlayerControllerNew.instance.startTouch == true && Time.timeScale != 0 && scoreUpdate == true)
        {
        
            newScore = newScore + 1;

            score.text = newScore.ToString();
        }
    }

    public void RestartGame()
    {
        LevelManager.instance.ResetSpeed();
        GameManager.instance.UpdateScores(collectedCurrency, newScore);
        MainMenuManager.instance.LoadLevel(GameManager.instance.gameplayScene.sceneName);
    }

    public void GameOver(int g)
    {
        LevelManager.instance.ResetSpeed();
        if (g == 2)
        {
            gameOverPanel.SetActive(true);
            scoreGOP.text = score.text;
            PlayerControllerNew.instance.startTouch = false;
            StopCoroutine(LevelManager.instance.SpeedIncrease());
            setStars();
        }
        else
        {
            GameManager.instance.UpdateScores(collectedCurrency, newScore);
            MainMenuManager.instance.EnterHome();
        }
    }

    public void setStars()
    {
        Debug.Log("value of s=========" + newScore);
        Debug.Log("value of n=========" + n);
        if (1000 >= newScore)
        {
            n = 2;
        }
        else if (5000 >= newScore)
        {
            n = 3;
        }
        else if (10000 >= newScore)
        {
            n = 4;
        }
        else if (25000 >= newScore)
        {
            n = 5;
        }

        for (int i = 0; i < n; i++)
        {
            stars[i].SetActive(true);
        }
    }

    public void StopParticleEffect(int i)
    {
        StartCoroutine(stopParticleEffect(i));
    }


    IEnumerator stopParticleEffect(int i)
    {
        Debug.Log("stop Particle");

        if (i == 2)
        {
            yield return new WaitForSeconds(1f);
            coinCollector.SetActive(false);
        }
        else
        {

            yield return new WaitForSeconds(1.5f);

            if (i == 0)
            {
                pickUpEffect.SetActive(false);
            }
            else if (i == 1)
            {
                smallExplosion.SetActive(false);
            }
            //      pickUpEffect.GetComponent<ParticleSystem>().Stop();
        }
    }

    public void LeaveGame()
    {
        MainMenuManager.instance.EnterHome();
    }

    public void PauseGame()
    {
        RefreshMapPos();
        isPaused = true;
        previousTimeScale = Time.timeScale;
        Time.timeScale = 0;
        PlayerControllerNew.instance.startTouch = false;
        pauseScreen.SetActive(true);
    }

    public void RefreshMapPos()
    {
        int maxNumber = playerPosDots.Length;

        int currentPos = UnityEngine.Random.Range(0, maxNumber);

        currentPosDot = playerPosDots[currentPos];
        currentPosDot.SetActive(true);
    }

    void TriggerPauseAnimation(GameObject pointer)
    {
        // Trigger the specified animation state in the Animator
        Animator animator = pointer.GetComponent<Animator>();

        if (animator != null)
        {
            animator.Play("PointerAnim");
        }
        else
        {
            Debug.LogWarning("Animator component not found on the UI element.");
        }
    }

    public void RevivePanel(bool show)
    {
#if UNITY_WEBGL
        revivePanelWeb.SetActive(show);
        reviveCostTextWeb.text = revivalCosts[currentPurchaseIndex];
#else
        revivePanel.SetActive(show);
        reviveCostText.text = revivalCosts[currentPurchaseIndex];
#endif
    }

    public void ContinueGame()
    {
        isPaused = false;
        currentPosDot.SetActive(false);
        pauseScreen.SetActive(false);
        PlayerControllerNew.instance.startTouch = true;
        Time.timeScale = previousTimeScale;
    }

    public void fistPowerMethod()
    {
        f = f + 1;
        fistText.text = f.ToString();
    }

    public void ExecutePunch()
    {
        //    Debug.Log("Executing the punch");
        if (f >= 1)
        {
            PlayerControllerNew.instance.powerUpController.Punch.SetActive(true);
            PlayerControllerNew.instance.GetComponent<Animator>().SetTrigger("Punch");

            f = f - 1;
            fistText.text = f.ToString();

            /*     if (PlayerController.instance.Punch.activeInHierarchy)
                 {
                     PlayerController.instance.Punch.SetActive(false);
                 }*/

            StartCoroutine(StopPunch());
        }
    }

    public void ShowWarningPop(bool show)
    {
        _warningPop.SetActive(show);
    }

    IEnumerator StopPunch()
    {
        yield return new WaitForSeconds(0.5f);
        PlayerControllerNew.instance.powerUpController.Punch.SetActive(false);
    }

    public async void ReviveWithToken()
    {
        try
        {
            await VirtualShopSceneManager.Instance.OnReviveWithTokenClicked(revivePurchaseIDs[currentPurchaseIndex]);
        }
        catch (Exception e)
        {
            //Debug.LogException(e);
        }
    }



    //public async Task OnReviveWithTokenClicked(string purchaseID)
    //{
    //    try
    //    {
    //        var result = await EconomyManager.instance.MakeVirtualPurchaseAsync(purchaseID);
    //        if (this == null) return;

    //        await EconomyManager.instance.RefreshCurrencyBalances();
    //        if (this == null) return;

    //        //ShowRewardPopup(result.Rewards, virtualShopItem, button);
    //    }
    //    catch (EconomyException e)
    //        when (e.ErrorCode == k_EconomyPurchaseCostsNotMetStatusCode)
    //    {
    //        //virtualShopSampleView.ShowVirtualPurchaseFailedErrorPopup();
    //        ShowWarningPop(true);
    //    }
    //    catch (Exception e)
    //    {
    //        Debug.LogException(e);
    //    }
    //}
}
