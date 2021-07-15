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

public class TargetDummy : CharacterMovement
{
    public void Init(int hp, int def)
    {
        this.hp.maxHp = hp > 0 ? hp : 100;

        this.def = def;
    }

    void Awake()
    {
        Init(PlayerPrefs.GetInt("hp", 100), PlayerPrefs.GetInt("def", 15));
    }

    public override void SetupAttack(int attackNumber) {}
}
