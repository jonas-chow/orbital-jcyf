using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMenu : MonoBehaviour
{
    private static CharacterMenu instance;
    public static CharacterMenu Instance { get { return instance; } }
    private Vector3 initialHealthPosition = new Vector3(2.25f, -1, 0);
    private Vector3 initialHealthScale = new Vector3(5.5f, 0.4f, 1);

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        } else {
            instance = this;
        }
    }

    public CooldownIndicator[] cooldowns = new CooldownIndicator[12];
    
    public GameObject[] greenHps = new GameObject[4];
    public GameObject[] selectionIndicators = new GameObject[4];
    public GameObject[] deadIndicators = new GameObject[4];
    public GameObject fourthChar;



    public void TurnPass()
    {
        foreach (CooldownIndicator cd in cooldowns) {
            cd.TurnPass();
        }
    }

    public void UseSkill(int charId, int skillId) {
        cooldowns[charId * 3 + skillId].SkillUsed();
    }

    public void SelectChar(int charId) {
        for (int i = 0; i < 4; i++) {
            selectionIndicators[i].SetActive(i == charId);
        }
    }

    public void SetHealth(int charId, float healthPercentage) {
        if (healthPercentage <= 0) {
            greenHps[charId].transform.localScale = Vector3.zero;
            deadIndicators[charId].SetActive(true);
        } else {
            greenHps[charId].transform.localScale = new Vector3(
                initialHealthScale.x * healthPercentage, initialHealthScale.y, 0);
            // weird formula that works
            greenHps[charId].transform.localPosition = new Vector3(
                initialHealthScale.x * healthPercentage / 2 - 0.5f, initialHealthPosition.y, 0);
        }
    }

    public void Init(Attack[] attacks) {
        for (int i = 0; i < 9; i++) {
            cooldowns[i].Init(attacks[i]);
        }
    }

    public void Set4thChar(Attack[] attacks) {
        fourthChar.SetActive(true);
        SetHealth(3, 1f);
        deadIndicators[3].SetActive(false);
        for (int i = 0; i < 3; i++) {
            cooldowns[i + 9].Init(attacks[i]);
        }
    }

    public void ResetCD(int charID, int skillID)
    {
        cooldowns[charID * 3 + skillID - 1].ResetCD();
    }
}
