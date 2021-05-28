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
        if (CharacterMovement.grid.MoveObject(getX(), getY(), getX(), getY() - 1)) {
            character.transform.position += Vector3.down;
        }
        // face up
        character.transform.up = Vector3.down;
        // hp bar stays on top
        character.hp.transform.up = Vector3.up;
        character.hp.transform.localPosition = new Vector3(0, -0.55f, 0);
    }
}