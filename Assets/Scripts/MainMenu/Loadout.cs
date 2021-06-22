using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Loadout : MonoBehaviour
{
    public MeleeSelect meleeSelect;
    public RangedSelect rangedSelect;
    public MageSelect mageSelect;

    public TMP_Dropdown meleeDropdown;
    public TMP_Dropdown rangedDropdown;
    public TMP_Dropdown mageDropdown;

    public GameObject characterDetails;

    void OnEnable()
    {
        meleeDropdown.value = PlayerPrefs.GetInt("Melee", (int)Melees.Assassin);
        rangedDropdown.value = PlayerPrefs.GetInt("Ranged", (int)Rangeds.Hunter);
        mageDropdown.value = PlayerPrefs.GetInt("Mage", (int)Mages.Wizard);

        SelectMelee();
        SelectRanged();
        SelectMage();
    }

    public void SelectMelee()
    {
        meleeSelect.ChangeIcon((Melees)meleeDropdown.value);
    }

    public void SelectRanged()
    {
        rangedSelect.ChangeIcon((Rangeds)rangedDropdown.value);
    }

    public void SelectMage()
    {
        mageSelect.ChangeIcon((Mages)mageDropdown.value);
    }

    public void Save()
    {
        // button click
        PlayerPrefs.SetInt("Melee", meleeDropdown.value);
        PlayerPrefs.SetInt("Ranged", rangedDropdown.value);
        PlayerPrefs.SetInt("Mage", mageDropdown.value);

        this.gameObject.SetActive(false);
    }

    public void Cancel()
    {
        // button click
        this.gameObject.SetActive(false);
    }

    public void MeleeDetails()
    {
        characterDetails.SetActive(true);
        CharacterDetails details = characterDetails.GetComponent<CharacterDetails>();
        details.Init(((Melees)meleeDropdown.value).ToString());
    }

    public void RangedDetails()
    {
        characterDetails.SetActive(true);
        CharacterDetails details = characterDetails.GetComponent<CharacterDetails>();
        details.Init(((Rangeds)rangedDropdown.value).ToString());
    }

    public void MageDetails()
    {
        characterDetails.SetActive(true);
        CharacterDetails details = characterDetails.GetComponent<CharacterDetails>();
        details.Init(((Mages)mageDropdown.value).ToString());
    }
}
