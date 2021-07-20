using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteManager : MonoBehaviour
{
    private static SpriteManager instance;
    public static SpriteManager Instance { get { return instance; } }
    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        } else {
            instance = this;
        }
    }

    [Header("Friendlies")]
    public Sprite friendlyAssassin;
    public Sprite friendlyBruiser;
    public Sprite friendlyHealer;
    public Sprite friendlyHunter;
    public Sprite friendlyScout;
    public Sprite friendlySummoner;
    public Sprite friendlyTank;
    public Sprite friendlyTrapper;
    public Sprite friendlyWizard;

    [Header("Enemies")]
    public Sprite enemyAssassin;
    public Sprite enemyBruiser;
    public Sprite enemyHealer;
    public Sprite enemyHunter;
    public Sprite enemyScout;
    public Sprite enemySummoner;
    public Sprite enemyTank;
    public Sprite enemyTrapper;
    public Sprite enemyWizard;

    public Sprite GetSprite(string name, bool friendly)
    {
        if (friendly)
        {
            switch (name)
            {
                case "Assassin":
                    return friendlyAssassin;
                case "Bruiser":
                    return friendlyBruiser;
                case "Healer":
                    return friendlyHealer;
                case "Hunter":
                    return friendlyHunter;
                case "Scout":
                    return friendlyScout;
                case "Summoner":
                    return friendlySummoner;
                case "Tank":
                    return friendlyTank;
                case "Trapper":
                    return friendlyTrapper;
                case "Wizard":
                    return friendlyWizard;
                default:
                    return null;
            }
        } else 
        {
            switch (name)
            {
                case "Assassin":
                    return enemyAssassin;
                case "Bruiser":
                    return enemyBruiser;
                case "Healer":
                    return enemyHealer;
                case "Hunter":
                    return enemyHunter;
                case "Scout":
                    return enemyScout;
                case "Summoner":
                    return enemySummoner;
                case "Tank":
                    return enemyTank;
                case "Trapper":
                    return enemyTrapper;
                case "Wizard":
                    return enemyWizard;
                default:
                    return null;
            }
        }
    }
}
