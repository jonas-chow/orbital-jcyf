using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisibleDebuff : Buff
{
    public VisibleDebuff(int turns)
    {
        this.turnsLeft = turns;
    }

    public override void Add(CharacterMovement character) {
        this.character = character;
        if (character.isEnemy) {
            // refresh buff duration if character is already invincible
            if (character.fog.activeSelf) {
                Buff existingBuff = character.buffs.Find(buff => typeof(VisibleDebuff).IsInstanceOfType(buff));
                existingBuff.turnsLeft = existingBuff.turnsLeft > this.turnsLeft ? existingBuff.turnsLeft : this.turnsLeft;
            } else {
                character.fog.SetActive(true);
                character.buffs.Add(this);
            }
        }
    }

    public override void Remove() {
        if (character.isEnemy)
        {
            character.fog.SetActive(false);
        }
    }
}
