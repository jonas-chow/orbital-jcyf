using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Attack : Action
{ 
  protected int damage;
  protected int range;
  protected string direction = "none";
  protected GameObject[] rangeIndicators = new GameObject[0] {};
  protected GameObject[] rangeLimits = new GameObject[0] {};

  public void ClearIndicators()
  {
    foreach (GameObject indicator in this.rangeIndicators)
    {
        GameObject.Destroy(indicator);
    }
    this.rangeIndicators = new GameObject[0] {};
  }

  public void ClearLimits()
  {
    foreach (GameObject indicator in this.rangeLimits)
    {
        GameObject.Destroy(indicator);
    }
    this.rangeLimits = new GameObject[0] {};
  }

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
    if (this.direction != "none") {
      character.Face(direction);
    }
  }
}


