using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MeleeSelect : MonoBehaviour
{   
    public Sprite assassinSprite;
    public Sprite bruiserSprite;
    public Sprite tankSprite;
    public Image icon;

    public void ChangeIcon(Melees id)
    {
        switch (id)
        {
            case Melees.Assassin:
                icon.sprite = assassinSprite;
                break;
            case Melees.Bruiser:
                icon.sprite = bruiserSprite;
                break;
            case Melees.Tank:
                icon.sprite = tankSprite;
                break;
        }
    }
}
