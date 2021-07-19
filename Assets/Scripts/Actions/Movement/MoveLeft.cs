using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLeft : Action
{
    public MoveLeft(CharacterMovement character)
    {
        this.character = character;
        this.charID = character.charID;
        this.name = "MoveLeft";
    }

    public override void Execute()
    {
        SendEvent();
        character.Move("left");
    }

    // send a move right event
    public override void SendEvent()
    {
        if (EventHandler.Instance != null) {
            EventHandler.Instance.SendMovementEvent(character.charID, "right", true);
        }
    }
}