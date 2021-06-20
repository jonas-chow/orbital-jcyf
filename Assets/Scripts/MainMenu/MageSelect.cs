using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MageSelect : MonoBehaviour
{
    public Sprite wizardSprite;
    public Sprite summonerSprite;
    public Sprite healerSprite;

    public Image icon;

    public void ChangeIcon(Mages id)
    {
        switch (id)
        {
            case Mages.Wizard:
                icon.sprite = wizardSprite;
                break;
            case Mages.Summoner:
                icon.sprite = summonerSprite;
                break;
            case Mages.Healer:
                icon.sprite = healerSprite;
                break;
        }
    }
}
