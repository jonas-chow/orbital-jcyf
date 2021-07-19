using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceLeft : Action
{
    public FaceLeft(CharacterMovement character)
    {
        this.character = character;
        this.charID = character.charID;
        this.name = "FaceLeft";
    }

    public override void Execute()
    {
        SendEvent();
        character.Face("left");
    }

    // send a face right event
    public override void SendEvent()
    {
        if (EventHandler.Instance != null) {
            EventHandler.Instance.SendMovementEvent(character.charID, "right", false);
        }
    }
}
