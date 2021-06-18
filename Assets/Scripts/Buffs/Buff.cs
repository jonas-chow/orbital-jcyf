using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Buff
{
    // 1 turn = end of your turn / end of enemy turn
    public int turnsLeft;
    public CharacterMovement character;

    public abstract void Add(CharacterMovement character);

    public abstract void Remove();

    public virtual bool TurnPass()
    {
        this.turnsLeft--;
        if (turnsLeft <= 0)
        {
            Remove();
        } 
        return turnsLeft <= 0;
    }
}
