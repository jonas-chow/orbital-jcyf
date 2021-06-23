using System;
using UnityEngine;
using UnityEngine.UI;

public class Controls : MonoBehaviour
{
    public Slider bgm;
    public Slider se;

    public void CloseControls()
    {
        AudioManager.Instance.Play("Click");
        gameObject.SetActive(false);
    }

    void OnEnable()
    {
        bgm.value = PlayerPrefs.GetFloat("BGM", 0.1f);
        se.value = PlayerPrefs.GetFloat("SE", 1f);
    }
}