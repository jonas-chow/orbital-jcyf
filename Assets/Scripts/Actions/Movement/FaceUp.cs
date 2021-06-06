using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceUp : Action
{
    public FaceUp(CharacterMovement character)
    {
        this.character = character;
        this.name = "FaceUp";
    }

    public override void Execute()
    {
        EventHandler.Instance.SendMovementEvent(getX(), getY(), "up", false);
        character.Face("up");
    }
}
