using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using LootLocker.Requests;
using Random = System.Random;
using UnityEngine.AI;

public class RaycastTargetScript : MonoBehaviour
{
    public float health = 50f;
    public Text scoreText;
    public int ScoreLimit;

    private Vector3 initialPosition;

    public NavMeshAgent agent;
    public GameObject player;
    



    public void Update()
    {
        agent.destination = player.transform.position;
    }

    public void SubmitScoreToLeaderboard(float time)
    {
        int timeInt = (int)Math.Round(time);
        Random rand = new Random();
        int userID = rand.Next(100000, 999999999);
        LootLockerSDKManager.SubmitScore(userID.ToString(), timeInt, "1643", (response) =>
        {
            if (response.success)
            {
                Debug.Log("Leaderboard Score Added");
            }
            else
            {
                Debug.Log("Leaderboard Score Not Added");
            }
        });
    }


    public void takeDamage(float damageValue)
    {
        health -= damageValue;
        if(health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
        UpdateScore();
    }

    private void UpdateScore()
    {
        int score = PlayerPrefs.GetInt("level1score");
        score++;
        PlayerPrefs.SetInt("level1score", score);
        PlayerPrefs.Save();
        scoreText.text = "Player Score: " + score;
        if (score == ScoreLimit)
        {
            CompleteLevel();
        }
    }

    private void CompleteLevel()
    {
        //ToDo: Update with new string when implemented.
        PlayerPrefs.SetFloat("levelCompleteTime", Time.timeSinceLevelLoad);
        SubmitScoreToLeaderboard(Time.timeSinceLevelLoad);
        PlayerPrefs.SetString("currentLevel", "Level1");
        PlayerPrefs.Save();
        SceneManager.LoadSceneAsync("GameWinScene");
    }
}
