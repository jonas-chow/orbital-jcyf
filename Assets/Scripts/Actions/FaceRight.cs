using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceRight : Action
{
    public FaceRight(CharacterMovement character)
    {
        this.character = character;
    }

    public override void Execute()
    {
        // face right
        character.transform.up = Vector3.right;
        character.faceDirection = "right";
        // hp bar stays on top
        character.hp.transform.up = Vector3.up;
        character.hp.transform.localPosition = new Vector3(-0.55f, 0, 0);
    }
}
