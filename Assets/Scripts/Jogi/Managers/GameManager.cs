using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Services.Economy;
using Unity.Services.Core;
using Unity.Services.CloudSave;
//using UnityEditor.Animations;
using UnityEngine;
using Unity.Services.Samples.VirtualShop;
using System.Threading.Tasks;
using UnityEngine.Networking;

public class GameManager : MonoBehaviour
{

    #region Singleton
    public static GameManager instance;

    private void Awake()
    {
        if(instance == null)
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

    #region Public Properties

    public string username;

    public bool showDebugs;

    public CharacterDatabase characterDatabase;

    public CharacterSkin currentSkin = null;
    public CharacterSkin defaultSkin;
    public RuntimeAnimatorController playerController;
    public ControlMode controlMode;

    public int score;
    public int highscore;

    [Header("Currencies")]
    public Currency[] myCurrencies;

    [Header("Player Pref Keys")]
    public string coinKey;
    public string skinIdKey;

    [Header("SceneIndexes")]
    public SceneIndexes welcomeScene;
    public SceneIndexes menuScene;
    public SceneIndexes metaverseScene;
    public SceneIndexes warZoneScene;
    public SceneIndexes gameplayScene;

    [Header("Skin Preview")]
    public Transform previewPoint;
    public string selectedArmor;
    public GameObject defaultArmor;

    [Header("Character Database")]
    public Avatar animationAvatar;
    public Character[] armors;

    public Transform previewBody;
    public Transform metaversePlayerBody;
    public Transform gamePlayBody;

    #endregion


    public GameObject prevbody;
    public GameObject GainTokenButton;

    public SwipeRotation _SR;


    private void Start()
    {

#if UNITY_EDITOR

        GainTokenButton.SetActive(true);

#endif

        //if (PlayerPrefs.HasKey(coinKey))
        //{
        //    coins = PlayerPrefs.GetInt(coinKey);
        //}

        if (Application.platform == RuntimePlatform.Android && controlMode != ControlMode.TOUCH)
        {
            controlMode = ControlMode.TOUCH;
        }
        else if(Application.platform == RuntimePlatform.IPhonePlayer && controlMode != ControlMode.TOUCH)
        {
            controlMode = ControlMode.TOUCH;
        }
    }

    #region Custom Log In

    public Task<bool> CheckUserExistsAsync(string id)
    {
        string url = "https://user-apis.zip2box.com/api/user/checkexistence?userId=" + id;

        TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();

        UnityWebRequest www = UnityWebRequest.Get(url);
        var operation = www.SendWebRequest();

        operation.completed += asyncOperation =>
        {
            if (www.result == UnityWebRequest.Result.Success)
            {
                string jsonResponse = www.downloadHandler.text;
                APIResponse responseData = JsonUtility.FromJson<APIResponse>(jsonResponse);

                // Check if user exists based on the data in the response
                bool userExists = responseData != null && responseData.data != null && responseData.data.exists;

                tcs.SetResult(userExists);
            }
            else
            {
                Debug.LogError("Error fetching data from API: " + www.error);
                tcs.SetResult(false); // Set to false if there's an error
            }
        };

        return tcs.Task;
    }

    public async Task SendSignUpRequestForm(string username)
    {
        string url = "https://user-apis.zip2box.com/api/user/login";

        //Create a form and add parameters
        WWWForm form = new WWWForm();
        form.AddField("userId", username);

        //create a task completion source
        var tcs = new TaskCompletionSource<object>();

        //send the request
        var request = UnityWebRequest.Post(url, form);
        var operation = request.SendWebRequest();

        operation.completed += (asyncOperation) =>
        {
            if (request.result == UnityWebRequest.Result.Success)
            {
                tcs.SetResult(null);
            }
            else
            {
                tcs.SetException(new System.Exception(request.error));
            }
        };

        //Wait for the task complete
        await tcs.Task;
    }

    public async Task UpdateTokensAndCoins(string username, int coins, int tokens)
    {
        string url = "https://user-apis.zip2box.com/api/user/newTokenAndCoins";

        //Create a form and add parameters
        WWWForm form = new WWWForm();
        form.AddField("coins", coins);
        form.AddField("tokens", tokens);
        form.AddField("userId", username);

        //create a task completion source
        var tcs = new TaskCompletionSource<object>();

        //send the request
        var request = UnityWebRequest.Post(url, form);
        var operation = request.SendWebRequest();

        operation.completed += (asyncOperation) =>
        {
            if (request.result == UnityWebRequest.Result.Success)
            {
                tcs.SetResult(null);
            }
            else
            {
                tcs.SetException(new System.Exception(request.error));
            }
        };

        //Wait for the task complete
        await tcs.Task;
    }

    public async Task<UserData> GetTokensAndCoins(string username)
    {
        string url = "https://user-apis.zip2box.com/api/user/myProfile?userId=" + username;

        // Create a task completion source
        var tcs = new TaskCompletionSource<UserData>();

        var request = UnityWebRequest.Get(url);
        var operation = request.SendWebRequest();

        operation.completed += (asyncOperation) =>
        {
            if (request.result == UnityWebRequest.Result.Success)
            {
                // Get the data from the request
                string responseData = request.downloadHandler.text;

                UserDataWrapper userDataWrapper = JsonUtility.FromJson<UserDataWrapper>(responseData);

                UserData userProfile = userDataWrapper.data;

                Debug.Log(responseData + " Recieved userData : " + userProfile.userId + " " + userProfile.coin + " " + userProfile.token);

                // Set the result with parsed data
                tcs.SetResult(userProfile);
            }
            else
            {
                // Set exception if request fails
                tcs.SetException(new System.Exception(request.error));
            }
        };

        // Return the data
        return await tcs.Task;
    }

    public async Task<Limit> GetWithdrawalLimit()
    {
        string url = "https://user-apis.zip2box.com/api/user/withdrawlimit";

        var tcs = new TaskCompletionSource<Limit>();

        var request = UnityWebRequest.Get(url);
        var operation = request.SendWebRequest();

        operation.completed += (asyncOperation) =>
        {
            if (request.result == UnityWebRequest.Result.Success)
            {
                string responseData = request.downloadHandler.text;

                WithdrawalLimitWrapper withdrawalLimitWrapper = JsonUtility.FromJson<WithdrawalLimitWrapper>(responseData);

                Limit limit = withdrawalLimitWrapper.data;

                Debug.Log(responseData + " limit: " + limit);

                tcs.SetResult(limit);
            }
            else
            {
                tcs.SetException(new System.Exception(request.error));
            }
        };

        return await tcs.Task;
    }

    public async Task RequestWithdraw(string username, string walletAddress, string tokenAmount)
    {
        string url = "https://user-apis.zip2box.com/api/user/withdraw?userId=" + username + "&wallet_address=" + walletAddress + "&amount=" + tokenAmount;


        // Create UnityWebRequest
        UnityWebRequest request = UnityWebRequest.PostWwwForm(url, "");

        // Send the request asynchronously
        var operation = request.SendWebRequest();

        // Wait until the operation is completed
        while (!operation.isDone)
        {
            await Task.Yield();
        }

        // Check for errors
        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Withdrawal request successful!");
        }
        else
        {
            Debug.LogError("Withdrawal request failed: " + request.error);
            throw new Exception("Withdrawal request failed: " + request.error);
        }
    }

