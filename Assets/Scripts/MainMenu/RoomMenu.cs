using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
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
    private ExitGames.Client.Photon.Hashtable notReady = new ExitGames.Client.Photon.Hashtable();
    private ExitGames.Client.Photon.Hashtable ready = new ExitGames.Client.Photon.Hashtable();
    private const byte readyEvent = 199;
    private const byte startEvent = 198;

    public override void OnEnable()
    {
        base.OnEnable();
        PhotonNetwork.NetworkingClient.EventReceived += EventReceived;
        room = PhotonNetwork.CurrentRoom;
        roomName.text = room.Name;
        if (!ready.ContainsKey("ready")) {
            ready.Add("ready", true);
            notReady.Add("ready", false);
        }

        player1 = PhotonNetwork.LocalPlayer;
        PhotonNetwork.SetPlayerCustomProperties(notReady);
        p1Ready.SetActive(false);

        foreach (KeyValuePair<int, Player> kvp in room.Players)
        {
            Player player = kvp.Value;
            if (!player.Equals(PhotonNetwork.LocalPlayer)) {
                player2 = player;
                if (player.CustomProperties.ContainsKey("ready") && (bool)player.CustomProperties["ready"]) {
                    p2Ready.SetActive(true);
                }
            }
        }

        p1Name.text = player1.NickName;
        p2Name.text = player2 != null ? player2.NickName : "Waiting for players";
    }

    public override void OnDisable()
    {
        base.OnDisable();
        PhotonNetwork.NetworkingClient.EventReceived -= EventReceived;
    }

    public override void OnPlayerEnteredRoom(Player player)
    {
        player2 = player;
        p2Name.text = player.NickName;
    }

    public override void OnPlayerLeftRoom(Player player)
    {
        player2 = null;
        p2Name.text = "Waiting for players";
        p2Ready.SetActive(false);
    }

    public void LeaveRoom()
    {
        Popup.StartPopup("Leaving...");
        PhotonNetwork.LeaveRoom();
    }

    public void OpenLoadout()
    {
        loadout.SetActive(true);
    }

    public override void OnLeftRoom()
    {
        gameObject.SetActive(false);
        Popup.StopPopup();
    }

    public void ClickReady()
    {
        PhotonNetwork.SetPlayerCustomProperties(p1Ready.activeSelf ? notReady : ready);
        PhotonNetwork.RaiseEvent(readyEvent, null, RaiseEventOptions.Default, SendOptions.SendReliable);
        p1Ready.SetActive(!p1Ready.activeSelf);

        if (p1Ready.activeSelf && p2Ready.activeSelf && PhotonNetwork.IsMasterClient) {
            startButton.SetActive(true);
        }
    }

    public void ClickStart()
    {
        PhotonNetwork.RaiseEvent(startEvent, null, RaiseEventOptions.Default, SendOptions.SendReliable);
        PhotonNetwork.SetPlayerCustomProperties(notReady);
        PhotonNetwork.LoadLevel(1);
    }

    public void EventReceived(EventData eventData)
    {
        switch (eventData.Code)
        {
            case readyEvent:
                p2Ready.SetActive(!p2Ready.activeSelf);
                if (p1Ready.activeSelf && p2Ready.activeSelf && PhotonNetwork.IsMasterClient) {
                    startButton.SetActive(true);
                }
                break;
            case startEvent:
                PhotonNetwork.SetPlayerCustomProperties(notReady);
                PhotonNetwork.LoadLevel(1);
                break;
        }
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        gameObject.SetActive(false);
    }
}
