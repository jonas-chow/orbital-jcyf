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
        if (CharacterMovement.grid.MoveObject(getX(), getY(), getX() - 1, getY())) {
            character.transform.position += Vector3.left;
        }
        // face right
        character.transform.up = Vector3.left;
        // hp bar stays on top
        character.hp.transform.up = Vector3.up;
        character.hp.transform.localPosition = new Vector3(0.55f, 0, 0);
    }
}