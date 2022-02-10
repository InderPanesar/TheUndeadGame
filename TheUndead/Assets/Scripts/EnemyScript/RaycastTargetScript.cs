using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastTargetScript : MonoBehaviour
{
    public float health = 50f;


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
    }
}
