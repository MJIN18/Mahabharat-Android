using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Runtime.InteropServices;
using System;
using System.Threading.Tasks;

public class MainMenuManager : MonoBehaviour
{
    #region Singleton
    public static MainMenuManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    #endregion

    #region Variables

    public Transform playerDisplayPos;
    public Transform customizationDisplayPoint;

    [Header("Next Scene")]
    public int sceneindex;

    [Header("UI Items")]
    public Button selectButton;
    public Image iconImage;
    public TextMeshProUGUI coinText;

    [Header("Private Properties")]
    CharacterDatabase characterDB;

    public int SelectedCharacter;
    public CharacterSkin skin;

    [Header("Menu Windows")]

    [Header("Customization Window")]
    public GameObject customizationWindow;
    public GameObject customizationCam;

    [Header("MainMenu Window")]
    public GameObject mainMenuWindow;
    public GameObject mainMenuCam;

    [Header("Settings Window")]
    public GameObject settingsWindow;

    [Header("Stores Window")]
    public GameObject storesWindow;

    [Header("Game Loading Window")]
    public GameObject gameLoadingWindow;
    public GameObject connectionLoader;

    [Header("Leaderboard Window")]
    public GameObject leaderboardWindow;

    [Header("Withdrawl Window")]
    public GameObject withdrawWindow;
    public GameObject historyTab;
    public Transform historyListParent;
    public GameObject historyHubPrefab;
    public TMP_InputField walletAdressField;
    public TMP_InputField amount;
    public Button withdrawButton;
    public GameObject withdrawWarning;
    public GameObject noHistory;

    [Header("Other")]
    public GameObject currencyHUD;

    [Header("HomePageURL")]
    public string homeURL;

#if UNITY_WEBGL

    [DllImport("__Internal")]
    private static extern void GoToHomePage();

#endif

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        characterDB = GameManager.instance.characterDatabase;

        //UseSkin(GameManager.instance.DisplaySkin());

        if(coinText != null)
            coinText.text = GameManager.instance.score.ToString();

        storesWindow = MenuManager.instance.storeWindow;

        //if(playerDisplayPos.childCount > 0)
        //{
        //    for (int i = 0; i < playerDisplayPos.childCount; i++)
        //    {
        //        Destroy(playerDisplayPos.GetChild(i).gameObject);
        //    }
        //}

        //Instantiate(GameManager.instance.DisplaySkin(), playerDisplayPos);
    }

    public void ConnectivityLoader(bool show)
    {
        connectionLoader.SetActive(show);
    }


#region Customization

    public void OpenCustomizationWindow()
    {
        //UpdateCharacter(SelectedCharacter);
        //customizationCam.SetActive(true);
        //mainMenuCam.SetActive(false);

        customizationWindow.SetActive(true);
        //mainMenuWindow.SetActive(false);
    }

    public void CloseCustomizationWindow()
    {
        //mainMenuCam.SetActive(true);
        //customizationCam.SetActive(false);

        //mainMenuWindow.SetActive(true);
        customizationWindow.SetActive(false);
    }

    public void NextSkin()
    {
        SelectedCharacter++;

        if (SelectedCharacter >= characterDB.CharacterCount)
        {
            SelectedCharacter = 0;
        }

        //UpdateCharacter(SelectedCharacter);
    }

    public void PrevSkin()
    {
        SelectedCharacter--;

        if (SelectedCharacter < 0)
        {
            SelectedCharacter = characterDB.CharacterCount - 1;
        }

        //UpdateCharacter(SelectedCharacter);
    }

    public void SelectSkin()
    {
        GameManager.instance.currentSkin = skin;
        UseSkin(GameManager.instance.currentSkin.skin);
    }

    public void UseSkin(GameObject newSkin)
    {
        if (playerDisplayPos.childCount > 0)
        {
            for (int i = 0; i < playerDisplayPos.childCount; i++)
            {
                Destroy(playerDisplayPos.GetChild(i).gameObject);
            }
        }

        Instantiate(newSkin, playerDisplayPos);
    }

    //public void UpdateCharacter(int selectedCharacter)
    //{
    //    Character character = characterDB.GetCharacter(selectedCharacter); // access characters index value from character database(Scriptableobject)
    //    selectButton.interactable = character.unlocked;
    //    iconImage.sprite = character.icon;
    //    skin = character.skin;
    //    GameObject slectedSkin = skin.skin;


    //    //Debug.Log("Show  ======"+ CharacterPOSInCharacterPnl.GetChild(0).gameObject);

    //    if (customizationDisplayPoint.childCount > 0) // if there is a character GAmeObject present, destroy it (so new gameobject can be shown there) 
    //    {
    //        for (int i = 0; i < customizationDisplayPoint.childCount; i++)
    //        {
    //            Destroy(customizationDisplayPoint.GetChild(i).gameObject);

    //        }
    //    }

    //    GameObject ShowingSkin = Instantiate(slectedSkin, customizationDisplayPoint);
    //}

#endregion


