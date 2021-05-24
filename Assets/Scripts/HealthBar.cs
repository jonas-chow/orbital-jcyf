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

    // Update is called once per frame
    void Update()
    {
        if (visible) {
            float scale = (float) hp / maxHp;
            greenBar.transform.localPosition = NewPosition(scale);
            greenBar.transform.localScale = new Vector3(scale, 1, 1);
        }
    }

    Vector3 NewPosition(float scale)
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
    public bool TakeDamage(int damage)
    {
        hp -= damage;

        // refreshes the visible time if already visible
        StopCoroutine("TemporaryVisible");
        StartCoroutine("TemporaryVisible");

        if (hp < 0) {
            hp = 0;
            return true;
        } else if (hp > maxHp) {
            hp = maxHp;
        }
        return false;
    }

    IEnumerator TemporaryVisible()
    {
        SetVisible(true);
        yield return new WaitForSeconds(tempTime);
        SetVisible(false);
    }
}
