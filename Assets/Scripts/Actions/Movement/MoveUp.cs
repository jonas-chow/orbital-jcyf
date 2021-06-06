using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveUp : Action
{
    public MoveUp(CharacterMovement character)
    {
        this.character = character;
        this.name = "MoveUp";
    }

    public override void Execute()
    {
        EventHandler.Instance.SendMovementEvent(getX(), getY(), "up", true);
        character.Move("up");
    }
}
