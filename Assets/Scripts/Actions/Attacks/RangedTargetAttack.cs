using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RangedTargetAttack : Attack
{
    protected const int globalRange = 30;
    public int offsetX = 0;
    public int offsetY = 0;

    public CharacterMovement FindTarget(int offsetX, int offsetY)
    {
        FaceTargetDirection(offsetX, offsetY);
        return GridManager.Instance.GetCharacter(GetX() + offsetX, GetY() + offsetY);
    }

    public void FaceTargetDirection(int offsetX, int offsetY)
    {
        // Face in the direction you fired
        if (Math.Abs(offsetX) > Math.Abs(offsetY)) {
            // horizontal component larger than vertical
            if (offsetX > 0) {
                character.Face("right");
            } else if (offsetX < 0) {
                character.Face("left");
            }
        } else {
            // vertical component equal or larger to horizontal
            if (offsetY > 0) {
                character.Face("up");
            } else if (offsetY < 0) {
                character.Face("down");
            }
        }
    }

    public object[] InvertOffsets()
    {
        return new object[] {-offsetX, -offsetY};
    }

    public override void InitialiseAim()
    {
        Attack.SetIndicators(RangeSpawner.Instance.RangedTargetIndicator(character, offsetX, offsetY));
        if (range != globalRange) {
            Attack.SetLimits(RangeSpawner.Instance.RangeLimit(character, range));
        }
    }

    public override void AimUp() 
    {
        int nextOffset = offsetY + 1;
        if (IsWithinRange(offsetX, nextOffset) && 
            GridManager.Instance.IsValidCoords(GetX() + offsetX, GetY() + nextOffset)) {
            offsetY = nextOffset;
            Attack.SetIndicators(RangeSpawner.Instance.RangedTargetIndicator(character, offsetX, offsetY));
        }
    }

    public override void AimDown()
    {
        int nextOffset = offsetY - 1;
        if (IsWithinRange(offsetX, nextOffset) && 
            GridManager.Instance.IsValidCoords(GetX() + offsetX, GetY() + nextOffset)) {
            offsetY = nextOffset;
            Attack.SetIndicators(RangeSpawner.Instance.RangedTargetIndicator(character, offsetX, offsetY));
        }
    }

    public override void AimLeft()
    {
        int nextOffset = offsetX - 1;
        if (IsWithinRange(nextOffset, offsetY) && 
            GridManager.Instance.IsValidCoords(GetX() + nextOffset, GetY() + offsetY)) {
            offsetX = nextOffset;
            Attack.SetIndicators(RangeSpawner.Instance.RangedTargetIndicator(character, offsetX, offsetY));
        }
    }

    public override void AimRight()
    {
        int nextOffset = offsetX + 1;
        if (IsWithinRange(nextOffset, offsetY) && 
            GridManager.Instance.IsValidCoords(GetX() + nextOffset, GetY() + offsetY)) {
            offsetX = nextOffset;
            Attack.SetIndicators(RangeSpawner.Instance.RangedTargetIndicator(character, offsetX, offsetY));
        }
    }

    private bool IsWithinRange(int x, int y)
    {
        return Math.Abs(x) + Math.Abs(y) <= this.range;
    }
}
