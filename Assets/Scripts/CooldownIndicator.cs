using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CooldownIndicator : MonoBehaviour
{
    private int cooldown = 0;
    private int counter;
    private string description;
    public TextMeshPro text;
    public SpriteRenderer cdOverlay;
    public SpriteRenderer icon;

    public void Init(Attack attack)
    {
        this.cooldown = attack.GetCooldown();
        this.description = attack.GetDescription();
        this.counter = 0;
        switch (attack.GetAttackType())
        {
            case "attack":
                icon.sprite = CharacterMenu.Instance.attack;
                break;
            case "buff":
                icon.sprite = CharacterMenu.Instance.buff;
                break;
            case "heal":
                icon.sprite = CharacterMenu.Instance.heal;
                break;
            default:
                icon.sprite = CharacterMenu.Instance.other;
                break;
        }
        ResetCD();
    }

    public void TurnPass()
    {
        if (counter > 0) {
            counter--;
            if (counter > 0) {
                text.text = counter.ToString();
            } else {
                ResetCD();
            }
        }
    }

    public void SkillUsed()
    {
        counter = cooldown;
        text.text = counter.ToString();
        cdOverlay.enabled = true;
    }

    public void ResetCD()
    {
        cdOverlay.enabled = false;
        counter = 0;
        text.text = "";
    }

    void OnMouseEnter()
    {
        GameManager.Instance.SetTooltip(description);
    }

    void OnMouseExit()
    {
        GameManager.Instance.SetTooltip("");
    }
}
