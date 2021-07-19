using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Attack : Action
{ 
  protected int cooldown;
  protected int range;
  protected int damage;
  protected string type;

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

  public abstract void InitialiseAim();
  public abstract void AimUp();
  public abstract void AimDown();
  public abstract void AimLeft();
  public abstract void AimRight();

  public abstract void EventExecute(object[] extraData);
  public virtual string GetDescription() {return "";}

  public int GetX()
  {
    return character.GetX();
  }

  public int GetY()
  {
    return character.GetY();
  }

  public string GetAttackType()
  {
    return type;
  }

  public int GetCooldown()
  {
    return cooldown;
  }

  public int GetDamage()
  {
    return damage;
  }
}


