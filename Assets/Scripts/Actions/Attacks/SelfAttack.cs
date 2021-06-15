using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    Needs: sourceChar
*/
public abstract class SelfAttack : Attack
{ 
    public override void InitialiseAim()
    {
        Attack.SetIndicators(
          RangeSpawner.Instance.CharacterIndicator(new CharacterMovement[] {sourceChar}));
    }

    public override void AimUp() {}
    public override void AimDown() {}
    public override void AimLeft() {}
    public override void AimRight() {}
}
