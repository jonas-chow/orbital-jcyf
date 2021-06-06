using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveRight : Action
{
    public MoveRight(CharacterMovement character)
    {
        this.character = character;
        this.name = "MoveRight";
    }

    public override void Execute()
    {
        if (EventHandler.Instance != null) {
            EventHandler.Instance.SendMovementEvent(getX(), getY(), "right", true);
        }
        character.Move("right");
    }
}
