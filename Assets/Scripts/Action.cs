using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Action
{
    protected CharacterMovement character;
    public string name;
    public int charID;
    
    public abstract void Execute();
    public abstract void SendEvent();

    protected int getX()
    {
        return GridManager.Instance.GetX(character.transform.position.x);
    }

    protected int getY()
    {
        return GridManager.Instance.GetY(character.transform.position.y);
    }

    public CharacterMovement GetCharacter()
    {
        return character;
    }
}
