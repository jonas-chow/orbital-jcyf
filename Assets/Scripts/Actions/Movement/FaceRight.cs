using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceRight : Action
{
    public FaceRight(CharacterMovement character)
    {
        this.character = character;
        this.charID = character.charID;
        this.name = "FaceRight";
    }

    public override void Execute()
    {
        SendEvent();
        character.Face("right");
    }

    // send a face left event
    public override void SendEvent()
    {
        if (EventHandler.Instance != null) {
            EventHandler.Instance.SendMovementEvent(character.charID, "left", false);
        }
    }
}
