using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonDebuff : Buff
{
    private ParticleSystem poisonEffectClone;
    public PoisonDebuff(int turns)
    {
        this.turnsLeft = turns;
    }

    public override void Add(CharacterMovement character) {
        this.character = character;
        // refresh buff duration if character is already invincible
        if (character.poisoned) {
            Buff existingBuff = character.buffs.Find(buff => typeof(PoisonDebuff).IsInstanceOfType(buff));
            existingBuff.turnsLeft = existingBuff.turnsLeft > this.turnsLeft ? existingBuff.turnsLeft : this.turnsLeft;
        } else {
            character.poisoned = true;
            character.buffs.Add(this);
            poisonEffectClone = ParticleSystem.Instantiate(character.poisonEffect, 
                GridManager.Instance.GetCoords(character.GetX(), character.GetY()), Quaternion.identity);
            poisonEffectClone.transform.parent = character.transform;
        } 
    }

    public override void Remove() {
        ParticleSystem.Destroy(this.poisonEffectClone.gameObject);
        character.poisoned = false;
    }

    public override bool TurnPass()
    {
        character.TakeDamage(character.GetDefense(), 10);
        return base.TurnPass();
    }
}
