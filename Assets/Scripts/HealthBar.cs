using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    private bool visible = false;
    private int hp;
    private float tempTime = 1.5f;

    [SerializeField]
    private int maxHp = 100;

    [SerializeField]
    private GameObject greenBar, redBar;

    // Start is called before the first frame update
    void Start()
    {
        hp = maxHp;
        greenBar.GetComponent<SpriteRenderer>().enabled = false;
        redBar.GetComponent<SpriteRenderer>().enabled = false;
    }

    private Vector3 NewPosition(float scale)
    {
        return new Vector3(scale / 2 - 0.5f, 0, 0);
    }

    public void SetVisible(bool visibility)
    {
        visible = visibility;
        greenBar.GetComponent<SpriteRenderer>().enabled = visibility;
        redBar.GetComponent<SpriteRenderer>().enabled = visibility;
    }

    // returns whether the character dies
    public bool TakeDamage(int damage, int charID)
    {
        hp -= damage;
        if (hp <= 0) {
            hp = 0;
        } else if (hp > maxHp) {
            hp = maxHp;
        }
        float scale = (float)hp / maxHp;
        greenBar.transform.localPosition = NewPosition(scale);
        greenBar.transform.localScale = new Vector3(scale, 1, 1);

        // if char is not an enemy (very bad practice)
        if (charID != -1) {
            CharacterMenu.Instance.SetHealth(charID, scale);
        }

        // refreshes the visible time if already visible
        StopCoroutine("TemporaryVisible");
        StartCoroutine("TemporaryVisible");

        return hp == 0;
    }

    IEnumerator TemporaryVisible()
    {
        SetVisible(true);
        yield return new WaitForSeconds(tempTime);
        SetVisible(false);
    }
}
