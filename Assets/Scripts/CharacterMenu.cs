using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMenu : MonoBehaviour
{
    private static CharacterMenu instance;
    public static CharacterMenu Instance { get { return instance; } }

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        } else {
            instance = this;
        }
    }

    public CharacterListing[] characterListings = new CharacterListing[4];
    public GameObject fourthChar;
    public Sprite attack;
    public Sprite buff;
    public Sprite heal;
    public Sprite other;


    public void TurnPass()
    {
        foreach (CharacterListing listing in characterListings) {
            listing.TurnPass();
        }
    }

    public void UseSkill(int charId, int skillId) {
        characterListings[charId].SkillUsed(skillId);
    }

    public void SelectChar(int charId) {
        for (int i = 0; i < 4; i++) {
            characterListings[i].SetSelect(i == charId);
        }
    }

    public void SetHealth(int charId, float healthPercentage) {
        characterListings[charId].SetHealth(healthPercentage);
    }

    public void Init(CharacterMovement[] attacks) {
        for (int i = 0; i < 3; i++) {
            characterListings[i].Init(attacks[i]);
        }
    }

    public void Set4thChar(CharacterMovement char4) {
        fourthChar.SetActive(true);
        characterListings[3].Init(char4);
    }

    public void ResetCD(int charId, int skillId)
    {
        characterListings[charId].ResetCD(skillId);
    }
}
