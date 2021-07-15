using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvincibleBuff : Buff
{
    private ParticleSystem invincibleEffectClone;
    public InvincibleBuff(int turns)
    {
        this.turnsLeft = turns;
    }

    public override void Add(CharacterMovement character) {
        this.character = character;
        // refresh buff duration if character is already invincible
        if (character.invincible) {
            Buff existingBuff = character.buffs.Find(buff => typeof(InvincibleBuff).IsInstanceOfType(buff));
            existingBuff.turnsLeft = existingBuff.turnsLeft > this.turnsLeft ? existingBuff.turnsLeft : this.turnsLeft;
        } else {
            character.invincible = true;
            character.buffs.Add(this);
            invincibleEffectClone = ParticleSystem.Instantiate(character.invincibleEffect, 
                GridManager.Instance.GetCoords(character.GetX(), character.GetY()), Quaternion.identity);
            invincibleEffectClone.transform.parent = character.transform;
        }
    }

    public override void Remove() {
        ParticleSystem.Destroy(invincibleEffectClone.gameObject);
        character.invincible = false;
    }
}
