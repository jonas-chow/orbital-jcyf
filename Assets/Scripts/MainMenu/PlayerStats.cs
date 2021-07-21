using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class PlayerStats : MonoBehaviour
{
    public TMP_Text winPercentage;

    public void Back()
    {
        AudioManager.Instance.Play("Click");
        gameObject.SetActive(false);
    }

    void Start()
    {
        winPercentage.text = PlayerPrefs.GetFloat("winPercent").ToString();
    }
}
