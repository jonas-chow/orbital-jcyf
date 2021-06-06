using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceDown : Action
{
    public FaceDown(CharacterMovement character)
    {
        this.character = character;
        this.name = "FaceDown";
    }

    public override void Execute()
    {
        EventHandler.Instance.SendMovementEvent(getX(), getY(), "down", false);
        character.Face("down");
    }
}
