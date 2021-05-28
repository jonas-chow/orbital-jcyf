using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Action
{
    protected CharacterMovement character;
    public string name;
    
    public abstract void Execute();

    protected int getX()
    {
        return (int) character.transform.localPosition.x;
    }

    protected int getY()
    {
        return (int) character.transform.localPosition.y;
    }
}
