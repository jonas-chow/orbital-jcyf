using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageText : MonoBehaviour
{
    public TextMeshPro text;

    void OnEnable() 
    {
        StartCoroutine(DisappearSoon());
    }

    private IEnumerator DisappearSoon()
    {
        yield return new WaitForSeconds(0.3f);
        GameObject.Destroy(this.gameObject);
    }

    public void SetText(string str)
    {
        text.text = str;
    }
}
