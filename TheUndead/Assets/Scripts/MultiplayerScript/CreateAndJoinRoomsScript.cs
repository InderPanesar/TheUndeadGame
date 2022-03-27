using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CreateAndJoinRoomsScript : MonoBehaviourPunCallbacks
{
    public Text currentTitleText;

    public InputField createRoom;
    public InputField joinRoom;

    public Button createRoomButton;
    public Button joinRoomButton;
    public Button backButton;
    public Button playButton;

    private bool hasJoinedRoom = false;


    void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;

        createRoomButton.onClick.AddListener(delegate { CreateRoom(); });
        joinRoomButton.onClick.AddListener(delegate { JoinRoom(); });
        backButton.onClick.AddListener(delegate { BackButton(); });
        playButton.onClick.AddListener(delegate { OnPlay(); });

        playButton.interactable = false;
    }


    void CreateRoom()
    {
        string value = PlayerPrefs.GetString("MultiplayerLevel", "Unknown");
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 2;
        PhotonNetwork.CreateRoom(value + "_" + createRoom.text, roomOptions, null);
    }

    void JoinRoom()
    {
        string value = PlayerPrefs.GetString("MultiplayerLevel", "Unknown");

        PhotonNetwork.JoinRoom(value + "_" + joinRoom.text);
    }

    void BackButton()
    {
        if (hasJoinedRoom)
        {
            PhotonNetwork.LeaveRoom();
            createRoomButton.interactable = true;
            joinRoomButton.interactable = true;
            createRoom.interactable = true;
            joinRoom.interactable = true;
            hasJoinedRoom = false;
            playButton.interactable = false;

        }
        SceneManager.LoadSceneAsync("MultiplayerLevelSelection");
    }

    void OnPlay()
    {
        string value = PlayerPrefs.GetString("MultiplayerLevel", "Unknown");
        PhotonNetwork.CurrentRoom.IsOpen = false;

        if (value == "MPLevel1")
        {
            PhotonNetwork.LoadLevel("Level1Multiplayer");
        }
        else if (value == "MPLevel2")
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

    public override void OnJoinedRoom()
    {
        print("HIT!"); 
        currentTitleText.text = "Current Room: " + PhotonNetwork.CurrentRoom.Name;
        createRoomButton.interactable = false;
        joinRoomButton.interactable = false;
        createRoom.interactable = false;
        joinRoom.interactable = false;
        hasJoinedRoom = true;
        if (PhotonNetwork.IsMasterClient)
        {
            playButton.interactable = true;
            currentTitleText.text = "Current Room: " + PhotonNetwork.CurrentRoom.Name + " Host";
        }
        else
        {
            currentTitleText.text = "Current Room: " + PhotonNetwork.CurrentRoom.Name + " Not Host";
        }


    }

}
