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
        if (CharacterMovement.grid.MoveObject(getX(), getY(), getX() + 1, getY())) {
            character.transform.position += Vector3.right;
        }
        // face right
        character.transform.up = Vector3.right;
        character.faceDirection = "right";
        // hp bar stays on top
        character.hp.transform.up = Vector3.up;
        character.hp.transform.localPosition = new Vector3(-0.55f, 0, 0);
    }
}
