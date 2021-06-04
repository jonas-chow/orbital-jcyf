using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class RoomListings : MonoBehaviourPunCallbacks
{
    public RoomListing RoomListing;
    public Transform content;
    private List<RoomListing> roomListings;

    public override void OnRoomListUpdate(List<RoomInfo> roomInfos)
    {
        Debug.Log("updated");
        foreach (RoomInfo roomInfo in roomInfos)
        {
            int idx = roomListings.FindIndex(listing => listing.GetName() == roomInfo.Name);
            if (roomInfo.RemovedFromList) {
                if (idx != -1) {
                    Destroy(roomListings[idx].gameObject);
                    roomListings.RemoveAt(idx);
                }
            } else {
                // if listing doesn't exist yet
                if (idx == -1) {
                    RoomListing listing = Instantiate(RoomListing, content);
                    listing.ChangeText(roomInfo);
                    roomListings.Add(listing);
                } else {
                    roomListings[idx].ChangeText(roomInfo);
                }
            }
        }
    }
}
