using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class MultiplayerLobby : MonoBehaviour
{
    public TMP_InputField roomName;
    public Transform content;
    public RoomListing listing;

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

    public void RandomGame()
    {
        PhotonNetwork.JoinRandomRoom();
    }
}
