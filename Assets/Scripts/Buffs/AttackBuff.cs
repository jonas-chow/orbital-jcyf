using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBuff : Buff
{
    private int strength;

    public AttackBuff(int strength, int turns)
    {
        this.strength = strength;
        this.turnsLeft = turns;
    }

    public override void Add(CharacterMovement character) {
        this.character = character;
        character.atkBuff += strength;
    }

    public override void Remove() {
        character.atkBuff -= strength;
    }
}
