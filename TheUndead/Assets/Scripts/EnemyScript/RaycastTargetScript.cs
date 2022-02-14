using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RaycastTargetScript : MonoBehaviour
{
    public float health = 50f;
    public Text scoreText;


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
    }
}
