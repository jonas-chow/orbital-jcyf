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
    private Room room;
    private Player player1 = null;
    private Player player2 = null;
    private int other;
    public void init()
    {
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

        if (PhotonNetwork.LocalPlayer.Equals(player1)) {
            other = 2;
        } else {
            other = 1;
        }
    }
    public override void OnPlayerEnteredRoom(Player player)
    {
        if (other == 1) {
            player1 = player;
            p1Name.text = player.NickName;
        } else {
            player2 = player;
            p2Name.text = player.NickName;
        }
    }

    public override void OnPlayerLeftRoom(Player player)
    {
        if (other == 1) {
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

    public override void OnLeftRoom()
    {
        gameObject.SetActive(false);
    }
}
