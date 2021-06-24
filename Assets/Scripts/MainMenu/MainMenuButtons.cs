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
        AudioManager.Instance.Play("Click");
        loadout.SetActive(true);
    }

    public void OpenControls()
    {
        AudioManager.Instance.Play("Click");
        controls.SetActive(true);
    }

    void Start()
    {
        if (PhotonNetwork.IsConnected) {
            multiplayerLobby.SetActive(true);
        }
        if (PhotonNetwork.CurrentRoom != null) {
            multiplayerRoom.SetActive(true);
        }

        AudioManager.Instance.SetBGMVolume(PlayerPrefs.GetFloat("BGM", 0.1f));
        AudioManager.Instance.SetSoundEffectVolume(PlayerPrefs.GetFloat("SE", 1f));
    }

    public void Connect()
    {
        AudioManager.Instance.Play("Click");
        PhotonNetwork.NickName = PlayerPrefs.GetString("Username", "");
        if (PhotonNetwork.NickName != "") {
            PhotonNetwork.GameVersion = "v1";
            Popup.StartPopup("Connecting...");
            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.ConnectToRegion("asia");
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
