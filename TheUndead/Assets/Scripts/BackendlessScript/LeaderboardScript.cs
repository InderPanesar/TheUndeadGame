using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.Networking;

[Serializable]
public class HighScoreResult
{
    public float score { get; set; }
}
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

    public IEnumerator GetHighScores(string tableName, int maxScore, System.Action<List<HighScoreResult>> callback = null)
    {

        using (UnityWebRequest webRequest = UnityWebRequest.Get("https://eu-api.backendless.com/E7AD2B56-F5DA-9783-FFD1-CD9846D91500/168B8736-3464-44E8-86F8-635C5C8D9B0F/data/" + tableName))
        {
            webRequest.SetRequestHeader("Content-Type", "application/json");
            List<HighScoreResult> tempResults = new List<HighScoreResult>();

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
                HighScoreResult[] results = JsonConvert.DeserializeObject<HighScoreResult[]>(webRequest.downloadHandler.text);

                Array.Sort(results, delegate (HighScoreResult one, HighScoreResult two) { return one.score.CompareTo(two.score); });

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

    public IEnumerator AddScore(float lengthOfTime, String tableName, Action<bool> scoreHandled)
    {


        HighScoreResult result = new HighScoreResult();
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
