using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLeft : Action
{
    public MoveLeft(CharacterMovement character)
    {
        this.character = character;
        this.name = "MoveLeft";
    }

    public override void Execute()
    {
        EventHandler.Instance.SendMovementEvent(getX(), getY(), "left", true);
        character.Move("left");
    }
}