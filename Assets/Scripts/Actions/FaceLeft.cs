using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceLeft : Action
{
    public FaceLeft(CharacterMovement character)
    {
        this.character = character;
    }

    public override void Execute()
    {
        // face right
        character.transform.up = Vector3.left;
        // hp bar stays on top
        character.hp.transform.up = Vector3.up;
        character.hp.transform.localPosition = new Vector3(0.55f, 0, 0);
    }
}
