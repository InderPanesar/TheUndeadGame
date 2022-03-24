using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.AI;
using Photon.Pun;

enum RayCastTargetState
{
    idle,
    chase,
    attack,
    patrol,
}
public class RaycastTargetScript : MonoBehaviourPunCallbacks
{
    public float health = 50f;
    public float attackRange = 3f;
    public int ScoreLimit;
    public NavMeshAgent agent;
    private GameObject[] players;
    private PhotonView view;
    private Animator animator;
    private RayCastTargetState state;

    public Transform[] locationsToMoveTo;
    private int randomSpot;

    private float idleTime = 5;
    private float currentIdleTime;

    private void Start()
    {
        view = GetComponent<PhotonView>();
        animator = GetComponent<Animator>();

        if (locationsToMoveTo != null)
        {
            randomSpot = UnityEngine.Random.Range(0, locationsToMoveTo.Length);
            currentIdleTime = idleTime;
        }

        if (locationsToMoveTo.Length == 0 || locationsToMoveTo == null)
        {
            GameObject[] waypoints = GameObject.FindGameObjectsWithTag("Waypoints");
            locationsToMoveTo = new Transform[waypoints.Length];

            for (int i = 0; i < waypoints.Length; i++)
            {
                locationsToMoveTo[i] = waypoints[i].transform;
            }

            Debug.Log(locationsToMoveTo.Length);

            randomSpot = UnityEngine.Random.Range(0, locationsToMoveTo.Length);
            Debug.Log(randomSpot);

            currentIdleTime = idleTime;
        }

    }




    public void Update()
    {
        if(photonView == null)
        {
            targetMovement();
        }
        else
        {
            if(PhotonNetwork.IsMasterClient)
            {
                targetMovement();
            }
        }
    }

    private void targetMovement()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players)
        {
            float dist = Vector3.Distance(agent.transform.position, player.transform.position);
            if (dist < 15)
            {
                agent.destination = player.transform.position;
                animator.SetFloat(Animator.StringToHash("walkingSpeed"), 10);
                if (dist <= attackRange)
                {
                    animator.SetLayerWeight(animator.GetLayerIndex("Movement Layer"), 0);
                    animator.SetFloat(Animator.StringToHash("walkingSpeed"), 0);
                    animator.SetBool(Animator.StringToHash("attack"), true);
                    agent.isStopped = true;
                    state = RayCastTargetState.attack;
                }
                else
                {
                    animator.SetLayerWeight(animator.GetLayerIndex("Movement Layer"), 1);
                    animator.SetFloat(Animator.StringToHash("walkingSpeed"), 10);
                    animator.SetBool(Animator.StringToHash("attack"), false);
                    agent.isStopped = false;
                    state = RayCastTargetState.chase;
                }
                break;
            }
            else
            {
                if (locationsToMoveTo != null && locationsToMoveTo.Length > 0)
                {

                    agent.destination = locationsToMoveTo[randomSpot].position;
                    animator.SetFloat(Animator.StringToHash("walkingSpeed"), 10);
                    if (Vector3.Distance(transform.position, locationsToMoveTo[randomSpot].position) < 0.2f)
                    {
                        if (currentIdleTime <= 0)
                        {
                            randomSpot = UnityEngine.Random.Range(0, locationsToMoveTo.Length);
                            currentIdleTime = idleTime;
                            state = RayCastTargetState.patrol;
                        }
                        else
                        {
                            currentIdleTime -= Time.deltaTime;
                            animator.SetFloat(Animator.StringToHash("walkingSpeed"), 0);
                            state = RayCastTargetState.idle;
                        }

                    }
                }
                else
                {
                    agent.destination = agent.transform.position;
                    animator.SetFloat(Animator.StringToHash("walkingSpeed"), 0);
                    state = RayCastTargetState.idle;
                }

            }
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

    public void LoadSaveFile(EnemySaveInformation saveInformation)
    {
        this.health = saveInformation.health;
        transform.position = saveInformation.position;
        transform.rotation = saveInformation.rotation;

    }


}
