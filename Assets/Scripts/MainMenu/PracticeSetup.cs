using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class PracticeSetup : MonoBehaviour
{
    public GameObject loadout;
    public TMP_InputField hp;
    public TMP_InputField def;
    public TMP_InputField x;
    public TMP_InputField y;
    public Toggle movement;
    public GameObject hpTooltip;
    public GameObject defTooltip;
    public GameObject xTooltip;
    public GameObject yTooltip;
    public GameObject movementTooltip;

    public void Back()
    {
        AudioManager.Instance.Play("Click");
        gameObject.SetActive(false);
    }

    public void StartGame()
    {
        AudioManager.Instance.Play("Click");
        SceneManager.LoadScene(1);
    }

    void Start()
    {
        hp.text = PlayerPrefs.GetInt("hp", 100).ToString();
        def.text = PlayerPrefs.GetInt("def", 15).ToString();
        x.text = PlayerPrefs.GetInt("X", 15).ToString();
        y.text = PlayerPrefs.GetInt("Y", 15).ToString();
        movement.isOn = PlayerPrefs.GetInt("Movement", 0) == 1;
    }

    public void Loadout()
    {
        AudioManager.Instance.Play("Click");
        loadout.SetActive(true);
    }

    public void ChangeHP(string input)
    {
        if (input == "") {
            PlayerPrefs.SetInt("hp", 100);
        } else {
            int hp = int.Parse(input);
            PlayerPrefs.SetInt("hp", hp);
        }
    }

    public void ChangeDef(string input)
    {
        if (input == "") {
            PlayerPrefs.SetInt("def", 15);
        } else {
            int def = int.Parse(input);
            PlayerPrefs.SetInt("def", def);
        }
    }

    public void ChangeX(string input)
    {
        if (input == "") {
            PlayerPrefs.SetInt("X", 15);
        } else {
            int x = int.Parse(input);
            PlayerPrefs.SetInt("X", x);
        }
    }

    public void ChangeY(string input)
    {
        if (input == "") {
            PlayerPrefs.SetInt("Y", 15);
        } else {
            int y = int.Parse(input);
            PlayerPrefs.SetInt("Y", y);
        }
    }

    public void ToggleMovement(bool input)
    {
        PlayerPrefs.SetInt("Movement", input ? 1 : 0);
    }

    public void HpEnter()
    {
        hpTooltip.SetActive(true);
    }

    public void HpExit()
    {
        hpTooltip.SetActive(false);
    }

    public void DefEnter()
    {
        defTooltip.SetActive(true);
    }

    public void DefExit()
    {
        defTooltip.SetActive(false);
    }

    public void XEnter()
    {
        xTooltip.SetActive(true);
    }

    public void XExit()
    {
        xTooltip.SetActive(false);
    }

    public void YEnter()
    {
        yTooltip.SetActive(true);
    }

    public void YExit()
    {
        yTooltip.SetActive(false);
    }

    public void MovementEnter()
    {
        movementTooltip.SetActive(true);
    }

    public void MovementExit()
    {
        movementTooltip.SetActive(false);
    }
}
