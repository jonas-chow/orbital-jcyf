using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class RoomListing : MonoBehaviour
{
    public TextMeshProUGUI roomName;
    public TextMeshProUGUI playerCount;

    public void ChangeText(RoomInfo roomInfo)
    {
        roomName.text = roomInfo.Name;
        playerCount.text = roomInfo.PlayerCount + " / " + roomInfo.MaxPlayers;
    }

    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(GetName());
    }

    public string GetName()
    {
        return roomName.text;
    }
}
