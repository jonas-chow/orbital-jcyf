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
    private const byte InstantiateEvent = 5;

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
            PlayerPrefs.GetString("Melee", "Melee1"),
            InvertXCoord(PlayerPrefs.GetInt("MeleeX", 0)),
            InvertYCoord(PlayerPrefs.GetInt("MeleeY", 0)),
            PlayerPrefs.GetString("Ranged", "Ranged1"),
            InvertXCoord(PlayerPrefs.GetInt("RangedX", 1)),
            InvertYCoord(PlayerPrefs.GetInt("RangedY", 0)),
            PlayerPrefs.GetString("Mage", "Mage1"),
            InvertXCoord(PlayerPrefs.GetInt("MageX", 2)),
            InvertYCoord(PlayerPrefs.GetInt("MageY", 0))
        };
        
        PhotonNetwork.RaiseEvent(InstantiateEvent, data, RaiseEventOptions.Default,SendOptions.SendReliable);

        // temporary
        if (PhotonNetwork.IsMasterClient) {
            GameManager.Instance.readyForTurn = true;
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
                if (enemy != null && !enemy.isEnemy) {
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

                // Instead of finding all enemies, we find all not enemies here
                // Because the event is from the opponent
                List<CharacterMovement> enemies = GridManager.Instance
                    .GetAllCharactersInAOE(targetX, targetY)
                    .FindAll(cm => !cm.isEnemy);
                enemies.ForEach(enemy => enemy.TakeDamage(damage));
                break;
            case TurnEndEvent:
                GameManager.Instance.readyForTurn = true;
                break;
            case InstantiateEvent:
                data = (object[])eventData.CustomData;
                
                GameManager.Instance.InstantiateEnemies(
                    (string)data[0], (int)data[1], (int)data[2], 
                    (string)data[3], (int)data[4], (int)data[5], 
                    (string)data[6], (int)data[7], (int)data[8]); 
                break;
        }
    }

    public void SendMovementEvent(int charX, int charY, string direction, bool isMove)
    {
        object[] data = new object[] {
            InvertXCoord(charX), 
            InvertYCoord(charY), 
            InvertDirection(direction), 
            isMove};
        PhotonNetwork.RaiseEvent(MovementEvent, data, RaiseEventOptions.Default, SendOptions.SendReliable);
    }

    public void SendLinearAttackEvent(int charX, int charY, string direction, int range, int damage)
    {
        object[] data = new object[] {
            InvertXCoord(charX), 
            InvertYCoord(charY), 
            InvertDirection(direction), 
            range, 
            damage};
        PhotonNetwork.RaiseEvent(LinearAttackEvent, data, RaiseEventOptions.Default, SendOptions.SendReliable);
    }

    public void SendAOEAttackEvent(int charX, int charY, int targetX, int targetY, int damage)
    {
        object[] data = new object[] {
            InvertXCoord(charX), 
            InvertYCoord(charY), 
            InvertXCoord(targetX), 
            InvertYCoord(targetY), 
            damage};
        PhotonNetwork.RaiseEvent(AOEAttackEvent, data, RaiseEventOptions.Default, SendOptions.SendReliable);
    }

    public void SendTurnEndEvent()
    {
        PhotonNetwork.RaiseEvent(TurnEndEvent, null, RaiseEventOptions.Default, SendOptions.SendReliable);
    }

    private string InvertDirection(string direction)
    {
        switch (direction)
        {
            case "up":
                return "down";
            case "down": 
                return "up";
            case "left":
                return "right";
            case "right":
                return "left";
            default:
                return direction;
        }
    }

    private int InvertXCoord(int x)
    {
        return GridManager.Instance.length - 1 - x;
    }

    private int InvertYCoord(int y)
    {
        return GridManager.Instance.height - 1 - y;
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