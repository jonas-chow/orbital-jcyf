using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceUp : Action
{
    public FaceUp(CharacterMovement character)
    {
        this.character = character;
        this.charID = character.charID;
        this.name = "FaceUp";
    }

    public override void Execute()
    {
        SendEvent();
        character.Face("up");
    }

    // send a face down event
    public override void SendEvent()
    {
        if (EventHandler.Instance != null) {
            EventHandler.Instance.SendMovementEvent(character.charID, "down", false);
        }
    }
}
