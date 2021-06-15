using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    Needs: sourceChar
*/
public abstract class MeleeAOEAttack : Attack
{ 
    public List<CharacterMovement> FindTargets()
    {
        // Get all enemy characters in the AOE centred at character
        return GridManager.Instance.GetAllCharactersInAOE(GetX(), GetY());
    }

    public List<CharacterMovement> FindEventTargets()
    {
        // Get all enemy characters in the AOE centred at character
        return GridManager.Instance.GetAllCharactersInAOE(GetX(), GetY());
    }

    // Melee AOE attacks can't be aimed, but will still make you face that direction
    public override void InitialiseAim()
    {
        Attack.SetIndicators(RangeSpawner.Instance.AOEIndicator(sourceChar, 0, 0));
    }
    public override void AimUp() {}
    public override void AimDown() {}
    public override void AimLeft() {}
    public override void AimRight() {}
}
