using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loadout : MonoBehaviour
{
    void Start()
    {
        PlayerPrefs.GetString("Melee", "Melee1");
        PlayerPrefs.GetString("Ranged", "Ranged1");
        PlayerPrefs.GetString("Mage", "Mage1");
    }

    public void SelectMelee()
    {

    }

    public void SelectRanged()
    {

    }

    public void SelectMage()
    {

    }

    public void Save()
    {
        // PlayerPrefs.SetString....
        this.gameObject.SetActive(false);
    }

    public void Cancel()
    {
        this.gameObject.SetActive(false);
    }
}
