using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using LootLocker.Requests;
using UnityEngine.AI;
using Photon.Pun;

public class RaycastTargetScript : MonoBehaviourPunCallbacks
{
    public float health = 50f;
    private Text scoreText;
    public int ScoreLimit;

    private Vector3 initialPosition;

    public NavMeshAgent agent;
    private GameObject[] players;
    private PhotonView view;


    private void Start()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        view = GetComponent<PhotonView>();
    }




    public void Update()
    {
        int[] values = new int[players.Length];
        foreach (GameObject player in players) {
            float dist = Vector3.Distance(agent.transform.position, player.transform.position);
            if(dist < 50)
            {
                agent.destination =  player.transform.position;
            }
            else
            {
                agent.destination = agent.transform.position;
            }
            print(dist);
        }
    }




    public void takeDamage(float damageValue, bool isSinglePlayer)
    {

        health -= damageValue;
        if (isSinglePlayer)
        {
            if (this.health <= 0)
            {
                DieSinglePlayer();
            }
        }
        else
        {
            if (this.health <= 0)
            {
                photonView.RPC("Die", RpcTarget.MasterClient);
            }
        }

    }

    [PunRPC]
    public void Die()
    {
        PhotonNetwork.Destroy(view);

        foreach (GameObject player in players)
        {
            PlayerMovementScript movementScript = player.GetComponent<PlayerMovementScript>();
            movementScript.UpdateScore();

        }
    }

    public void DieSinglePlayer()
    {
        Destroy(gameObject);

        foreach (GameObject player in players)
        {
            PlayerMovementScript movementScript = player.GetComponent<PlayerMovementScript>();
            movementScript.UpdateScore();

        }
    }


}
