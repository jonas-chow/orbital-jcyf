using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RangedSelect : MonoBehaviour
{
    public Sprite hunterSprite;
    public Sprite scoutSprite;
    public Sprite trapperSprite;

    public Image icon;

    public void ChangeIcon(Rangeds id)
    {
        switch (id)
        {
            case Rangeds.Hunter:
                icon.sprite = hunterSprite;
                break;
            case Rangeds.Scout:
                icon.sprite = scoutSprite;
                break;
            case Rangeds.Trapper:
                icon.sprite = trapperSprite;
                break;
        }
    }
}
