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
        this.rangeIndicators = RangeSpawner.Instance.AOEIndicator(character, 0, 0);
    }

    public override void Execute()
    {
        if (EventHandler.Instance != null) {
            EventHandler.Instance.SendAOEAttackEvent(
                getX(), getY(), getX(), getY(), damage);
        }

        // Get all enemy characters in the AOE centred at character
        List<CharacterMovement> enemies = GridManager.Instance
            .GetAllCharactersInAOE(getX(), getY())
            .FindAll(cm => !cm.isFriendly);
        
        enemies.ForEach(enemy => enemy.TakeDamage(damage));
    }

    // Melee AOE attacks can't be aimed, but will still make you face that direction
}
