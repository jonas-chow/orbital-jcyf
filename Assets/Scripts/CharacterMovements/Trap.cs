using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
TODO:
1) Change what each attack inherits from
1a) Add to the constructors/add more fields if needed
2) Change damage/range/cooldown as you want in the constructors
2) Implement execute, sendEvent, eventExecute
*/

public class Trap : MonoBehaviour
{
    private int damage;
    TrapperMovement trapper;

    public void Init(TrapperMovement trapper, int damage)
    {
        this.trapper = trapper;
        this.damage = damage;
        // GridManager.Instance.InsertTrap(this);
    }

    public void Trigger(CharacterMovement character)
    {
        if (character.IsEnemyOf(trapper)) {
            character.TakeDamage(trapper.GetAttack(), damage);
            character.AddBuff(new DisabledDebuff(2));
            character.AddBuff(new VisibleDebuff(2));
        }
    }

}
