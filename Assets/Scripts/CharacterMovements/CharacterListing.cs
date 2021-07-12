using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterListing : MonoBehaviour
{
    public CooldownIndicator[] cooldowns = new CooldownIndicator[3];
    public GameObject greenBar;
    public SpriteRenderer icon;
    public GameObject selectionIndicator;
    public GameObject deadIndicator;
    private Vector3 initialHealthPosition = new Vector3(2.25f, -1, 0);
    private Vector3 initialHealthScale = new Vector3(5.5f, 0.4f, 1);

    public void Init(CharacterMovement character)
    {
        icon.sprite = character.friendlySprites[1];
        cooldowns[0].Init(character.attack1);
        cooldowns[1].Init(character.attack2);
        cooldowns[2].Init(character.attack3);
        greenBar.transform.localScale = initialHealthScale;
        greenBar.transform.localPosition = initialHealthPosition;
        deadIndicator.SetActive(false);
    }

    public void SetSelect(bool selection)
    {
        selectionIndicator.SetActive(selection);
    }

    public void SetHealth(float hpPercentage)
    {
        if (hpPercentage <= 0) {
            greenBar.transform.localScale = Vector3.zero;
            deadIndicator.SetActive(true);
        } else {
            greenBar.transform.localScale = new Vector3(
                initialHealthScale.x * hpPercentage, initialHealthScale.y, 0);
            // weird formula that works
            greenBar.transform.localPosition = new Vector3(
                initialHealthScale.x * hpPercentage / 2 - 0.5f, initialHealthPosition.y, 0);
        }
    }

    public void ResetCD(int skillId)
    {
        cooldowns[skillId].ResetCD();
    }

    public void TurnPass()
    {
        foreach(CooldownIndicator cooldown in cooldowns)
        {
            cooldown.TurnPass();
        }
    }

    public void SkillUsed(int skillId)
    {
        cooldowns[skillId].SkillUsed();
    }
}