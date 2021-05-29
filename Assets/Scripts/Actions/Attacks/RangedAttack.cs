using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAttack : Attack
{ 
    public RangedAttack(CharacterMovement character, int damage, int range) 
    {
        this.character = character;
        this.range = range;
        this.damage = damage;
        this.name = "MeleeAttack";
    }

    public override void Execute()
    {
        ChangeDirection();
        if (direction == "none") {
          direction = character.faceDirection;
        }
        CharacterMovement enemy = CharacterMovement.grid
            .GetFirstCharacterInLine(getX(), getY(), range, direction);
        
        if (enemy != null && !enemy.isControllable)
        {
            enemy.TakeDamage(damage);
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
