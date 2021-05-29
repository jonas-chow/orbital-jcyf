using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAOEAttack : Attack
{ 
    private int offsetX = 0;
    private int offsetY = 0;

    public RangedAOEAttack(CharacterMovement character, int damage, int range) 
    {
        this.character = character;
        this.damage = damage;
        this.range = range;
        this.name = "MeleeAttack";
        // generate Range Indicator
    }

    public override void Execute()
    {
        ChangeDirection();
        List<CharacterMovement> enemies = CharacterMovement.grid
            .GetAllCharactersInAOE(getX() + offsetX, getY() + offsetY)
            .FindAll(cm => !cm.isControllable);

        enemies.ForEach(enemy => enemy.TakeDamage(damage));

        // Destory range indicators
    }

    public override void AimUp() 
    {
        this.direction = "up";
        if (IsWithinRange(offsetX, offsetY + 1)) {
            offsetY++;
        }
        // change indicator
    }

    public override void AimDown()
    {
        this.direction = "down";
        if (IsWithinRange(offsetX, offsetY - 1)) {
            offsetY--;
        }
        // change indicator
    }

    public override void AimLeft()
    {
        this.direction = "left";
        if (IsWithinRange(offsetX - 1, offsetY)) {
            offsetX--;
        }
        // change indicator
    }

    public override void AimRight()
    {
        this.direction = "right";
        if (IsWithinRange(offsetX + 1, offsetY)) {
            offsetX++;
        }
        // change indicator
    }

    private bool IsWithinRange(int x, int y)
    {
        return Math.Abs(x) + Math.Abs(y) <= this.range;
    }
}
