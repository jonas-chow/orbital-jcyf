using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : Action
{ 
    public MeleeAttack(CharacterMovement character) 
    {
        this.character = character;
    }

    public override void Execute()
    {
        GameObject enemy = null;
        if (character.faceDirection == "up")
        {
            enemy = CharacterMovement.grid.GetObject( (int) character.transform.position.x, 
            (int) character.transform.position.y + 1);
        }
        if (character.faceDirection == "down")
        {
            enemy = CharacterMovement.grid.GetObject( (int) character.transform.position.x, 
            (int) character.transform.position.y - 1);
        }
        if (character.faceDirection == "left")
        {
            enemy = CharacterMovement.grid.GetObject( (int) character.transform.position.x - 1, 
            (int) character.transform.position.y);
        }
        if (character.faceDirection == "right")
        {
            enemy = CharacterMovement.grid.GetObject( (int) character.transform.position.x + 1, 
            (int) character.transform.position.y);
        }
        if (enemy != null)
        {
            enemy.GetComponent<CharacterMovement>().hp.TakeDamage(10);
        }
    }
}
