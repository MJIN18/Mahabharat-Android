using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSaveData : MonoBehaviour
{
    public static PlayerSaveData Instance;

    //public string _playerName;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public int GetPlayerData()
    {
        return PlayerPrefs.GetInt("PlayerName");
    }
    public void SavePlayerData(int _playerName)
    {
        PlayerPrefs.SetInt("PlayerName", _playerName);
    }
}
