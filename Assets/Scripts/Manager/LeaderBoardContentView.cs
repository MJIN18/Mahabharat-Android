using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using Unity.Services.Leaderboards.Models;
using Unity.Services.Leaderboards;
using Unity.Services.Authentication;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using System.Threading.Tasks;
using System;
using UnityEngine.UI;

public class LeaderBoardContentView : MonoBehaviour
{
    #region singleton

    public static LeaderBoardContentView instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    #endregion

    #region PublicVariables

    public GameObject leaderboardEntryPrefab;

    public Transform contentContainer;

    #endregion

    string leaderboardId = "Leaderboards";

    public async void RefreshLeaderboards()
    {
        string playerName = await AuthenticationService.Instance.GetPlayerNameAsync();
        var scoresResponse = await LeaderboardsService.Instance.GetScoresAsync(leaderboardId);

        // Clear existing entries (if any)
        foreach (Transform child in contentContainer)
        {
            Destroy(child.gameObject);
        }

        // Instantiate leaderboard entries for each result in scoresResponse
        for (int i = 0; i < scoresResponse.Results.Count; i++)
        {
            var result = scoresResponse.Results[i];
            GameObject entry = Instantiate(leaderboardEntryPrefab, contentContainer);
            LeaderboardProfileUI entryUI = entry.GetComponent<LeaderboardProfileUI>();

            if(scoresResponse.Results[i].PlayerName == playerName)
            {
                entry.GetComponent<Image>().color = new Color32(135,248,99,255);
            }

            // Set the data for the entry
            entryUI.SetData(result.PlayerName, result.Rank, result.Score);
        }

        Debug.Log(JsonConvert.SerializeObject(scoresResponse));
    }

    public async void RefreshScore()
    {
        await GetPlayersScore();
    }

    public async Task<int> GetPlayersScore()
    {
        try
        {
            var scoreResponse = await LeaderboardsService.Instance.GetPlayerScoreAsync(leaderboardId);

            return (int)scoreResponse.Score;
        }
        catch (Exception ex)
        {
            // Handle the exception here, such as logging it or displaying an error message.
            // You can also choose to return a default value or handle the error differently.
            Debug.LogError("An error occurred while fetching player score: " + ex.Message);
            LeaderboardsManager.instance.AddScore(0);
            return 0;// Return a default value or an error code as needed.
        }
    }
}

public class LeaderboardPlayer
{
    public string playerName;
    public string playerScore;
    public string rank;
}
