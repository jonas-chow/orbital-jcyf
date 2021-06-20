using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance { get { return instance; } }
    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        } else {
            instance = this;
        }
    }

    void Start()
    {
        if (testing) {
            GameObject tank = BuildChar("Tank", false);
            GridManager.Instance.MoveToAndInsert(tank, 3, 5);
            GameObject bruiser = BuildChar("Bruiser", false);
            GridManager.Instance.MoveToAndInsert(bruiser, 4, 5);
            GameObject assassin = BuildChar("Assassin", false);
            GridManager.Instance.MoveToAndInsert(assassin, 5, 5);
            GameObject trapper = BuildChar("Trapper", false);
            GridManager.Instance.MoveToAndInsert(trapper, 6, 5);
            GameObject scout = BuildChar("Scout", false);
            GridManager.Instance.MoveToAndInsert(scout, 7, 5);
            GameObject hunter = BuildChar("Hunter", false);
            GridManager.Instance.MoveToAndInsert(hunter, 8, 5);
            GameObject wizard = BuildChar("Wizard", false);
            GridManager.Instance.MoveToAndInsert(wizard, 9, 5);
            GameObject healer = BuildChar("Healer", false);
            GridManager.Instance.MoveToAndInsert(healer, 10, 5);
            GameObject summoner = BuildChar("Summoner", false);
            GridManager.Instance.MoveToAndInsert(summoner, 11, 5);
            friendly = System.Array.ConvertAll(new GameObject[] {
                tank, bruiser, assassin, 
                trapper, scout, hunter, 
                wizard, healer, summoner},
                go => go.GetComponent<CharacterMovement>());

            GameObject etank = BuildChar("Tank", true);
            GridManager.Instance.MoveToAndInsert(etank, 3, 10);
            GameObject ebruiser = BuildChar("Bruiser", true);
            GridManager.Instance.MoveToAndInsert(ebruiser, 4, 10);
            GameObject eassassin = BuildChar("Assassin", true);
            GridManager.Instance.MoveToAndInsert(eassassin, 5, 10);
            GameObject etrapper = BuildChar("Trapper", true);
            GridManager.Instance.MoveToAndInsert(etrapper, 6, 10);
            GameObject escout = BuildChar("Scout", true);
            GridManager.Instance.MoveToAndInsert(escout, 7, 10);
            GameObject ehunter = BuildChar("Hunter", true);
            GridManager.Instance.MoveToAndInsert(ehunter, 8, 10);
            GameObject ewizard = BuildChar("Wizard", true);
            GridManager.Instance.MoveToAndInsert(ewizard, 9, 10);
            GameObject ehealer = BuildChar("Healer", true);
            GridManager.Instance.MoveToAndInsert(ehealer, 10, 10);
            GameObject esummoner = BuildChar("Summoner", true);
            GridManager.Instance.MoveToAndInsert(esummoner, 11, 10);
            enemies = System.Array.ConvertAll(new GameObject[] {
                etank, ebruiser, eassassin, 
                etrapper, escout, ehunter, 
                ewizard, ehealer, esummoner},
                go => go.GetComponent<CharacterMovement>());
            
            foreach (CharacterMovement enemy in enemies) {
                enemy.Face("down");
            }

            numFriendly = 9;
            numEnemy = 9;
            loadingUI.SetActive(false);
            readyForTurn = true;
        } else {
            InstantiateSelf();
        }
    }

    public bool testing;
    [SerializeField]
    private GameObject defeatUI, victoryUI, loadingUI, yourTurnUI, enemyTurnUI;

    [SerializeField]
    private GameObject Tank, Bruiser, Assassin;
    [SerializeField]
    private GameObject Scout, Trapper, Hunter;
    [SerializeField]
    private GameObject Healer, Wizard, Summoner;
    public CharacterMovement[] enemies;
    public CharacterMovement[] friendly;
    private int numEnemy = 3;
    private int numFriendly = 3;
    private int currentChar = 0;
    private bool friendlyLoaded = false;
    private bool enemyLoaded = false;
    public bool enemyReady = false;
    
    private bool animationPhase = true;
    private bool animating = false;
    public bool readyForTurn = false;
    private float animationGap = .1f;
    public int actionCount = 0;

    public void InstantiateSelf()
    {
        // instantiate melee character
        GameObject melee = BuildChar(((Melees)PlayerPrefs.GetInt("Melee", 0)).ToString(), false);
        GridManager.Instance.MoveToAndInsert(melee, 0, 0);

        // instantiate ranged character
        GameObject ranged = BuildChar(((Rangeds)PlayerPrefs.GetInt("Ranged", 0)).ToString(), false);
        GridManager.Instance.MoveToAndInsert(ranged, 1, 0);

        // instantiate mage character
        GameObject mage = BuildChar(((Mages)PlayerPrefs.GetInt("Mage", 0)).ToString(), false);
        GridManager.Instance.MoveToAndInsert(mage, 2, 0);

        // set those characterMovements into the friendly array
        friendly = new CharacterMovement[] {
            melee.GetComponent<CharacterMovement>(),
            ranged.GetComponent<CharacterMovement>(),
            mage.GetComponent<CharacterMovement>()};

        CharacterMenu.Instance.init(new int[] {
            friendly[0].attack1.cooldown, 
            friendly[0].attack2.cooldown, 
            friendly[0].attack3.cooldown, 
            friendly[1].attack1.cooldown, 
            friendly[1].attack2.cooldown, 
            friendly[1].attack3.cooldown, 
            friendly[2].attack1.cooldown, 
            friendly[2].attack2.cooldown, 
            friendly[2].attack3.cooldown});

        CharacterMenu.Instance.SelectChar(0);

        friendlyLoaded = true;
        CheckLoading();
        CheckBothReady();

        // For single player mode
        if (EventHandler.Instance == null) {
            InstantiateEnemies(0, 0, 0);
        }
    }

    public void InstantiateEnemies(int meleeChar, int rangedChar, int mageChar)
    {
        // instantiate melee character
        GameObject melee = BuildChar(((Melees)meleeChar).ToString(), true);
        GridManager.Instance.MoveToAndInsert(melee, 15, 15);

        // instantiate ranged character
        GameObject ranged = BuildChar(((Rangeds)rangedChar).ToString(), true);
        GridManager.Instance.MoveToAndInsert(ranged, 14, 15);

        // instantiate mage character
        GameObject mage = BuildChar(((Mages)mageChar).ToString(), true);
        GridManager.Instance.MoveToAndInsert(mage, 13, 15);

        // set those characterMovements into the friendly array
        enemies = new CharacterMovement[] {
            melee.GetComponent<CharacterMovement>(),
            ranged.GetComponent<CharacterMovement>(),
            mage.GetComponent<CharacterMovement>()};

        // they face "up" for the enemy, so "down" for us
        foreach (CharacterMovement enemy in enemies) {
            enemy.Face("down");
        }

        enemyLoaded = true;

        CheckLoading();
        CheckBothReady();
    }

    private GameObject BuildChar(string prefabName, bool isEnemy)
    {
        GameObject obj = null;
        switch (prefabName)
        {
            case "Tank":
                obj = Tank;
                break;
            case "Bruiser":
                obj = Bruiser;
                break;
            case "Assassin":
                obj = Assassin;
                break;
            case "Scout":
                obj = Scout;
                break;
            case "Trapper":
                obj = Trapper;
                break;
            case "Hunter":
                obj = Hunter;
                break;
            case "Healer":
                obj = Healer;
                break;
            case "Wizard":
                obj = Wizard;
                break;
            case "Summoner":
                obj = Summoner;
                break;
            default:
                throw new NotImplementedException();
        }
        if (obj != null) {
            obj = Instantiate(obj, transform);
            obj.GetComponent<CharacterMovement>().SetEnemy(isEnemy);
        }
        return obj;
    }

    private void CheckLoading()
    {
        if (friendlyLoaded && enemyLoaded) {
            EventHandler.Instance.SendReady();
        }
    }

    public void CheckBothReady()
    {
        if (friendlyLoaded && enemyLoaded && enemyReady) {
            loadingUI.SetActive(false);
            EventHandler.Instance.FlipCoin();
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (numEnemy > 0 && numFriendly > 0) {
            if (readyForTurn) {
                readyForTurn = false;
                StartCoroutine(StartTurn());
            }

            if (TimeBar.Instance.IsTurn()) {
                if (Input.GetButtonDown("Next Character")) {
                    DeactivateCurrent();
                    currentChar = (currentChar + 1) % numFriendly;
                    ActivateCurrent();
                    AudioManager.Instance.Play("SwitchCharacters");
                }

                if (Input.GetButtonDown("Previous Character")) {
                    DeactivateCurrent();
                    currentChar = (currentChar + numFriendly - 1) % numFriendly;
                    ActivateCurrent();
                    AudioManager.Instance.Play("SwitchCharacters");
                }
            }

            if (animationPhase && !animating) {
                animating = true;
                StartCoroutine(AnimateActions());
            }
        }
    }
    
    public void RemoveEnemy() {
        numEnemy--;
        if (numEnemy == 0) {
            Win();
        }
    }
    
    public void RemoveFriendly(CharacterMovement dead) {
        int deadIdx = Array.FindIndex(friendly, character => character.Equals(dead));
        numFriendly--;

        if (numFriendly > 0) {
            if (deadIdx >= currentChar) {
                CharacterMovement[] newFriendly = new CharacterMovement[numFriendly];
                int i = 0;
                foreach (CharacterMovement cm in friendly)
                {
                    if (!cm.Equals(dead))
                    {
                        newFriendly[i] = cm;
                        i++;
                    }
                }
                friendly = newFriendly;
                if (currentChar >= numFriendly) {
                    currentChar = 0;
                }
            } else {
                CharacterMovement[] newFriendly = new CharacterMovement[numFriendly];
                int i = 0;
                foreach (CharacterMovement cm in friendly)
                {
                    if (!cm.Equals(dead))
                    {
                        newFriendly[i] = cm;
                        i++;
                    }
                }
                friendly = newFriendly;
                currentChar--;
            }

            if (TimeBar.Instance.IsTurn()) {
                ActivateCurrent();
            }
        } else {
            Lose();
        }
    }

    private void Lose() {
        defeatUI.SetActive(true);
        AudioManager.Instance.Play("Lose");
    }

    private void Win() {
        victoryUI.SetActive(true);
        AudioManager.Instance.Play("Win");
    }
    

    void DeactivateCurrent()
    {
        friendly[currentChar].Deactivate();
    }

    void ActivateCurrent()
    {
        friendly[currentChar].Activate();
        CharacterMenu.Instance.SelectChar(friendly[currentChar].charID);
    }

    IEnumerator StartTurn()
    {
        System.Array.ForEach(friendly, cm => cm.TurnPass());
        System.Array.ForEach(enemies, cm => cm.TurnPass());
        StartCoroutine(AppearForAWhile(yourTurnUI));
        ActivateCurrent();
        animationPhase = true;
        // start turn and wait for turn
        TimeBar.Instance.Reset();
        while (TimeBar.Instance.IsTurn()) {
            yield return new WaitForSeconds(0.1f);
        }
        DeactivateCurrent();

        while (true) {
            if (!animationPhase) {
                break;
            } else {
                yield return new WaitForSeconds(.1f);
            }
        }

        EventHandler.Instance.SendTurnEndEvent();
        EnemyTurn();
        System.Array.ForEach(friendly, cm => cm.TurnPass());
        System.Array.ForEach(enemies, cm => cm.TurnPass());
        
        if (testing) {
            foreach (CharacterMovement cm in enemies) {
                MoveRandomly(cm);
            }
            yield return new WaitForSeconds(.5f);
            readyForTurn = true;
        }
    }

    public void EnemyTurn()
    {       
        StartCoroutine(AppearForAWhile(enemyTurnUI));
    }

    IEnumerator AnimateActions()
    {
        while (TimeBar.Instance.IsTurn() || ActionQueue.Instance.hasActions()) {
            if (ActionQueue.Instance.hasActions()) {
                ActionQueue.Instance.ExecuteNext();
            }
            yield return new WaitForSeconds(animationGap);
        }
        ActionQueue.Instance.ResetQueue();
        animationPhase = false;
        animating = false;
    }

    IEnumerator AppearForAWhile(GameObject obj)
    {
        obj.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        obj.SetActive(false);
    }

    public void OpponentDisconnect()
    {
        if (numEnemy > 0 && numFriendly > 0) {
            Win();
        }
    }

    public void ActionAdded()
    {
        actionCount++;
        CharacterMenu.Instance.TurnPass();
    }

    public void InsertChar(CharacterMovement cm)
    {
        if (cm.isEnemy) {
            CharacterMovement[] newEnemies = new CharacterMovement[numEnemy + 1];
            for (int i = 0; i < numEnemy; i++) {
                newEnemies[i] = enemies[i];
            }
            newEnemies[numEnemy] = cm;
            numEnemy++;
            enemies = newEnemies;
        } else {
            CharacterMovement[] newFriendlies = new CharacterMovement[numFriendly + 1];
            for (int i = 0; i < numFriendly; i++) {
                newFriendlies[i] = friendly[i];
            }
            newFriendlies[numFriendly] = cm;
            numFriendly++;
            friendly = newFriendlies;
            CharacterMenu.Instance.Set4thChar(new int[] {
                cm.attack1.cooldown, cm.attack2.cooldown, cm.attack3.cooldown
            });
        }
    }

    public void MoveRandomly(CharacterMovement character)
    {
        if (character.isAlive) {
            switch (UnityEngine.Random.Range(0, 4))
            {
                case 0:
                    character.Move("up");
                    break;
                case 1:
                    character.Move("down");
                    break;
                case 2:
                    character.Move("left");
                    break;
                case 3:
                    character.Move("right");
                    break;
            }
        }
    }
}
