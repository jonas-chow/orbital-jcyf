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
    public GameObject roomMenu;

    public void BackButton()
    {
        AudioManager.Instance.Play("Click");
        Popup.StartPopup("Disconnecting...");
        PhotonNetwork.Disconnect();
    }

    public void CreateGame()
    {
        AudioManager.Instance.Play("Click");
        if (roomName.text != "") {
            Popup.StartPopup("Creating room...");
            RoomOptions options = new RoomOptions();
            options.MaxPlayers = 2;
            PhotonNetwork.CreateRoom(roomName.text, options, TypedLobby.Default);
        } else {
            Popup.Notify("Please enter room name");
        }
    }

    public void JoinGame()
    {
        AudioManager.Instance.Play("Click");
        if (roomName.text != "") {
            Popup.StartPopup("Joining room...");
            PhotonNetwork.JoinRoom(roomName.text);
        } else {
            Popup.Notify("Please enter room name");
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
        AudioManager.Instance.Play("Click");
        Popup.StartPopup("Joining room...");
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnCreatedRoom()
    {
        Popup.StopPopup();
        roomMenu.SetActive(true);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Popup.StopPopup();
        Popup.Notify(message);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Popup.StopPopup();
        Popup.Notify(message);
    }

    public override void OnJoinedRoom()
    {
        Popup.StopPopup();
        roomMenu.SetActive(true);
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Popup.StopPopup();
        Popup.Notify(message);
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Popup.StopPopup();
        if (cause != DisconnectCause.DisconnectByClientLogic) {
            Popup.Notify("Disconnected");
        }
        gameObject.SetActive(false);
    }
}
