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
        AudioManager.Instance.Play("Click");
        meleeSelect.ChangeIcon((Melees)meleeDropdown.value);
    }

    public void SelectRanged()
    {
        AudioManager.Instance.Play("Click");
        rangedSelect.ChangeIcon((Rangeds)rangedDropdown.value);
    }

    public void SelectMage()
    {
        AudioManager.Instance.Play("Click");
        mageSelect.ChangeIcon((Mages)mageDropdown.value);
    }

    public void Save()
    {
        AudioManager.Instance.Play("Click");
        PlayerPrefs.SetInt("Melee", meleeDropdown.value);
        PlayerPrefs.SetInt("Ranged", rangedDropdown.value);
        PlayerPrefs.SetInt("Mage", mageDropdown.value);

        this.gameObject.SetActive(false);
    }

    public void Cancel()
    {
        AudioManager.Instance.Play("Click");
        this.gameObject.SetActive(false);
    }

    public void MeleeDetails()
    {
        AudioManager.Instance.Play("Click");
        characterDetails.SetActive(true);
        CharacterDetails details = characterDetails.GetComponent<CharacterDetails>();
        details.Init(((Melees)meleeDropdown.value).ToString());
    }

    public void RangedDetails()
    {
        AudioManager.Instance.Play("Click");
        characterDetails.SetActive(true);
        CharacterDetails details = characterDetails.GetComponent<CharacterDetails>();
        details.Init(((Rangeds)rangedDropdown.value).ToString());
    }

    public void MageDetails()
    {
        AudioManager.Instance.Play("Click");
        characterDetails.SetActive(true);
        CharacterDetails details = characterDetails.GetComponent<CharacterDetails>();
        details.Init(((Mages)mageDropdown.value).ToString());
    }
}
