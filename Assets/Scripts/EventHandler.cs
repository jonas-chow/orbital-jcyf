using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class EventHandler : MonoBehaviourPunCallbacks
{
    private static EventHandler instance;
    public static EventHandler Instance { get { return instance; } }

    private const byte MovementEvent = 1;
    private const byte AttackEvent = 2;
    private const byte TurnEndEvent = 3;
    private const byte InstantiateEvent = 4;
    private const byte FirstTurnEvent = 5;
    private const byte ReadyEvent = 6;

    public void MainMenu()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        PhotonNetwork.LoadLevel(0);
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        PhotonNetwork.LoadLevel(0);
    }

    public override void OnPlayerLeftRoom(Player player)
    {
        GameManager.Instance.OpponentDisconnect();
    }

    public void Rematch()
    {
        PhotonNetwork.LoadLevel(0);
    }
    
    // ensures that there is only one event handler, for easy reference
    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        } else {
            instance = this;
        }
    }

    void Start()
    {
        object[] data = new object[] {
            PlayerPrefs.GetInt("Melee", 0),
            PlayerPrefs.GetInt("Ranged", 0),
            PlayerPrefs.GetInt("Mage", 0),
        };
        
        PhotonNetwork.RaiseEvent(InstantiateEvent, data, RaiseEventOptions.Default,SendOptions.SendReliable);
    }

    public void FlipCoin()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            bool startFirst = UnityEngine.Random.Range(0, 2) == 0;
            PhotonNetwork.RaiseEvent(FirstTurnEvent, startFirst, RaiseEventOptions.Default, SendOptions.SendReliable);
            if (startFirst)
            {
                GameManager.Instance.readyForTurn = true;
            } else {
                GameManager.Instance.EnemyTurn();
            }
        }
    }

    public override void OnEnable()
    {
        base.OnEnable();
        PhotonNetwork.NetworkingClient.EventReceived += EventReceived;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        PhotonNetwork.NetworkingClient.EventReceived -= EventReceived;
    }

    public void EventReceived(EventData eventData)
    {
        object[] data;
        int charX;
        int charY;
        string direction;
        bool isMove;
        int range;
        int damage;
        int charId;
        int targetX;
        int targetY;
        CharacterMovement cm;
        switch (eventData.Code)
        {
            case MovementEvent:
                data = (object[])eventData.CustomData;
                charId = (int)data[0];
                direction = (string)data[1];
                isMove = (bool)data[2];

                cm = GameManager.Instance.enemies[charId];
                if (isMove)
                {
                    cm.Move(direction);
                } else {
                    cm.Face(direction);
                }
                break;
            case AttackEvent:
                data = (object[])eventData.CustomData;
                charId = (int)data[0];
                int attackId = (int)data[1];
                object[] extraData = (object[])data[2];
                GameManager.Instance.enemies[charId].EventAttack(attackId, extraData);
                break;
            case TurnEndEvent:
                GameManager.Instance.readyForTurn = true;
                break;
            case InstantiateEvent:
                data = (object[])eventData.CustomData;
                
                GameManager.Instance.InstantiateEnemies((int)data[0], (int)data[1], (int)data[2]); 
                break;
            case FirstTurnEvent:
                bool enemyFirst = (bool)eventData.CustomData;
                if (enemyFirst) {
                    GameManager.Instance.EnemyTurn();
                } else {
                    GameManager.Instance.readyForTurn = true;
                }
                break;
            case ReadyEvent:
                GameManager.Instance.enemyReady = true;
                GameManager.Instance.CheckBothReady();
                break;
        }
    }

    public void SendMovementEvent(int charId, string direction, bool isMove)
    {
        object[] data = new object[] {charId, direction, isMove};
        PhotonNetwork.RaiseEvent(MovementEvent, data, RaiseEventOptions.Default, SendOptions.SendReliable);
    }

    public void SendAttackEvent(int charId, int attackId, object[] extraData)
    {
        object[] data = new object[] {charId, attackId, extraData};
        PhotonNetwork.RaiseEvent(AttackEvent, data, RaiseEventOptions.Default, SendOptions.SendReliable);
    }

    public void SendTurnEndEvent()
    {
        PhotonNetwork.RaiseEvent(TurnEndEvent, null, RaiseEventOptions.Default, SendOptions.SendReliable);
    }

    public void SendReady()
    {
        PhotonNetwork.RaiseEvent(ReadyEvent, null, RaiseEventOptions.Default, SendOptions.SendReliable);
    }
}