using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : Attack
{ 
    public MeleeAttack(CharacterMovement character, int damage) 
    {
        this.character = character;
        this.damage = damage;
        this.range = 1;
        this.name = "MeleeAttack";
        // this.indicator = GameObject.Instantiate(Indicator);
    }

    public override void Execute()
    {
        ChangeDirection();

        CharacterMovement enemy = null;
        if (direction == "none") {
            direction = character.faceDirection;
        }
        if (direction == "up")
        {
            enemy = CharacterMovement.grid.GetCharacter(getX(), getY() + 1);
        }
        if (direction == "down")
        {
            enemy = CharacterMovement.grid.GetCharacter(getX(), getY() - 1);
        }
        if (direction == "left")
        {
            enemy = CharacterMovement.grid.GetCharacter(getX() - 1, getY());
        }
        if (direction == "right")
        {
            enemy = CharacterMovement.grid.GetCharacter(getX() + 1, getY());
        }
        if (enemy != null && !enemy.isControllable)
        {
            enemy.GetComponent<CharacterMovement>().TakeDamage(damage);
        }
    }

    public override void AimUp() 
    {
        this.direction = "up";
        // change indicator
    }

    public override void AimDown()
    {
        this.direction = "down";
        // change indicator
    }

    public override void AimLeft()
    {
        this.direction = "left";
        // change indicator
    }

    public override void AimRight()
    {
        this.direction = "right";
        // change indicator
    }
}