    public async Task<HistoryData> RequestHistory(string username, int pageNo, int pageLimit)
    {
        string url = "https://user-apis.zip2box.com/api/user/myhistory?userId=" + username + "&pageNumber=" + pageNo + "&pageLimit=" + pageLimit;

        var tcs = new TaskCompletionSource<HistoryData>();

        var request = UnityWebRequest.Get(url);
        var operation = request.SendWebRequest();

        operation.completed += (asyncOperation) =>
        {
            if (request.result == UnityWebRequest.Result.Success)
            {
                string responseData = request.downloadHandler.text;

                WithdrawHistoryWrapper withdrawHistoryWrapper = JsonUtility.FromJson<WithdrawHistoryWrapper>(responseData);

                HistoryData historyData = withdrawHistoryWrapper.data;

                Debug.Log(responseData + " limit: " + historyData.history);

                tcs.SetResult(historyData);
            }
            else
            {
                tcs.SetException(new System.Exception(request.error));
            }
        };

        return await tcs.Task;
    }

    #endregion

    public GameObject DisplaySkin()
    {
        if (PlayerPrefs.HasKey("PlayerName"))
        {
            int id = PlayerPrefs.GetInt("PlayerName");
            //currentSkin = characterDatabase.GetCharacter(id).skin;
        }
        else
        {
            currentSkin = defaultSkin;
        }

        GameObject skin = currentSkin.skin;

        return skin;
    }

    internal void ShowOffer()
    {
        Debug.Log("Not Enough Coins.. Earn or Buy some");
    }

    public async Task SetHighscoreAsync()
    {
        int highscore = await LeaderBoardContentView.instance.GetPlayersScore();
        this.highscore = highscore;
    }

    public void UpdateScores(Currency[] currencies, int score)
    {
        AddCoins(currencies[0].amount, currencies[1].amount);

        if(score > highscore)
        {
            highscore = score;
            LeaderboardsManager.instance.AddScore(score);
        }
    }

    public async Task UpdateCurrencies()
    {
        var data = await GetTokensAndCoins(username);

        myCurrencies[0].SetBalance(data.coin);
        myCurrencies[1].SetBalance(data.token);

        foreach (Currency currency in myCurrencies)
        {
            await EconomyService.Instance.PlayerBalances.SetBalanceAsync(currency.id, currency.amount);
        }
    }

    public async void AddCoins(int coins, int tokens)
    {
        await UpdateTokensAndCoins(username, coins, tokens);

        await UpdateCurrencies();

        await EconomyManager.instance.RefreshCurrencyBalances();
    }

