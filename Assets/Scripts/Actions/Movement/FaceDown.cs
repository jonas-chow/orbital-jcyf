using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceDown : Action
{
    public FaceDown(CharacterMovement character)
    {
        this.character = character;
        this.charID = character.charID;
        this.name = "FaceDown";
    }

    public override void Execute()
    {
        SendEvent();
        character.Face("down");
    }

    // send a face up event
    public override void SendEvent()
    {
        if (EventHandler.Instance != null) {
            EventHandler.Instance.SendMovementEvent(character.charID, "up", false);
        }
    }
}
