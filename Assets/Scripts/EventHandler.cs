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
    private const byte LinearAttackEvent = 2;
    private const byte AOEAttackEvent = 3;
    private const byte TurnEndEvent = 4;
    private const byte TestEvent = 5;

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

    public override void OnEnable()
    {
        base.OnEnable();
        PhotonNetwork.NetworkingClient.EventReceived += EventReceived;

        // Get own player ID
        // Setup the game manager and instantiate the characters at appropriate locations

    }

    public override void OnDisable()
    {
        base.OnDisable();
        PhotonNetwork.NetworkingClient.EventReceived -= EventReceived;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {
            PhotonNetwork.RaiseEvent(TestEvent, null, RaiseEventOptions.Default, SendOptions.SendReliable);
        }
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
        int targetX;
        int targetY;
        CharacterMovement cm;
        switch (eventData.Code)
        {
            case MovementEvent:
                data = (object[])eventData.CustomData;
                charX = (int)data[0];
                charY = (int)data[1];
                direction = (string)data[2];
                isMove = (bool)data[3];

                cm = GridManager.Instance.GetCharacter(charX, charY);
                if (isMove)
                {
                    cm.Move(direction);
                } else {
                    cm.Face(direction);
                }
                break;
            case LinearAttackEvent:
                data = (object[])eventData.CustomData;
                charX = (int)data[0];
                charY = (int)data[1];
                direction = (string)data[2];
                range = (int)data[3];
                damage = (int)data[4];

                cm = GridManager.Instance.GetCharacter(charX, charY);
                if (direction != "none") {
                    cm.Face(direction);
                }

                CharacterMovement enemy = GridManager.Instance.GetFirstCharacterInLine(charX, charY, range, cm.faceDirection);
                if (enemy != null) {
                    enemy.TakeDamage(damage);
                }
                break;
            case AOEAttackEvent:
                data = (object[])eventData.CustomData;
                charX = (int) data[0];
                charY = (int) data[1];
                targetX = (int) data[2];
                targetY = (int)data[3];
                damage = (int)data[4];

                cm = GridManager.Instance.GetCharacter(charX, charY);
                int dirY = targetY - charY;
                int dirX = targetX - charX;
                if (Math.Abs(dirX) > Math.Abs(dirY)) {
                    // horizontal component larger than vertical
                    if (dirX > 0) {
                        cm.Face("right");
                    } else if (dirX < 0) {
                        cm.Face("left");
                    }
                } else {
                    // vertical component equal or larger to horizontal
                    if (dirY > 0) {
                        cm.Face("up");
                    } else if (dirY < 0) {
                        cm.Face("down");
                    }
                }

                // Instead of finding all not controllable, we find all controllable here
                // Because the event is from the opponent
                List<CharacterMovement> enemies = GridManager.Instance
                    .GetAllCharactersInAOE(targetX, targetY)
                    .FindAll(cm => cm.isControllable);
                enemies.ForEach(enemy => enemy.TakeDamage(damage));
                break;
            case TurnEndEvent:
                // readyForNextTurn = true;
                break;
            case TestEvent:
                Debug.Log("event happened");
                break;
        }
    }

    public void SendMovementEvent(int charX, int charY, string direction, bool isMove)
    {
        object[] data = new object[] {charX, charY, direction, isMove};
        PhotonNetwork.RaiseEvent(MovementEvent, data, RaiseEventOptions.Default, SendOptions.SendReliable);
    }

    public void SendLinearAttackEvent(int charX, int charY, string direction, int range, int damage)
    {
        object[] data = new object[] {charX, charY, direction, range, damage};
        PhotonNetwork.RaiseEvent(LinearAttackEvent, data, RaiseEventOptions.Default, SendOptions.SendReliable);
    }

    public void SendAOEAttackEvent(int charX, int charY, int targetX, int targetY, int damage)
    {
        object[] data = new object[] {charX, charY, targetX, targetY, damage};
        PhotonNetwork.RaiseEvent(AOEAttackEvent, data, RaiseEventOptions.Default, SendOptions.SendReliable);
    }

    public void SendTurnEndEvent()
    {
        PhotonNetwork.RaiseEvent(TurnEndEvent, null, RaiseEventOptions.Default, SendOptions.SendReliable);
    }
}



/*
Movement Event
    payload: charX, charY, left/right/up/down, bool true if move (false if face)
KIV in case we want some unique animations:
Attack Event
    payload: charX, charY, attackID (1/2/3/4), relevantProperties[]
Linear Attack Event
    payload: charX, charY, direction, range, damage
AOE Attack Event
    payload: targetX, targetY, damage
Turn End Event
    payload: null (just signifies that a turn ended)
*/