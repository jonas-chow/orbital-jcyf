using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class MainMenuButtons : MonoBehaviourPunCallbacks
{
    public GameObject multiplayerLobby;
    public GameObject multiplayerRoom;
    public GameObject loadout;

    public void OpenLoadout()
    {
        loadout.SetActive(true);
    }

    void Start()
    {
        if (PhotonNetwork.IsConnected) {
            multiplayerLobby.SetActive(true);
        }
        if (PhotonNetwork.CurrentRoom != null) {
            multiplayerRoom.SetActive(true);
        }
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
        multiplayerLobby.SetActive(true);
    }
}
