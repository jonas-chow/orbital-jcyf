using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

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
        AudioManager.Instance.SetBGMVolume(PlayerPrefs.GetFloat("BGM", 0.1f));
        AudioManager.Instance.SetSoundEffectVolume(PlayerPrefs.GetFloat("SE", 1f));
        
        gameMode = PlayerPrefs.GetInt("Mode", 0);
        autoMove = PlayerPrefs.GetInt("Movement", 0) == 1;

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
            CharacterMenu.Instance.Init(new CharacterMovement[] {friendly[0], friendly[3], friendly[6]});
        } else {
            replay = new Replay();
            InstantiateSelf();
        }
    }

    public bool testing;
    public int gameMode;
    [SerializeField]
    private GameObject defeatUI, victoryUI, loadingUI, yourTurnUI, enemyTurnUI, pauseUI, 
        opponentConcedeUI, opponentDisconnectUI, statsUI;

    [SerializeField]
    private GameObject Tank, Bruiser, Assassin;
    [SerializeField]
    private GameObject Scout, Trapper, Hunter;
    [SerializeField]
    private GameObject Healer, Wizard, Summoner;
    [SerializeField]
    private GameObject TargetDummy;
    public CharacterMovement[] enemies;
    public CharacterMovement[] friendly;
    private int numEnemy = 3;
    private int numFriendly = 3;
    private int currentChar = 0;
    private bool friendlyLoaded = false;
    private bool enemyLoaded = false;
    public bool enemyReady = false;
    
    private bool conceded = false;
    private bool animationPhase = true;
    private bool animating = false;
    public bool readyForTurn = false;
    private float animationGap = .1f;
    public int actionCount = 0;
    private bool paused = false;
    public TextMeshProUGUI tooltipText;
    public TextMeshProUGUI statsText;

    private int turnCount = 0;
    private bool autoMove = false;
    public Replay replay;

    public void InstantiateSelf()
    {
        string meleeChar = ((Melees)PlayerPrefs.GetInt("Melee", 0)).ToString();
        string rangedChar = ((Rangeds)PlayerPrefs.GetInt("Ranged", 0)).ToString();
        string mageChar = ((Mages)PlayerPrefs.GetInt("Mage", 0)).ToString();
        // instantiate melee character
        GameObject melee = BuildChar(meleeChar, false);
        GridManager.Instance.MoveToAndInsert(melee, 0, 0);

        // instantiate ranged character
        GameObject ranged = BuildChar(rangedChar, false);
        GridManager.Instance.MoveToAndInsert(ranged, 1, 0);

        // instantiate mage character
        GameObject mage = BuildChar(mageChar, false);
        GridManager.Instance.MoveToAndInsert(mage, 2, 0);

        // set those characterMovements into the friendly array
        friendly = new CharacterMovement[] {
            melee.GetComponent<CharacterMovement>(),
            ranged.GetComponent<CharacterMovement>(),
            mage.GetComponent<CharacterMovement>()};

        CharacterMenu.Instance.Init(friendly);

        CharacterMenu.Instance.SelectChar(0);
        replay.SetFriendlies(new string[3] {meleeChar, rangedChar, mageChar});

        friendlyLoaded = true;
        CheckLoading();
        CheckBothReady();

        // For single player mode
        if (gameMode == 1) {
            // Instantiate dummy
            GameObject dummy = Instantiate(TargetDummy, transform);
            CharacterMovement dummyMovement = dummy.GetComponent<CharacterMovement>();
            dummyMovement.SetEnemy(true);
            dummyMovement.Face("down");
            // if failed to insert in specified position
            if (!GridManager.Instance.MoveToAndInsert(dummy, PlayerPrefs.GetInt("X", 15), PlayerPrefs.GetInt("Y", 15)))
            {
                // default position is (15, 15)
                GridManager.Instance.MoveToAndInsert(dummy, 15, 15);
            }
            enemies = new CharacterMovement[] {dummyMovement};
            numEnemy = 1;
            loadingUI.SetActive(false);
            readyForTurn = true;
        }
    }

    public void InstantiateEnemies(int meleeId, int rangedId, int mageId)
    {
        string meleeChar = ((Melees)meleeId).ToString();
        string rangedChar = ((Rangeds)rangedId).ToString();
        string mageChar = ((Mages)mageId).ToString();

        // instantiate melee character
        GameObject melee = BuildChar(meleeChar, true);
        GridManager.Instance.MoveToAndInsert(melee, 15, 15);

        // instantiate ranged character
        GameObject ranged = BuildChar(rangedChar, true);
        GridManager.Instance.MoveToAndInsert(ranged, 14, 15);

        // instantiate mage character
        GameObject mage = BuildChar(mageChar, true);
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

        replay.SetEnemies(new string[3] {meleeChar, rangedChar, mageChar});

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

            if (TimeBar.Instance.IsTurn() && !paused) {
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

            if (Input.GetButtonDown("Pause") && !conceded) {
                Pause();
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

    // Pausing brings up the UI and prevents any actions until you resume
    public void Pause() {
        AudioManager.Instance.Play("Click");
        paused = !paused;
        if (TimeBar.Instance.IsTurn()) {
            friendly[currentChar].isActive = !paused;
        }
        pauseUI.SetActive(paused);
    }

    public void Concede() {
        AudioManager.Instance.Play("Click");
        EventHandler.Instance.SendConcedeEvent();
        StopGame();
        conceded = true;
        Lose();
    }
    
    public void OpponentConcede() {
        conceded = true;
        opponentConcedeUI.SetActive(true);
        StopGame();
    }

    private void StopGame() {
        paused = true;
        friendly[currentChar].isActive = false;
        pauseUI.SetActive(false);
        TimeBar.Instance.Stop();
    }

    public void Lose() {
        defeatUI.SetActive(true);
        if (gameMode == 1) {
            statsUI.SetActive(true);
            statsText.text = $"Turns taken: {turnCount}\nActions taken: {actionCount}";
        }
        AudioManager.Instance.Stop("BattleTheme");
        AudioManager.Instance.Play("Lose");
    }

    public void Win() {
        victoryUI.SetActive(true);
        if (gameMode == 1) {
            statsUI.SetActive(true);
            statsText.text = $"Turns taken: {turnCount}\nActions taken: {actionCount}";
        }
        AudioManager.Instance.Stop("BattleTheme");
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
        if (paused) {
            friendly[currentChar].isActive = false;
        }
    }

    IEnumerator StartTurn()
    {
        ActionQueue.Instance.TurnEnd();
        turnCount++;
        System.Array.ForEach(friendly, cm => cm.TurnPass());
        System.Array.ForEach(enemies, cm => cm.TurnPass());
        StartCoroutine(AppearForAWhile(yourTurnUI));
        ActivateCurrent();
        animationPhase = true;
        // start turn and wait for turn
        TimeBar.Instance.Reset();
        while (TimeBar.Instance.IsTurn() || conceded) {
            yield return new WaitForSeconds(0.1f);
        }
        DeactivateCurrent();

        while (animationPhase) {
            yield return new WaitForSeconds(.1f);
        }

        ActionQueue.Instance.TurnEnd();
        EventHandler.Instance.SendTurnEndEvent();
        EnemyTurn();
        System.Array.ForEach(friendly, cm => cm.TurnPass());
        System.Array.ForEach(enemies, cm => cm.TurnPass());
        
        if (gameMode == 1 || testing) {
            if (autoMove) {
                foreach (CharacterMovement cm in enemies) {
                    if (cm.isAlive) {
                        for (int i = 0; i < 3; i++) {
                            MoveRandomly(cm);
                            yield return new WaitForSeconds(0.15f);
                        }
                    }
                }
            }
            yield return new WaitForSeconds(.5f);
            readyForTurn = true;
        }
        AudioManager.Instance.Play("Turn");
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
        if (numEnemy > 0 && numFriendly > 0 && !conceded) {
            opponentDisconnectUI.SetActive(true);
            StopGame();
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
            // want to maintain enemies as a way to index enemies
            if (enemies.Length == 3) {
                CharacterMovement[] newEnemies = new CharacterMovement[4];
                for (int i = 0; i < 3; i++) {
                    newEnemies[i] = enemies[i];
                }
                newEnemies[3] = cm;
                numEnemy++;
                enemies = newEnemies;
            } else {
                // ensure that only one char of that index is alive
                if (enemies[3].isAlive) {
                    enemies[3].Die();
                }
                enemies[3] = cm;
                numEnemy++;
            }
        } else {
            CharacterMovement[] newFriendlies = new CharacterMovement[numFriendly + 1];
            for (int i = 0; i < numFriendly; i++) {
                newFriendlies[i] = friendly[i];
            }
            newFriendlies[numFriendly] = cm;
            numFriendly++;
            friendly = newFriendlies;
            CharacterMenu.Instance.Set4thChar(cm);
        }
    }

    public void MoveRandomly(CharacterMovement character)
    {
        if (!character.disabled)
        {
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

    public void SetTooltip(string text)
    {
        tooltipText.text = text;
    }

    public void SaveReplay()
    {
        replay.SaveReplay(ActionQueue.Instance.actionCache);
    }
}
