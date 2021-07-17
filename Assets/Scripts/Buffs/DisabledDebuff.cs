using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisabledDebuff : Buff
{
    public ParticleSystem disabledEffectClone;

    public DisabledDebuff(int turns)
    {
        this.turnsLeft = turns;
    }

    public override void Add(CharacterMovement character) {
        this.character = character;
        // refresh buff duration if character is already invincible
        if (character.disabled) {
            Buff existingBuff = character.buffs.Find(buff => typeof(DisabledDebuff).IsInstanceOfType(buff));
            existingBuff.turnsLeft = existingBuff.turnsLeft > this.turnsLeft ? existingBuff.turnsLeft : this.turnsLeft;
        } else {
            character.Disable();
            character.buffs.Add(this);
            disabledEffectClone = ParticleSystem.Instantiate(character.disabledEffect, 
                GridManager.Instance.GetCoords(character.GetX(), character.GetY()), Quaternion.identity);
        }
    }

    public override void Remove() {
        ParticleSystem.Destroy(disabledEffectClone.gameObject);
        character.disabled = false;
        // this can't happen unless end/start of turn, so no need to set isActive manually
    }
}
