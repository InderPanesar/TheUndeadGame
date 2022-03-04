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

    void Start()
    {
        createRoomButton.onClick.AddListener(delegate { CreateRoom(); });
        joinRoomButton.onClick.AddListener(delegate { JoinRoom(); });
    }


    void CreateRoom()
    {
        PhotonNetwork.CreateRoom(createRoom.text);
    }

    void JoinRoom()
    {
        print(joinRoom.text);
        PhotonNetwork.JoinRoom(joinRoom.text);
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("Level1Multiplayer");
    }

}
