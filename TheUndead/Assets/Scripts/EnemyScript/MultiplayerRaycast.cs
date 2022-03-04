using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using LootLocker.Requests;
using Random = System.Random;
using UnityEngine.AI;

public class MultiplayerRaycast : MonoBehaviour
{
    public float health = 50f;
    public Text scoreText;
    public int ScoreLimit;

    private Vector3 initialPosition;

    public GameObject player;




    public void Update()
    {
    }



    public void takeDamage(float damageValue)
    {
        health -= damageValue;
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
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
        SceneManager.LoadSceneAsync("GameWinScene");
    }
}
