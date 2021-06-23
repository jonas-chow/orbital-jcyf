using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CharacterDetails : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI stats;
    public TextMeshProUGUI attack1;
    public TextMeshProUGUI attack2;
    public TextMeshProUGUI attack3;

    public void BackButton()
    {
        AudioManager.Instance.Play("Click");
        this.gameObject.SetActive(false);
    }

    public void Init(string character)
    {
        nameText.text = character;
        Attack _attack1 = null;
        Attack _attack2 = null;
        Attack _attack3 = null;
        switch (character)
        {
            case "Assassin":
                stats.text = $"HP: {AssassinMovement._hp}     "
                    + $"Attack: {AssassinMovement._attack}     "
                    + $"Defense: {AssassinMovement._defense}";
                _attack1 = new AssassinMovement.Attack1(null);
                _attack2 = new AssassinMovement.Attack2(null);
                _attack3 = new AssassinMovement.Attack3(null);
                break;
            case "Bruiser":
                stats.text = $"HP: {BruiserMovement._hp}     "
                    + $"Attack: {BruiserMovement._attack}     "
                    + $"Defense: {BruiserMovement._defense}";
                _attack1 = new BruiserMovement.Attack1(null);
                _attack2 = new BruiserMovement.Attack2(null);
                _attack3 = new BruiserMovement.Attack3(null);
                break;
            case "Tank":
                stats.text = $"HP: {TankMovement._hp}     "
                    + $"Attack: {TankMovement._attack}     "
                    + $"Defense: {TankMovement._defense}";
                _attack1 = new TankMovement.Attack1(null);
                _attack2 = new TankMovement.Attack2(null);
                _attack3 = new TankMovement.Attack3(null);
                break;
            case "Hunter":
                stats.text = $"HP: {HunterMovement._hp}     "
                    + $"Attack: {HunterMovement._attack}     "
                    + $"Defense: {HunterMovement._defense}";
                _attack1 = new HunterMovement.Attack1(null);
                _attack2 = new HunterMovement.Attack2(null);
                _attack3 = new HunterMovement.Attack3(null);
                break;
            case "Scout":
                stats.text = $"HP: {ScoutMovement._hp}     "
                    + $"Attack: {ScoutMovement._attack}     "
                    + $"Defense: {ScoutMovement._defense}";
                _attack1 = new ScoutMovement.Attack1(null);
                _attack2 = new ScoutMovement.Attack2(null);
                _attack3 = new ScoutMovement.Attack3(null);
                break;
            case "Trapper":
                stats.text = $"HP: {TrapperMovement._hp}     "
                    + $"Attack: {TrapperMovement._attack}     "
                    + $"Defense: {TrapperMovement._defense}";
                _attack1 = new TrapperMovement.Attack1(null);
                _attack2 = new TrapperMovement.Attack2(null);
                _attack3 = new TrapperMovement.Attack3(null);
                break;
            case "Wizard":
                stats.text = $"HP: {WizardMovement._hp}     "
                    + $"Attack: {WizardMovement._attack}     "
                    + $"Defense: {WizardMovement._defense}";
                _attack1 = new WizardMovement.Attack1(null);
                _attack2 = new WizardMovement.Attack2(null);
                _attack3 = new WizardMovement.Attack3(null);
                break;
            case "Summoner":
                stats.text = $"HP: {SummonerMovement._hp}     "
                    + $"Attack: {SummonerMovement._attack}     "
                    + $"Defense: {SummonerMovement._defense}";
                _attack1 = new SummonerMovement.Attack1(null);
                _attack2 = new SummonerMovement.Attack2(null);
                _attack3 = new SummonerMovement.Attack3(null);
                break;
            case "Healer":
                stats.text = $"HP: {HealerMovement._hp}     "
                    + $"Attack: {HealerMovement._attack}     "
                    + $"Defense: {HealerMovement._defense}";
                _attack1 = new HealerMovement.Attack1(null);
                _attack2 = new HealerMovement.Attack2(null);
                _attack3 = new HealerMovement.Attack3(null);
                break;
        }

        attack1.text = _attack1 == null ? "" : _attack1.GetDescription();
        attack2.text = _attack2 == null ? "" : _attack2.GetDescription();
        attack3.text = _attack3 == null ? "" : _attack3.GetDescription();
    }
}
