using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class serves the purpose of holding all the enemy game objects at the start of the level.
/// </summary>
public class EnemiesContainerScript : MonoBehaviour
{
    public GameObject[] enemies;
    void Start()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemies");
    }

}
