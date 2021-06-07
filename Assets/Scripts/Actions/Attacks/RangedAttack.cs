using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAttack : Attack
{ 
    public RangedAttack(CharacterMovement character, int damage, int range) 
    {
        this.character = character;
        this.grid = CharacterMovement.grid;
        this.rangeSpawner = grid.GetComponent<RangeSpawner>();
        this.range = range;
        this.damage = damage;
        this.name = "MeleeAttack";
        this.rangeIndicators = rangeSpawner.LinearIndicator(character, range, character.faceDirection);
    }

    public override void Execute()
    {
        if (EventHandler.Instance != null) {
            EventHandler.Instance.SendLinearAttackEvent(getX(), getY(), direction, range, damage);
        }
    
        ChangeDirection();

        // If user did not input a direction, the final direction used will be where the player faces on executing the attack
        if (direction == "none") {
          direction = character.faceDirection;
        }
        CharacterMovement enemy = grid
            .GetFirstCharacterInLine(getX(), getY(), range, direction);
        
        if (enemy != null && !enemy.isControllable)
        {
            enemy.TakeDamage(damage);
        }
    }

    public override void AimUp() 
    {
        if (this.direction != "up") {
            this.direction = "up";
            ClearIndicators();
            this.rangeIndicators = rangeSpawner.LinearIndicator(character, range, "up");
        }
    }

    public override void AimDown()
    {
        if (this.direction != "down") {
            this.direction = "down";
            ClearIndicators();
            this.rangeIndicators = rangeSpawner.LinearIndicator(character, range, "down");
        }
    }

    public override void AimLeft()
    {
        if (this.direction != "left") {
            this.direction = "left";
            ClearIndicators();
            this.rangeIndicators = rangeSpawner.LinearIndicator(character, range, "left");
        }
    }

    public override void AimRight()
    {
        if (this.direction != "right") {
            this.direction = "right";
            ClearIndicators();
            this.rangeIndicators = rangeSpawner.LinearIndicator(character, range, "right");
        }
    }
}
