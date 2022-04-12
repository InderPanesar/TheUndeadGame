using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Photon level Handler to keep both players in sync.
/// </summary>
public class PhotonLevelScript : MonoBehaviourPunCallbacks
{
    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        PhotonNetwork.AutomaticallySyncScene = false;
        if (PhotonNetwork.CurrentRoom != null)
        {
            PhotonNetwork.LeaveRoom();
        }
        PhotonNetwork.Disconnect();
        SceneManager.LoadSceneAsync("MainMenu");
    }
}
