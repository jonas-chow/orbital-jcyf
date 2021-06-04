using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class MultiplayerLobby : MonoBehaviourPunCallbacks
{
    public TMP_InputField roomName;
    public Transform content;
    public RoomListing listing;
    public GameObject roomMenu;

    public void BackButton()
    {
        gameObject.SetActive(false);
        PhotonNetwork.Disconnect();
    }

    public void CreateGame()
    {
        if (roomName.text != "") {
            RoomOptions options = new RoomOptions();
            options.MaxPlayers = 2;
            PhotonNetwork.CreateRoom(roomName.text, options, TypedLobby.Default);
        }
    }

    public void JoinGame()
    {
        if (roomName.text != "") {
            PhotonNetwork.JoinRoom(roomName.text);
        }
    }

    private void CreateRandomGame()
    {
        RoomOptions options = new RoomOptions();
        options.MaxPlayers = 2;
        PhotonNetwork.CreateRoom("", options, TypedLobby.Default);
    }

    public void RandomGame()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnCreatedRoom()
    {
        roomMenu.SetActive(true);
        roomMenu.GetComponent<RoomMenu>().init();
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Room already exists");
        Debug.Log(message);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("No room");
    }

    public override void OnJoinedRoom()
    {
        roomMenu.SetActive(true);
        roomMenu.GetComponent<RoomMenu>().init();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("No rooms available, making one");
        CreateRandomGame();
    }
}
