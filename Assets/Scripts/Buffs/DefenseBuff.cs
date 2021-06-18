using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenseBuff : Buff
{
    private int strength;

    public DefenseBuff(int strength, int turns)
    {
        this.strength = strength;
        this.turnsLeft = turns;
    }

    public override void Add(CharacterMovement character) {
        this.character = character;
        character.defBuff += strength;
    }

    public override void Remove() {
        character.defBuff -= strength;
    }
}
