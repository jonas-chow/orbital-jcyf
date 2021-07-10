using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WardMovement : CharacterMovement
{
    public override void TakeDamage(float enemyAtk, int damage)
    {
        GridManager.Instance.RemoveObject(GetX(), GetY());
        GameObject.Destroy(gameObject);
    }

    public void init(bool isEnemy)
    {
        this.isEnemy = isEnemy;
        spriteRenderer.sprite = isEnemy ? enemySprites[0] : friendlySprites[0];
        fog.SetActive(!isEnemy);
    }

    public override void SetupAttack(int attackNum) {}
}
