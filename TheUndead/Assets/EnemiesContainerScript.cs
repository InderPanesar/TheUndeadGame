using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesContainerScript : MonoBehaviour
{
    public GameObject[] enemies;
    // Start is called before the first frame update
    void Start()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemies");
    }

}
