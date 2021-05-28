using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveUp : Action
{
    public MoveUp(CharacterMovement character)
    {
        this.character = character;
    }

    public override void Execute()
    {
        if (CharacterMovement.grid.MoveObject(getX(), getY(), getX(), getY() + 1)) {
            character.transform.position += Vector3.up;
        }
        // face up
        character.transform.up = Vector3.up;
        character.faceDirection = "up";
        // hp bar stays on top
        character.hp.transform.up = Vector3.up;
        character.hp.transform.localPosition = new Vector3(0, 0.55f, 0);
    }
}
