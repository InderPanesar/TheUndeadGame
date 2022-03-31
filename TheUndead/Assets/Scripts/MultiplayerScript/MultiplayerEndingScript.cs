using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MultiplayerEndingScript : MonoBehaviourPunCallbacks
{
    [SerializeField] private Button mainMenuButton;


    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        mainMenuButton.onClick.AddListener(MainMenuButtonClick);

        PhotonNetwork.AutomaticallySyncScene = false;

    }

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
