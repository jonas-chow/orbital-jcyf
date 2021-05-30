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
        this.grid = CharacterMovement.grid;
        this.rangeSpawner = grid.GetComponent<RangeSpawner>();
        this.damage = damage;
        this.range = range;
        this.name = "MeleeAttack";
        this.rangeIndicators = rangeSpawner.AOEIndicator(character, offsetX, offsetY);
        this.rangeLimits = rangeSpawner.RangeLimit(character, range);
    }

    public override void Execute()
    {
        ChangeDirection();
        List<CharacterMovement> enemies = grid
            .GetAllCharactersInAOE(getX() + offsetX, getY() + offsetY)
            .FindAll(cm => !cm.isControllable);

        enemies.ForEach(enemy => enemy.TakeDamage(damage));
    }

    public override void AimUp() 
    {
        this.direction = "up";
        int nextOffset = offsetY + 1;
        if (IsWithinRange(offsetX, nextOffset) && 
            grid.IsValidCoords(getX() + offsetX, getY() + nextOffset)) {
            offsetY = nextOffset;
            ClearIndicators();
            this.rangeIndicators = rangeSpawner.AOEIndicator(character, offsetX, offsetY);
        }
        // change indicator
    }

    public override void AimDown()
    {
        this.direction = "down";
        int nextOffset = offsetY - 1;
        if (IsWithinRange(offsetX, nextOffset) && 
            grid.IsValidCoords(getX() + offsetX, getY() + nextOffset)) {
            offsetY = nextOffset;
            ClearIndicators();
            this.rangeIndicators = rangeSpawner.AOEIndicator(character, offsetX, offsetY);
        }
        // change indicator
    }

    public override void AimLeft()
    {
        this.direction = "left";
        int nextOffset = offsetX - 1;
        if (IsWithinRange(nextOffset, offsetY) && 
            grid.IsValidCoords(getX() + nextOffset, getY() + offsetY)) {
            offsetX = nextOffset;
            ClearIndicators();
            this.rangeIndicators = rangeSpawner.AOEIndicator(character, offsetX, offsetY);
        }
        // change indicator
    }

    public override void AimRight()
    {
        this.direction = "right";
        int nextOffset = offsetX + 1;
        if (IsWithinRange(nextOffset, offsetY) && 
            grid.IsValidCoords(getX() + nextOffset, getY() + offsetY)) {
            offsetX = nextOffset;
            ClearIndicators();
            this.rangeIndicators = rangeSpawner.AOEIndicator(character, offsetX, offsetY);
        }
        // change indicator
    }

    private bool IsWithinRange(int x, int y)
    {
        return Math.Abs(x) + Math.Abs(y) <= this.range;
    }
}
