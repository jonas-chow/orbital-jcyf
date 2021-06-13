using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Attack : Action
{ 
  protected int damage;
  protected int range;
  protected string direction = "none";
  private static GameObject[] rangeIndicators = new GameObject[] {};
  private static GameObject[] rangeLimits = new GameObject[] {};

  public static void ClearIndicators()
  {
    SetIndicators(new GameObject[] {});
  }

  public static void ClearLimits()
  {
    SetLimits(new GameObject[] {});
  }

  public static void SetIndicators(GameObject[] indicators)
  {
    foreach (GameObject indicator in rangeIndicators)
    {
        GameObject.Destroy(indicator);
    }
    rangeIndicators = indicators;
  }

  public static void SetLimits(GameObject[] limits)
  {
    foreach (GameObject limit in rangeLimits)
    {
        GameObject.Destroy(limit);
    }
    rangeLimits = limits;
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