#region MenuFunctions

    public async void WithdrawlWindow(bool show)
    {
        withdrawWindow.SetActive(show);

        if(show)
        {
            await RefreshWithdrawWindow();
        }
    }

    private async Task RefreshWithdrawWindow()
    {
        GameManager gameManager = GameManager.instance;

        ConnectivityLoader(true);

        Limit limit = await gameManager.GetWithdrawalLimit();
        var data = await gameManager.GetTokensAndCoins(gameManager.username);

        if (limit != null && data.token > limit.limit)
        {
            withdrawButton.interactable = true;
            withdrawWarning.SetActive(false);
        }
        else
        {
            withdrawButton.interactable = false;
            withdrawWarning.GetComponent<TextMeshProUGUI>().text = "Not enough Tokens. Min Required: " + limit.limit;
            withdrawWarning.SetActive(true);
        }

        ConnectivityLoader(false);
    }

    public void HistoryTab(bool show)
    {
        historyTab.SetActive(show);
    }

    public async void RefreshWithdrawHistory()
    {
        ConnectivityLoader(true);

        if(historyListParent.childCount > 0)
        {
            for(int i = 0;i < historyListParent.childCount; i++)
            {
                Destroy(historyListParent.GetChild(i).gameObject);
            }
        }

        GameManager gm = GameManager.instance;

        var History = await gm.RequestHistory(gm.username, 1, 10);

        if (History != null)
        {
            foreach (History history in History.history)
            {
                WithdrawHistoryHud withdrawHistoryHud = Instantiate(historyHubPrefab, historyListParent).GetComponent<WithdrawHistoryHud>();

                string dateString = history.createdAt;
                string dateOnly = "yyyy-MM-dd";

                if (DateTime.TryParseExact(dateString, "yyyy-MM-dd'T'HH:mm:ss.fff'Z'", null, System.Globalization.DateTimeStyles.None, out DateTime dateTime))
                {
                    // Extract the date part
                    dateOnly = dateTime.ToString("yyyy-MM-dd");

                    // Debug only the date
                    Debug.Log("Date only: " + dateOnly);
                }
                else
                {
                    Debug.LogError("Failed to parse the date string.");
                }

                withdrawHistoryHud._date.text = dateOnly;
                withdrawHistoryHud._amount.text = history.amount.ToString();
                withdrawHistoryHud._status.text = history.status;
            }
        }

        ConnectivityLoader(false);
    }

    public async void SendWithdrawRequest()
    {
        GameManager gm = GameManager.instance;

        ConnectivityLoader(true);

        await gm.RequestWithdraw(gm.username, walletAdressField.text, amount.text);
        await RefreshWithdrawWindow();

        ConnectivityLoader(false);
    }

    public void EnterHome()
    {
        UIController.instance.ButtonSound();

        mainMenuWindow.SetActive(true);
        currencyHUD.SetActive(true);
        LoadLevel(GameManager.instance.menuScene.sceneName);
    }

    public void EnterMetaverse()
    {
        UIController.instance.ButtonSound();

        currencyHUD.SetActive(false);
        mainMenuWindow.SetActive(false);
        LoadLevel(GameManager.instance.gameplayScene.sceneName);
    }

    public virtual void LoadLevel(string levelName)
    {
        SceneManager.LoadSceneAsync(levelName);
    }

    public void LoadLevel(int levelIndex)
    {
        SceneManager.LoadSceneAsync(levelIndex);
    }

    public void OpenStoreWindow()
    {
        UIController.instance.ButtonSound();

        storesWindow.SetActive(true);
    }

    public void CloseStoreWindow()
    {
        UIController.instance.ButtonSound();

        storesWindow.SetActive(false);
    }

    public void OpenSettingsMenu()
    {
        UIController.instance.ButtonSound();

        currencyHUD.SetActive(false);
        settingsWindow.SetActive(true);
        mainMenuWindow.SetActive(false);
    }

    public void CloseSettingsMenu()
    {
        UIController.instance.ButtonSound();

        currencyHUD.SetActive(true);
        mainMenuWindow.SetActive(true);
        settingsWindow.SetActive(false);
    }

    public void OpenLeaderboard()
    {
        UIController.instance.ButtonSound();

        LeaderBoardContentView.instance.RefreshLeaderboards();
        leaderboardWindow.SetActive(true);
    }

    public void CloseLeaderboard()
    {
        UIController.instance.ButtonSound();

        leaderboardWindow.SetActive(false);
    }

    public void QuitButton()
    {
        Application.Quit();
    }    
    public void Quit()
    {
        UIController.instance.ButtonSound();

#if UNITY_ANDROID
        // Code for Android
       // SceneManager.LoadSceneAsync(0);
        //  LoadLevel(GameManager.instance.welcomeScene.sceneName);
        // Code for Android
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            Application.Quit();
        }
        else
        {
            LoadLevel(GameManager.instance.welcomeScene.sceneName);
        }
#elif UNITY_IOS
        // Code for iOS
        LoadLevel(GameManager.instance.metaverseScene.sceneName);
        //SceneManager.LoadSceneAsync(0);
#elif UNITY_WEBGL
        // Code for WebGL
        GoToHomePage();
#elif PLATFORM_STANDALONE_WIN
        //code for windows
        Application.Quit();
#endif
        //Application.Unload();
    }

#endregion
}
