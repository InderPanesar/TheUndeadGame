using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Serialised Leaderboard Result Model from the JSON web request.
/// </summary>
[Serializable]
public class LeaderboardResult
{
    public float score { get; set; }
}

/// <summary>
///  Singleton Class to add or retrieve the backendless high scores for a level.
/// </summary>
public class LeaderboardScript 
{

    private LeaderboardScript() { }
    private static LeaderboardScript instance { get; set; }

    public static LeaderboardScript Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new LeaderboardScript();
            }
            return instance;
        }
    }

    /// <summary>
    /// Retrieves the high scores for a specific Level from Backendless
    /// </summary>
    /// <param name="tableName">The name of the level</param>
    /// <param name="maxScore">Number of scores to be returned</param>
    /// <param name="callback">Method to be called once web request completed - returns list of results</param>
    public IEnumerator GetHighScores(string tableName, int maxScore, System.Action<List<LeaderboardResult>> callback = null)
    {

        using (UnityWebRequest webRequest = UnityWebRequest.Get("https://eu-api.backendless.com/E7AD2B56-F5DA-9783-FFD1-CD9846D91500/168B8736-3464-44E8-86F8-635C5C8D9B0F/data/" + tableName))
        {
            webRequest.SetRequestHeader("Content-Type", "application/json");
            List<LeaderboardResult> tempResults = new List<LeaderboardResult>();

            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.DataProcessingError)
            {
                Debug.LogError(webRequest.error);
            }
            else if (webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError(webRequest.error);
            }
            else
            {
                LeaderboardResult[] results = JsonConvert.DeserializeObject<LeaderboardResult[]>(webRequest.downloadHandler.text);

                Array.Sort(results, delegate (LeaderboardResult one, LeaderboardResult two) { return one.score.CompareTo(two.score); });

                for (int i = 0; i < results.Length; i++)
                {
                    tempResults.Add(results[i]);
                    if (i >= maxScore) break;
                }


            }
            callback.Invoke(tempResults);
            webRequest.Dispose();
        }



    }

    /// <summary>
    /// Add a score to the high scores for a specific Level from Backendless
    /// </summary>
    /// <param name="lengthOfTime">The time for user to complete level.</param>
    /// <param name="tableName">The name of the level.</param>
    /// <param name="scoreHandled">Action returns bool when score added.</param>
    public IEnumerator AddScore(float lengthOfTime, String tableName, Action<bool> scoreHandled)
    {


        LeaderboardResult result = new LeaderboardResult();
        result.score = lengthOfTime;
        String value = JsonConvert.SerializeObject(result);


        using (UnityWebRequest webRequest = UnityWebRequest.Put("https://eu-api.backendless.com/E7AD2B56-F5DA-9783-FFD1-CD9846D91500/168B8736-3464-44E8-86F8-635C5C8D9B0F/data/" + tableName, value))
        {
            webRequest.SetRequestHeader("Content-Type", "application/json");
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.DataProcessingError)
            {
                scoreHandled(false);
            }
            else if (webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                scoreHandled(false);
            }
            else { 
                scoreHandled(true);
            }


        }
       



    }
}
