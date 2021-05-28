using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceUp : Action
{
    public FaceUp(CharacterMovement character)
    {
        this.character = character;
    }

    public override void Execute()
    {
        // face up
        character.transform.up = Vector3.up;
        character.faceDirection = "up";
        // hp bar stays on top
        character.hp.transform.up = Vector3.up;
        character.hp.transform.localPosition = new Vector3(0, 0.55f, 0);
    }
}
