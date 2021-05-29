using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAOEAttack : Attack
{ 
    public MeleeAOEAttack(CharacterMovement character, int damage) 
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

        // Get all enemy characters in the AOE centred at character
        List<CharacterMovement> enemies = CharacterMovement.grid
            .GetAllCharactersInAOE(getX(), getY())
            .FindAll(cm => !cm.isControllable);
        
        enemies.ForEach(enemy => enemy.TakeDamage(damage));
    }

    // Melee AOE attacks can't be aimed, but will still make you face that direction
}
