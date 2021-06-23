using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public Slider bgm;
    public Slider se;
    
    void OnEnable()
    {
        bgm.value = PlayerPrefs.GetFloat("BGM", 0.1f);
        se.value = PlayerPrefs.GetFloat("SE", 1f);
    }
}
