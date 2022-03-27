using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CreateAndJoinRoomsScript : MonoBehaviourPunCallbacks
{
    public InputField createRoom;
    public InputField joinRoom;

    public Button createRoomButton;
    public Button joinRoomButton;
    public Button backButton;


    void Start()
    {
        createRoomButton.onClick.AddListener(delegate { CreateRoom(); });
        joinRoomButton.onClick.AddListener(delegate { JoinRoom(); });
        backButton.onClick.AddListener(delegate { BackButton(); });

    }


    void CreateRoom()
    {
        string value = PlayerPrefs.GetString("MultiplayerLevel", "Unknown");

        PhotonNetwork.CreateRoom(value + "_" + createRoom.text);
    }

    void JoinRoom()
    {
        string value = PlayerPrefs.GetString("MultiplayerLevel", "Unknown");

        PhotonNetwork.JoinRoom(value + "_" + joinRoom.text);
    }

    void BackButton()
    {
        SceneManager.LoadSceneAsync("MultiplayerLevelSelection");
    }

    public override void OnJoinedRoom()
    {
        string value = PlayerPrefs.GetString("MultiplayerLevel", "Unknown");
        if(value == "MPLevel1")
        {
            PhotonNetwork.LoadLevel("Level1Multiplayer");
        }
        else if(value == "MPLevel2")
        {
            PhotonNetwork.LoadLevel("Level2Multiplayer");
        }
        else if (value == "MPLevel3")
        {
            PhotonNetwork.LoadLevel("Level3Multiplayer");
        }
        else if (value == "MPLevel4")
        {
            PhotonNetwork.LoadLevel("Level4Multiplayer");
        }
        else if (value == "MPLevel5")
        {
            PhotonNetwork.LoadLevel("Level5Multiplayer");
        }

    }

}
