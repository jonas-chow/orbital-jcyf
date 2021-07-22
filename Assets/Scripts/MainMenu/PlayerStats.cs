using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class PlayerStats : MonoBehaviour
{
    public TMP_Text wins;
    public TMP_Text losses;
    public TMP_Text winPercentage;
    public TMP_Text gamesPlayed;
    public TMP_Text mostMelee;
    public TMP_Text mostRange;
    public TMP_Text mostMage;

    public void Back()
    {
        AudioManager.Instance.Play("Click");
        gameObject.SetActive(false);
    }

    public void Reset()
    {
        AudioManager.Instance.Play("Click");
        PlayerPrefs.DeleteKey("winCount");
        PlayerPrefs.DeleteKey("loseCount");
        PlayerPrefs.DeleteKey("winPercent");
        PlayerPrefs.DeleteKey("totalGames");
        PlayerPrefs.DeleteKey("Tank");
        PlayerPrefs.DeleteKey("Bruiser");
        PlayerPrefs.DeleteKey("Assassin");
        PlayerPrefs.DeleteKey("Scout");
        PlayerPrefs.DeleteKey("Trapper");
        PlayerPrefs.DeleteKey("Hunter");
        PlayerPrefs.DeleteKey("Healer");
        PlayerPrefs.DeleteKey("Wizard");
        PlayerPrefs.DeleteKey("Summoner");
        wins.text = "0";
        losses.text = "0";
        mostMelee.text = "Most Played Melee: None (0 games played)";
        mostRange.text = "Most Played Range: None (0 games played)";
        mostMage.text = "Most Played Mage: None (0 games played)";
        winPercentage.text = "Win Rate: 0.0%";
        gamesPlayed.text = "Games Played: 0";
    }

    private string mostPlayedMelee()
    {
        if (PlayerPrefs.GetInt("Tank", 0) == 0 & PlayerPrefs.GetInt("Bruiser", 0) == 0
            & PlayerPrefs.GetInt("Assassin", 0) == 0)
        {
            return "None";
        }
        if (PlayerPrefs.GetInt("Tank", 0) >= PlayerPrefs.GetInt("Bruiser", 0)
            & PlayerPrefs.GetInt("Tank", 0) >= PlayerPrefs.GetInt("Assassin", 0))
        {
            return "Tank";
        }
        if (PlayerPrefs.GetInt("Bruiser", 0) >= PlayerPrefs.GetInt("Assassin", 0)
        & PlayerPrefs.GetInt("Bruiser", 0) >= PlayerPrefs.GetInt("Tank", 0))
        {
            return "Bruiser";
        }
        if (PlayerPrefs.GetInt("Assassin", 0) >= PlayerPrefs.GetInt("Tank", 0)
            & PlayerPrefs.GetInt("Assassin", 0) >= PlayerPrefs.GetInt("Bruiser", 0)) 
        {
            return "Assassin";
        }
        return "error";
    }

    private string mostPlayedRange()
    {
        if (PlayerPrefs.GetInt("Scout", 0) == 0 & PlayerPrefs.GetInt("Trapper", 0) == 0
            & PlayerPrefs.GetInt("Hunter", 0) == 0)
            {
                return "None";
            }
        if (PlayerPrefs.GetInt("Scout", 0) >= PlayerPrefs.GetInt("Trapper", 0)
        & PlayerPrefs.GetInt("Scout", 0) >= PlayerPrefs.GetInt("Hunter", 0))
        {
            return "Scout";
        }
        if (PlayerPrefs.GetInt("Trapper", 0) >= PlayerPrefs.GetInt("Hunter", 0)
            & PlayerPrefs.GetInt("Trapper", 0) >= PlayerPrefs.GetInt("Scout", 0)) 
        {
            return "Trapper";
        }
        if (PlayerPrefs.GetInt("Hunter", 0) >= PlayerPrefs.GetInt("Scout", 0)
            & PlayerPrefs.GetInt("Hunter", 0) >= PlayerPrefs.GetInt("Trapper", 0))
        {
            return "Hunter";
        }
        return "error";
    }

    private string mostPlayedMage()
    {
        if (PlayerPrefs.GetInt("Healer", 0) == 0 & PlayerPrefs.GetInt("Wizard", 0) == 0
            & PlayerPrefs.GetInt("Summoner", 0) == 0)
            {
                return "None";
            }
        if (PlayerPrefs.GetInt("Healer", 0) >= PlayerPrefs.GetInt("Wizard", 0)
        & PlayerPrefs.GetInt("Healer", 0) >= PlayerPrefs.GetInt("Summoner", 0))
        {
            return "Healer";
        }
        if (PlayerPrefs.GetInt("Wizard", 0) >= PlayerPrefs.GetInt("Summoner", 0)
            & PlayerPrefs.GetInt("Wizard", 0) >= PlayerPrefs.GetInt("Healer", 0)) 
        {
            return "Wizard";
        }
        if (PlayerPrefs.GetInt("Summoner", 0) >= PlayerPrefs.GetInt("Healer", 0)
            & PlayerPrefs.GetInt("Summoner", 0) >= PlayerPrefs.GetInt("Wizard", 0))
        {
            return "Summoner";
        }
        return "error";
    }

    void Start()
    {
        wins.text = PlayerPrefs.GetFloat("winCount").ToString();
        losses.text = PlayerPrefs.GetFloat("loseCount").ToString();
        winPercentage.text = "Win Rate: " + PlayerPrefs.GetFloat("winPercent").ToString("#0.0") + "%";
        gamesPlayed.text = "Games Played: " + PlayerPrefs.GetFloat("totalGames").ToString();
        if (PlayerPrefs.GetInt(mostPlayedMelee()) == 1) {
            mostMelee.text = "Most Played Melee: " + mostPlayedMelee() + 
                " (" + PlayerPrefs.GetInt(mostPlayedMelee()).ToString() + " game played)";
        } else {
            mostMelee.text = "Most Played Melee: " + mostPlayedMelee() + 
                " (" + PlayerPrefs.GetInt(mostPlayedMelee()).ToString() + " games played)";
        }
        if (PlayerPrefs.GetInt(mostPlayedRange()) == 1) {
            mostRange.text = "Most Played Range: " + mostPlayedRange() + 
                " (" + PlayerPrefs.GetInt(mostPlayedRange()).ToString() + " game played)";
        } else {
            mostRange.text = "Most Played Range: " + mostPlayedRange() + 
                " (" + PlayerPrefs.GetInt(mostPlayedRange()).ToString() + " games played)";
        }
        if (PlayerPrefs.GetInt(mostPlayedMage()) == 1) {
            mostMage.text = "Most Played Mage: " + mostPlayedMage() + 
                " (" + PlayerPrefs.GetInt(mostPlayedMage()).ToString() + " game played)";
        } else {
            mostMage.text = "Most Played Mage: " + mostPlayedMage() + 
                " (" + PlayerPrefs.GetInt(mostPlayedMage()).ToString() + " games played)";
        }
    }
}
