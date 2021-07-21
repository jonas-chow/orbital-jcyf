using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class PlayerStats : MonoBehaviour
{
    public TMP_Text wins;
    public TMP_Text losses;
    public TMP_Text winPercentage;

    public void Back()
    {
        AudioManager.Instance.Play("Click");
        gameObject.SetActive(false);
    }

    public void Reset()
    {
        AudioManager.Instance.Play("Click");
        PlayerPrefs.DeleteKey("winCount");
        PlayerPrefs.DeleteKey("loseCount");
        PlayerPrefs.DeleteKey("winPercent");
    }

    void Start()
    {
        wins.text = PlayerPrefs.GetFloat("winCount").ToString();
        losses.text = PlayerPrefs.GetFloat("loseCount").ToString();
        winPercentage.text = PlayerPrefs.GetFloat("winPercent").ToString("#0.0");
    }
}
