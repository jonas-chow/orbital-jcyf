using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CooldownIndicator : MonoBehaviour
{
    private int cooldown = 0;
    private int counter;
    public TextMeshPro text;

    public void init(int cooldown)
    {
        this.cooldown = cooldown;
        gameObject.SetActive(false);
    }

    void OnEnable()
    {
        counter = cooldown;
        text.text = counter.ToString();
    }

    public void TurnPass()
    {
        if (counter > 0) {
            counter--;
            if (counter > 0) {
                text.text = counter.ToString();
            } else {
                gameObject.SetActive(false);
            }
        }
    }
}
