using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveDown : Action
{
    public MoveDown(CharacterMovement character)
    {
        this.character = character;
        this.name = "MoveDown";
    }

    public override void Execute()
    {
        EventHandler.Instance.SendMovementEvent(getX(), getY(), "down", true);
        character.Move("down");
    }
}