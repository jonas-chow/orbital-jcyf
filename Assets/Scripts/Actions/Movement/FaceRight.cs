using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceRight : Action
{
    public FaceRight(CharacterMovement character)
    {
        this.character = character;
        this.name = "FaceRight";
    }

    public override void Execute()
    {
        EventHandler.Instance.SendMovementEvent(getX(), getY(), "right", false);
        character.Face("right");
    }
}
