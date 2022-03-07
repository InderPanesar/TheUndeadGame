using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPlayersScript : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject enemyPrefab;

    public float minimumXValue;
    public float maximumXValue;
    public float minimumZValue;
    public float maximumZValue;
    // Start is called before the first frame update
    void Start()
    {
        Vector3 randomPosition = new Vector3(Random.Range(minimumXValue, maximumXValue), 1, Random.Range(minimumZValue, maximumZValue));
        Debug.Log(randomPosition);
        PhotonNetwork.Instantiate(playerPrefab.name, randomPosition, Quaternion.identity);
        if(PhotonNetwork.IsMasterClient)
        {
            for (int i = 0; i < 6; i++)
            {
                randomPosition = new Vector3(Random.Range(minimumXValue, maximumXValue), 1, Random.Range(minimumZValue, maximumZValue));
                Debug.Log(randomPosition);
                PhotonNetwork.Instantiate(enemyPrefab.name, randomPosition, Quaternion.identity);
            }
        }

    }

}
