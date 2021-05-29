using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Attack : Action
{ 
  protected int damage;
  protected int range;
  protected string direction = "none";

  public virtual void AimUp() 
  {
    this.direction = "up";
  }

  public virtual void AimDown()
  {
    this.direction = "down";
  }

  public virtual void AimLeft()
  {
    this.direction = "left";
  }

  public virtual void AimRight()
  {
    this.direction = "right";
  }

  protected void ChangeDirection()
  {
    switch (this.direction)
    {
      case "up":
        new FaceUp(character).Execute();
        break;
      case "down":
        new FaceDown(character).Execute();
        break;
      case "left":
        new FaceLeft(character).Execute();
        break;
      case "right":
        new FaceRight(character).Execute();
        break;
    }
  }
}


