using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
    Needs: character, range ( =1 for melee )
*/
public abstract class LinearAttack : Attack
{
    protected int globalRange = 15;
    protected string direction = "none";
    private bool global = false;

    public CharacterMovement FindTarget(string direction)
    {
        // If user did not input a direction, the final direction used will be where the player faces on executing the attack
        if (direction == "none") {
            direction = character.faceDirection;
        }
        character.Face(direction);
        return GridManager.Instance
            .GetFirstCharacterInLine(GetX(), GetY(), range, direction);
    }

    public override void InitialiseAim()
    {
        if (range != globalRange) {
            Attack.SetIndicators(RangeSpawner.Instance.LinearIndicator(character, range, 
                character.faceDirection));
        } else {
            global = true;
            if (character.faceDirection == "up") {
                range = 15 - GetY();
                Attack.SetIndicators(RangeSpawner.Instance.LinearIndicator(character, range, 
                    character.faceDirection));
            }
            if (character.faceDirection == "down") {
                range = GetY();
                Attack.SetIndicators(RangeSpawner.Instance.LinearIndicator(character, range, 
                    character.faceDirection));
            }
            if (character.faceDirection == "left") {
                range = GetX();
                Attack.SetIndicators(RangeSpawner.Instance.LinearIndicator(character, range,
                    character.faceDirection));
            }
            if (character.faceDirection == "right") {
                range = 15 - GetX();
                Attack.SetIndicators(RangeSpawner.Instance.LinearIndicator(character, range, 
                    character.faceDirection));
            }
        }   
    }

    public override void AimUp() 
    {
        if (this.direction != "up") {
            this.direction = "up";
            if (global) {
                range = 15 - GetY();
            }
            Attack.SetIndicators(RangeSpawner.Instance.LinearIndicator(character, range, "up"));
        }
    }

    public override void AimDown()
    {
        if (this.direction != "down") {
            this.direction = "down";
            if (global) {
                range = GetY();
            }
            Attack.SetIndicators(RangeSpawner.Instance.LinearIndicator(character, range, "down"));

        }
    }

    public override void AimLeft()
    {
        if (this.direction != "left") {
            this.direction = "left";
            if (global) {
                range = GetX();
            }
            Attack.SetIndicators(RangeSpawner.Instance.LinearIndicator(character, range, "left"));
        }
    }

    public override void AimRight()
    {
        if (this.direction != "right") {
            this.direction = "right";
            if (global) {
                range = 15 - GetX();
            }
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
