using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapScript : MonoBehaviour
{
    public bool isMultiplayer = false;
    public float heightOfCamera = 39;

    void LateUpdate()
    {
        if(!isMultiplayer)
        {
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject _player in players)
            {
                transform.position = new Vector3(_player.transform.position.x, heightOfCamera, _player.transform.position.z);
            }
        }
        //Implementation For multiple players online.
        else
        {
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject _player in players)
            {
                PhotonView view = _player.GetComponent<PhotonView>();
                if (view.IsMine)
                {
                    transform.position = new Vector3(_player.transform.position.x, heightOfCamera, _player.transform.position.z);
                }
            }
        }

    }
}
