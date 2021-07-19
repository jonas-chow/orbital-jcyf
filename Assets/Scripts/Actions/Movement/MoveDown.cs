using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveDown : Action
{
    public MoveDown(CharacterMovement character)
    {
        this.character = character;
        this.charID = character.charID;
        this.name = "MoveDown";
    }

    public override void Execute()
    {
        SendEvent();
        character.Move("down");
    }

    // send a move up event
    public override void SendEvent()
    {
        if (EventHandler.Instance != null) {
            EventHandler.Instance.SendMovementEvent(character.charID, "up", true);
        }
    }
}