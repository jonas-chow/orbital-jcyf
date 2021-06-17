using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
    Needs: character, range ( =1 for melee )
*/
public abstract class LinearAttack : Attack
{ 
    protected string direction = "none";

    public CharacterMovement FindTarget()
    {
        // If user did not input a direction, the final direction used will be where the player faces on executing the attack
        if (direction == "none") {
            direction = character.faceDirection;
        }

        return GridManager.Instance
            .GetFirstCharacterInLine(GetX(), GetY(), range, direction);
    }

    public CharacterMovement FindEventTarget(string direction)
    {
        // If user did not input a direction, the final direction used will be where the player faces on executing the attack
        if (direction == "none") {
            direction = character.faceDirection;
        }

        return GridManager.Instance
            .GetFirstCharacterInLine(GetX(), GetY(), range, direction);
    }

    public override void InitialiseAim()
    {
        Attack.SetIndicators(RangeSpawner.Instance.LinearIndicator(character, range, character.faceDirection));
    }

    public override void AimUp() 
    {
        if (this.direction != "up") {
            this.direction = "up";
            Attack.SetIndicators(RangeSpawner.Instance.LinearIndicator(character, range, "up"));
        }
    }

    public override void AimDown()
    {
        if (this.direction != "down") {
            this.direction = "down";
            Attack.SetIndicators(RangeSpawner.Instance.LinearIndicator(character, range, "down"));

        }
    }

    public override void AimLeft()
    {
        if (this.direction != "left") {
            this.direction = "left";
            Attack.SetIndicators(RangeSpawner.Instance.LinearIndicator(character, range, "left"));
        }
    }

    public override void AimRight()
    {
        if (this.direction != "right") {
            this.direction = "right";
            Attack.SetIndicators(RangeSpawner.Instance.LinearIndicator(character, range, "right"));
        }
    }

    public string InvertDirection(string dir)
    {
        switch (dir)
        {
            case "up":
                return "down";
            case "down":
                return "up";
            case "left":
                return "right";
            case "right":
                return "left";
            default:
                return "none";
        }
    }
}