    //public void AddCoins(Currency[] collectedCurrencies)
    //{
    //    VirtualShopSceneManager.Instance.SaveEarnedCurrency(collectedCurrencies);
    //    //PlayerPrefs.SetInt(coinKey, coins);
    //}

    #region Debugging

    public void ShowDubug(object keyA)
    {
        if (showDebugs)
        {
            Debug.Log(keyA);
        }
    }

    #endregion

    #region Character Customization

    public void UseArmor(string id, ArmorType type, Transform pos, ref GameObject obj)
    {
        selectedArmor = id;

        if(prevbody != null)
        {
            Destroy(prevbody.gameObject);
            prevbody = null;
        }

        obj = Instantiate(SelectArmorByID(selectedArmor, type), pos.position, pos.transform.rotation);
    }

    public void UseArmorChild(string id, ArmorType type, Transform pos, ref GameObject obj)
    {
        selectedArmor = id;

        //if (pos.childCount > 0)
        //{
        //    foreach (Transform child in pos.GetChild(0))
        //    {
        //        Destroy(child.gameObject);
        //    }
        //}

        if (prevbody != null)
        {
            Destroy(prevbody.gameObject);
            prevbody = null;
        }

        obj = Instantiate(SelectArmorByID(selectedArmor, type), pos);
    }

    public GameObject SelectArmorByID(string id, ArmorType type)
    {
        GameObject _armor = defaultArmor;

        for (int i = 0; i < armors.Length; i++)
        {
            if(armors[i]._ID == id)
            {
                switch (type)
                {
                    case ArmorType.DISPLAY:

                        _armor = armors[i].displayBody;
                        break;
                    case ArmorType.METAVERSE:

                        _armor = armors[i].metaverseBody;
                        break;
                    case ArmorType.GAMEPLAY:

                        _armor = armors[i].gameplayBody;
                        break;
                }
                break;
            }
        }

        return _armor;
    }

    public void ProcessAvatar(GameObject avatar)
    {
        try
        {
            AddArmature(avatar);

            SetUpArmature(avatar);
        }
        catch(Exception ex)
        {
            Debug.LogError("This is " + ex);
        }
    }

    // Bone names
    private const string BONE_HIPS = "root";
    private const string BONE_ARMATURE = "Armature";

    public void AddArmature(GameObject avatar)
    {
        var armature = new GameObject();
        armature.transform.position = avatar.transform.position;
        armature.name = BONE_ARMATURE;

        armature.transform.parent = avatar.transform;

        var hips = avatar.transform.Find(BONE_HIPS);
        hips.parent = armature.transform;
    }

    public void SetUpArmature(GameObject avatar)
    {
        Animator animator = avatar.transform.parent.parent.GetComponent<Animator>(); //Get PlayerArmature Animator
        animator.enabled = false;
        Debug.Log(avatar.transform.parent.parent + animator.avatar.name);
        animator.avatar = animationAvatar; //Assign Animator Avatar
        animator.enabled = true;
    }

    #endregion
}

#region GameplayData

public enum ArmorType
{
    DISPLAY,
    METAVERSE,
    GAMEPLAY
}

[System.Serializable]
public class Currency
{
    public string id;
    public int amount;

    public void SetBalance(int balance)
    {
        amount = balance;
    }
}

[System.Serializable]
public class SceneIndexes
{
    public string sceneName;
    public int sceneIndex;
}

public enum Stage
{
    STAGE1,
    STAGE2,
    STAGE3,
}

public enum CurrencyType
{
    COIN,
    TARALITY_TOKEN
}

[System.Serializable]
public class Armor
{
    public Sprite icon;
    public GameObject armor;
}

[System.Serializable]
public class Character
{
    public string _ID;
    public GameObject displayBody;
    public GameObject metaverseBody;
    public GameObject gameplayBody;
}

#endregion


#region API's Wrappers

[Serializable]
public class APIResponse
{
    public Data data;
}

[Serializable]
public class Data
{
    public bool exists;
}

[Serializable]
public class UserData
{
    public string _id;
    public string userId;
    public int token;
    public int coin;
    public string createdAt;
    public string updatedAt;
}

[System.Serializable]
public class UserDataWrapper
{
    public UserData data;
    public string message;
    public string error;
    public int status;
}



[System.Serializable]
public class WithdrawalLimitWrapper
{
    public Limit data;
}

[Serializable]
public class Limit
{
    public float limit;
}

[System.Serializable]
public class WithdrawHistoryWrapper
{
    public HistoryData data;
    public string message;
    public string error;
    public int status;
}

[System.Serializable]
public class HistoryData
{
    public History[] history;
    public int total;
}

[System.Serializable]
public class History
{
    public string _id;
    public string userId;
    public int amount;
    public string walletaddress;
    public string status;
    public string createdAt;
    public string updatedAt;
}

#endregion
