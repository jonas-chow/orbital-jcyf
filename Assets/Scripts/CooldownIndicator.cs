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
    public SpriteRenderer sprite;

    public void Init(Attack attack)
    {
        this.cooldown = attack.cooldown;
        this.description = attack.GetDescription();
        ResetCD();
        // gameObject.SetActive(false);
    }

    public void TurnPass()
    {
        if (counter > 0) {
            counter--;
            if (counter > 0) {
                text.text = counter.ToString();
            } else {
                ResetCD();
                // gameObject.SetActive(false);
            }
        }
    }

    public void SkillUsed()
    {
        counter = cooldown;
        text.text = counter.ToString();
        sprite.enabled = true;
    }

    public void ResetCD()
    {
        sprite.enabled = false;
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
