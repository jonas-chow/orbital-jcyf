using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StealthBuff : Buff
{
    public StealthBuff(int turns)
    {
        this.turnsLeft = turns;
    }

    public override void Add(CharacterMovement character) {
        this.character = character;
        // refresh buff duration if character is already invincible
        if (character.stealthed) {
            Buff existingBuff = character.buffs.Find(buff => typeof(StealthBuff).IsInstanceOfType(buff));
            existingBuff.turnsLeft = existingBuff.turnsLeft > this.turnsLeft ? existingBuff.turnsLeft : this.turnsLeft;
        } else {
            character.stealthed = true;
            character.spriteRenderer.color = new Color(1, 1, 1, character.isEnemy ? 0 : 0.5f);
        }
    }

    public override void Remove() {
        character.stealthed = false;
        character.spriteRenderer.color = Color.white;
    }
}
