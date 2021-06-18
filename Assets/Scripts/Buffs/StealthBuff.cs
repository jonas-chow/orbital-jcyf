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
            if (character.isEnemy) {
                character.overallSprites.SetActive(false);
            } else {
                character.friendlySprite.SetActive(false);
                character.stealthedSprite.SetActive(true);
            }
        }
    }

    public override void Remove() {
        if (character.isEnemy) {
            character.overallSprites.SetActive(true);
        } else {
            character.stealthedSprite.SetActive(false);
            character.friendlySprite.SetActive(true);
        }
        character.stealthed = false;
    }
}
