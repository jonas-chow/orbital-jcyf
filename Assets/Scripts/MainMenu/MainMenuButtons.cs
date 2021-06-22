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
    public GameObject controls;

    public void OpenLoadout()
    {
        // button click
        loadout.SetActive(true);
    }

    public void OpenControls()
    {
        controls.SetActive(true);
    }

    public void CloseControls()
    {
        controls.SetActive(false);
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
        // button click
        PhotonNetwork.NickName = PlayerPrefs.GetString("Username", "");
        if (PhotonNetwork.NickName != "") {
            PhotonNetwork.GameVersion = "v1";
            Popup.StartPopup("Connecting...");
            PhotonNetwork.ConnectUsingSettings();
        } else {
            Popup.Notify("Please enter name");
        }
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
        multiplayerLobby.SetActive(true);
        Popup.StopPopup();
    }
}
