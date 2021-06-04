using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.CreateRoom(null, new RoomOptions());
        Debug.Log("hi");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void connect()
    {
        if (PhotonNetwork.IsConnected)
        {
            Debug.Log("wasConnected");
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            Debug.Log("trying to connect");
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("connected");
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("no random room");
    }
}
