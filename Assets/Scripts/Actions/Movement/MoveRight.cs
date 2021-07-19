using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveRight : Action
{
    public MoveRight(CharacterMovement character)
    {
        this.character = character;
        this.charID = character.charID;
        this.name = "MoveRight";
    }

    public override void Execute()
    {
        SendEvent();
        character.Move("right");
    }

    // send a move left event
    public override void SendEvent()
    {
        if (EventHandler.Instance != null) {
            EventHandler.Instance.SendMovementEvent(character.charID, "left", true);
        }
    }
}
