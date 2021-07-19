using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveUp : Action
{
    public MoveUp(CharacterMovement character)
    {
        this.character = character;
        this.charID = character.charID;
        this.name = "MoveUp";
    }

    public override void Execute()
    {
        SendEvent();
        character.Move("up");
    }

    // send a move down event
    public override void SendEvent()
    {
        if (EventHandler.Instance != null) {
            EventHandler.Instance.SendMovementEvent(character.charID, "down", true);
        }
    }
}
