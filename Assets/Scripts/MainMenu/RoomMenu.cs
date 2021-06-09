using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class RoomMenu : MonoBehaviourPunCallbacks
{
    public TextMeshProUGUI roomName;

    public TextMeshProUGUI p1Name;
    public TextMeshProUGUI p2Name;
    public GameObject p1Ready;
    public GameObject p2Ready;
    public GameObject startButton;
    public GameObject loadout;

    private Room room;
    private Player player1 = null;
    private Player player2 = null;

    public override void OnEnable()
    {
        base.OnEnable();
        PhotonNetwork.AutomaticallySyncScene = true;
        room = PhotonNetwork.CurrentRoom;
        roomName.text = room.Name;
        if (room.Players.ContainsKey(1)) {
            player1 = room.Players[1];
        } 
        if (room.Players.ContainsKey(2)) {
            player2 = room.Players[2];
        }

        p1Name.text = player1 != null ? player1.NickName : "Waiting for players";
        p2Name.text = player2 != null ? player2.NickName : "Waiting for players";
    }

    public override void OnPlayerEnteredRoom(Player player)
    {
        if (PlayerId(PhotonNetwork.LocalPlayer) == 2) {
            player1 = player;
            p1Name.text = player.NickName;
        } else {
            player2 = player;
            p2Name.text = player.NickName;
        }
    }

    public override void OnPlayerLeftRoom(Player player)
    {
        if (PlayerId(player) == 1) {
            player1 = null;
            p1Name.text = "Waiting for players";
        } else {
            player2 = null;
            p2Name.text = "Waiting for players";
        }
    }

    public void LeaveRoom()
    {
        Debug.Log("leaving");
        PhotonNetwork.LeaveRoom();
    }

    public void OpenLoadout()
    {
        loadout.SetActive(true);
    }

    public override void OnLeftRoom()
    {
        gameObject.SetActive(false);
    }
    
    private int PlayerId(Player player)
    {
        if (player1.Equals(player)) {
            return 1;
        } else if (player2.Equals(player)) {
            return 2;
        } else {
            // player not in room, something went wrong
            return 0;
        }
    }

    public void ClickReady()
    {
        base.photonView.RPC("toggleReadyText", RpcTarget.AllBufferedViaServer, PhotonNetwork.LocalPlayer);
    }


    [PunRPC]
    private void toggleReadyText(Player player)
    {
        switch (PlayerId(player))
        {
            case 1:
                p1Ready.SetActive(!p1Ready.activeSelf);
                break;
            case 2:
                p2Ready.SetActive(!p2Ready.activeSelf);
                break;
        }

        if (PhotonNetwork.IsMasterClient)
        {
            if (p2Ready.activeSelf && p1Ready.activeSelf)
            {
                startButton.SetActive(true);
            } else {
                startButton.SetActive(false);
            }
        }
    }

    public void ClickStart()
    {
        PhotonNetwork.LoadLevel(2);
    }
}
