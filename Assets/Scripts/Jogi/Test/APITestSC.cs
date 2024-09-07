using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class APITestSC : MonoBehaviour
{
    public string username;
    public float tokenToWithdraw;

    public string _userID;
    public int _coin;
    public int _token;

    public int _newCoins = 5;
    public int _newTokens = 2;

    public async void SignIn()
    {
        await GameManager.instance.SendSignUpRequestForm(username);

        SendData();
    }

    public async void SendData()
    {
        await GameManager.instance.UpdateTokensAndCoins(username, _newCoins, _newTokens);

        GetData();
    }

    public async void GetData()
    {
        var data = await GameManager.instance.GetTokensAndCoins(username);

        Debug.Log("this is userID = " + data.userId);

        _userID = data.userId;
        _coin = data.coin;
        _token = data.token;
    }

    public async void GetLimit()
    {
        var limit = await GameManager.instance.GetWithdrawalLimit();

        Debug.Log("withdrawal Limit is " + limit.limit);
    }

    public async void SendWithdrawRequest()
    {
        await GameManager.instance.RequestWithdraw(username, "00", tokenToWithdraw.ToString());

        GetData();
    }

    public async void RequestHistory()
    {
        await GameManager.instance.RequestHistory(username, 1, 10);
    }
}
