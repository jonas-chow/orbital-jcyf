using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class PlayMenu : MonoBehaviourPunCallbacks
{
    public GameObject multiplayerLobby;
    public GameObject practiceSetup;

    public void Back()
    {
        AudioManager.Instance.Play("Click");
        gameObject.SetActive(false);
    }

    public void Practice()
    {
        AudioManager.Instance.Play("Click");
        practiceSetup.SetActive(true);
        // 1 for practice mode
        PlayerPrefs.SetInt("Mode", 1);
    }

    public void Connect()
    {
        AudioManager.Instance.Play("Click");
        PhotonNetwork.NickName = PlayerPrefs.GetString("Username", "");
        PhotonNetwork.GameVersion = "v1";
        Popup.StartPopup("Connecting...");
        // 0 for multiplayer mode
        PlayerPrefs.SetInt("Mode", 0);
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.ConnectToRegion("asia");
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
        multiplayerLobby.SetActive(true);
        Popup.StopPopup();
    }
}
