using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisabledDebuff : Buff
{
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
        }
    }

    public override void Remove() {
        character.disabled = false;
        // this can't happen unless end/start of turn, so no need to set isActive manually
    }
}
