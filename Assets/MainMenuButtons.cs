using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class MainMenuButtons : MonoBehaviourPunCallbacks
{
    public GameObject multiplayerMenu;
    public GameObject roomMenu;

    public void PlayGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void Connect()
    {
        Debug.Log("Connecting");
        PhotonNetwork.NickName = PlayerPrefs.GetString("Username", "");
        if (PhotonNetwork.NickName != "") {
            PhotonNetwork.GameVersion = "v1";
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected");
        PhotonNetwork.JoinLobby();
        multiplayerMenu.SetActive(true);
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
}
