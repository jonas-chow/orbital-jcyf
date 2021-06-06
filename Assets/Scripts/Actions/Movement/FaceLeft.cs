using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceLeft : Action
{
    public FaceLeft(CharacterMovement character)
    {
        this.character = character;
        this.name = "FaceLeft";
    }

    public override void Execute()
    {
        if (EventHandler.Instance != null) {
            EventHandler.Instance.SendMovementEvent(getX(), getY(), "left", false);
        }
        character.Face("left");
    }
}
