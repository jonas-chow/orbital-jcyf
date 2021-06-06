using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UsernameInput : MonoBehaviour
{
    public TMP_InputField text;

    void Start()
    {
        text.text = PlayerPrefs.GetString("Username", "");
    }

    public void ChangeUsername()
    {
        PlayerPrefs.SetString("Username", text.text);
    }
}
