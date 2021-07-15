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
    public GameObject playMenu;
    public GameObject loadout;
    public GameObject controls;

    public void Quit()
    {
        Application.Quit();
    }
    
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

    public void OpenPlayMenu()
    {
        AudioManager.Instance.Play("Click");
        if (PlayerPrefs.GetString("Username", "") != "") {
            playMenu.SetActive(true);
        } else {
            Popup.Notify("Please enter name");
        }
    }

    void Start()
    {
        if (PhotonNetwork.IsConnected) {
            playMenu.SetActive(true);
            multiplayerLobby.SetActive(true);
        }
        if (PhotonNetwork.CurrentRoom != null) {
            multiplayerRoom.SetActive(true);
        }

        AudioManager.Instance.SetBGMVolume(PlayerPrefs.GetFloat("BGM", 0.1f));
        AudioManager.Instance.SetSoundEffectVolume(PlayerPrefs.GetFloat("SE", 1f));
    }
}
