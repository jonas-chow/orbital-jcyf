using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : Action
{ 
    private int damage;

    public MeleeAttack(CharacterMovement character, int damage) 
    {
        this.character = character;
        this.damage = damage;
        this.name = "MeleeAttack";
    }

    public override void Execute()
    {
        GameObject enemy = null;
        if (character.faceDirection == "up")
        {
            enemy = CharacterMovement.grid.GetObject(getX(), getY() + 1);
        }
        if (character.faceDirection == "down")
        {
            enemy = CharacterMovement.grid.GetObject(getX(), getY() - 1);
        }
        if (character.faceDirection == "left")
        {
            enemy = CharacterMovement.grid.GetObject(getX() - 1, getY());
        }
        if (character.faceDirection == "right")
        {
            enemy = CharacterMovement.grid.GetObject(getX() + 1, getY());
        }
        if (enemy != null)
        {
            enemy.GetComponent<CharacterMovement>().TakeDamage(damage);
        }
    }
}
