using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Loadout : MonoBehaviour
{
    public TMP_Dropdown meleeDropdown;
    public TMP_Dropdown rangedDropdown;
    public TMP_Dropdown mageDropdown;

    public GameObject characterDetails;

    public Image meleeIcon;
    public Image rangedIcon;
    public Image mageIcon;

    void OnEnable()
    {
        meleeDropdown.value = PlayerPrefs.GetInt("Melee", (int)Melees.Assassin);
        rangedDropdown.value = PlayerPrefs.GetInt("Ranged", (int)Rangeds.Hunter);
        mageDropdown.value = PlayerPrefs.GetInt("Mage", (int)Mages.Wizard);

        SelectMelee(meleeDropdown.value);
        SelectRanged(rangedDropdown.value);
        SelectMage(mageDropdown.value);
    }

    public void SelectMelee(int value)
    {
        meleeIcon.sprite = SpriteManager.Instance.GetSprite(((Melees)value).ToString(), true);
    }

    public void SelectRanged(int value)
    {
        rangedIcon.sprite = SpriteManager.Instance.GetSprite(((Rangeds)value).ToString(), true);
    }

    public void SelectMage(int value)
    {
        mageIcon.sprite = SpriteManager.Instance.GetSprite(((Mages)value).ToString(), true);
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
