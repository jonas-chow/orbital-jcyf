using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PauseMenu : MonoBehaviour
{
    public Slider bgm;
    public Slider se;
    public TextMeshProUGUI quitText;

    void Start()
    {
        if (GameManager.Instance.gameMode == 2)
        {
            quitText.text = "Quit";
        } else {
            quitText.text = "Concede";
        }
    }
    
    void OnEnable()
    {
        bgm.value = PlayerPrefs.GetFloat("BGM", 0.1f);
        se.value = PlayerPrefs.GetFloat("SE", 1f);
    }
}
