using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : Attack
{ 
    public MeleeAttack(CharacterMovement character, int damage) 
    {
        this.character = character;
        this.rangeSpawner = GridManager.Instance.GetComponent<RangeSpawner>();
        this.damage = damage;
        this.range = 1;
        this.name = "MeleeAttack";
        this.rangeIndicators = rangeSpawner.LinearIndicator(character, range, character.faceDirection);
    }

    public override void Execute()
    {
        if (EventHandler.Instance != null) {
            EventHandler.Instance.SendLinearAttackEvent(getX(), getY(), direction, range, damage);
        }

        ChangeDirection();

        CharacterMovement enemy = null;
        // If user did not input a direction, the final direction used will be where the player faces on executing the attack
        if (direction == "none") {
            direction = character.faceDirection;
        }

        if (direction == "up")
        {
            enemy = GridManager.Instance.GetCharacter(getX(), getY() + 1);
        }
        if (direction == "down")
        {
            enemy = GridManager.Instance.GetCharacter(getX(), getY() - 1);
        }
        if (direction == "left")
        {
            enemy = GridManager.Instance.GetCharacter(getX() - 1, getY());
        }
        if (direction == "right")
        {
            enemy = GridManager.Instance.GetCharacter(getX() + 1, getY());
        }
        if (enemy != null && !enemy.isFriendly)
        {
            enemy.GetComponent<CharacterMovement>().TakeDamage(damage);
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
