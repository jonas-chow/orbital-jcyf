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
    private const byte ConcedeEvent = 7;
    public string enemyName;

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
        // creates the victory screen if opponent leaves before you
        GameManager.Instance.OpponentDisconnect();
    }

    public void Rematch()
    {
        AudioManager.Instance.Play("Click");
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

        if (PhotonNetwork.IsConnected)
        {
            foreach (KeyValuePair<int, Player> kvp in PhotonNetwork.CurrentRoom.Players)
            {
                Player player = kvp.Value;
                if (!player.Equals(PhotonNetwork.LocalPlayer)) {
                    enemyName = player.NickName;
                }
            }
        }
    }

    void Start()
    {
        AudioManager.Instance.Stop("MenuTheme");
        AudioManager.Instance.Play("BattleTheme");
        if (PhotonNetwork.IsConnected)
        {
            object[] data = new object[] {
                PlayerPrefs.GetInt("Melee", 0),
                PlayerPrefs.GetInt("Ranged", 0),
                PlayerPrefs.GetInt("Mage", 0),
            };
            
            PhotonNetwork.RaiseEvent(InstantiateEvent, data, RaiseEventOptions.Default,SendOptions.SendReliable);
        }
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
        string direction;
        bool isMove;
        int charId;
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
                    switch (direction)
                    {
                        case "up":
                            ActionQueue.Instance.RecordEnemyEvent(new MoveUp(cm));
                            break;
                        case "down":
                            ActionQueue.Instance.RecordEnemyEvent(new MoveDown(cm));
                            break;
                        case "left":
                            ActionQueue.Instance.RecordEnemyEvent(new MoveLeft(cm));
                            break;
                        case "right":
                            ActionQueue.Instance.RecordEnemyEvent(new MoveRight(cm));
                            break;
                        default:
                            break;
                    }
                } else {
                    cm.Face(direction);
                    switch (direction)
                    {
                        case "up":
                            ActionQueue.Instance.RecordEnemyEvent(new FaceUp(cm));
                            break;
                        case "down":
                            ActionQueue.Instance.RecordEnemyEvent(new FaceDown(cm));
                            break;
                        case "left":
                            ActionQueue.Instance.RecordEnemyEvent(new FaceLeft(cm));
                            break;
                        case "right":
                            ActionQueue.Instance.RecordEnemyEvent(new FaceRight(cm));
                            break;
                        default:
                            break;
                    }
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
            case ConcedeEvent:
                GameManager.Instance.OpponentConcede();
                break;
        }
    }

    public void SendMovementEvent(int charId, string direction, bool isMove)
    {
        if (PhotonNetwork.IsConnected)
        {
            object[] data = new object[] {charId, direction, isMove};
            PhotonNetwork.RaiseEvent(MovementEvent, data, RaiseEventOptions.Default, SendOptions.SendReliable);
        }
    }

    public void SendAttackEvent(int charId, int attackId, object[] extraData)
    {
        if (PhotonNetwork.IsConnected)
        {
            object[] data = new object[] {charId, attackId, extraData};
            PhotonNetwork.RaiseEvent(AttackEvent, data, RaiseEventOptions.Default, SendOptions.SendReliable);
        }
    }

    public void SendTurnEndEvent()
    {
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.RaiseEvent(TurnEndEvent, null, RaiseEventOptions.Default, SendOptions.SendReliable);
        }
    }

    public void SendReady()
    {
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.RaiseEvent(ReadyEvent, null, RaiseEventOptions.Default, SendOptions.SendReliable);
        }
    }

    public void SendConcedeEvent()
    {
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.RaiseEvent(ConcedeEvent, null, RaiseEventOptions.Default, SendOptions.SendReliable);
        }
    }
}