using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAOEAttack : Attack
{ 
    public MeleeAOEAttack(CharacterMovement character, int damage) 
    {
        this.character = character;
        this.grid = CharacterMovement.grid;
        this.rangeSpawner = grid.GetComponent<RangeSpawner>();
        this.damage = damage;
        this.range = 1;
        this.name = "MeleeAttack";
        this.rangeIndicators = rangeSpawner.AOEIndicator(character, 0, 0);
    }

    public override void Execute()
    {
        ChangeDirection();

        // Get all enemy characters in the AOE centred at character
        List<CharacterMovement> enemies = grid
            .GetAllCharactersInAOE(getX(), getY())
            .FindAll(cm => !cm.isControllable);
        
        enemies.ForEach(enemy => enemy.TakeDamage(damage));
    }

    // Melee AOE attacks can't be aimed, but will still make you face that direction
}
