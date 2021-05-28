using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceDown : Action
{
    public FaceDown(CharacterMovement character)
    {
        this.character = character;
        this.name = "FaceDown";
    }

    public override void Execute()
    {
        // face up
        character.transform.up = Vector3.down;
        // hp bar stays on top
        character.hp.transform.up = Vector3.up;
        character.hp.transform.localPosition = new Vector3(0, -0.55f, 0);
    }
}
