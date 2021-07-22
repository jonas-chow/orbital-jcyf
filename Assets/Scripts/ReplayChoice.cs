using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ReplayChoice : MonoBehaviour
{
    public TextMeshProUGUI p1Name;
    public TextMeshProUGUI p2Name;

    public void Init(string p1Name, string p2Name)
    {
        this.p1Name.text = p1Name;
        this.p2Name.text = p2Name;
    }

    public void p1Select()
    {
        AudioManager.Instance.Play("Click");
        GameManager.Instance.reversedPov = false;
        GameManager.Instance.StartReplay();
        gameObject.SetActive(false);
        
    }

    public void p2Select()
    {
        AudioManager.Instance.Play("Click");
        GameManager.Instance.reversedPov = true;
        GameManager.Instance.StartReplay();
        gameObject.SetActive(false);
    }
}
