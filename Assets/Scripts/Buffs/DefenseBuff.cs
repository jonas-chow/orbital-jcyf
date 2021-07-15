using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenseBuff : Buff
{
    private int strength;
    private ParticleSystem buffEffectClone;
    private ParticleSystem debuffEffectClone;

    public DefenseBuff(int strength, int turns)
    {
        this.strength = strength;
        this.turnsLeft = turns;
    }

    public override void Add(CharacterMovement character) {
        this.character = character;
        character.defBuff += strength;
        if (strength > 0) {
            buffEffectClone = ParticleSystem.Instantiate(character.buffEffect, 
                GridManager.Instance.GetCoords(character.GetX(), character.GetY()), Quaternion.identity);
            buffEffectClone.transform.parent = character.transform;
        } else {
            debuffEffectClone = ParticleSystem.Instantiate(character.debuffEffect, 
                GridManager.Instance.GetCoords(character.GetX(), character.GetY()), Quaternion.identity);
            debuffEffectClone.transform.parent = character.transform;
        }
    }

    public override void Remove() {
        if (buffEffectClone != null) {
            ParticleSystem.Destroy(buffEffectClone.gameObject);
        } else {
            ParticleSystem.Destroy(debuffEffectClone.gameObject);
        }
        character.defBuff -= strength;
    }
}
