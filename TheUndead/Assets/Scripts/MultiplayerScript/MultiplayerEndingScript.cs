using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Script handles the ending scene of Multiplayer.
/// </summary>
public class MultiplayerEndingScript : MonoBehaviourPunCallbacks
{
    [SerializeField] private Button mainMenuButton;

    void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        mainMenuButton.onClick.AddListener(MainMenuButtonClick);

        PhotonNetwork.AutomaticallySyncScene = false;

    }

    /// <summary>
    /// Handling of the Main menu button click.
    /// </summary>
    void MainMenuButtonClick()
    {
        if (PhotonNetwork.CurrentRoom != null)
        {
            PhotonNetwork.LeaveRoom();
        }
        PhotonNetwork.Disconnect();
        SceneManager.LoadSceneAsync("MainMenu");
    }

}
