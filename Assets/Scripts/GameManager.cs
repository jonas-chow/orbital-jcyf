using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // TODO:
    // Fix the lobby bugs
    // Choose your prefabs in lobby
    // Customisable controls

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
        InstantiateSelf();
    }

    [SerializeField]
    private GameObject defeatUI, victoryUI, loadingUI, yourTurnUI, enemyTurnUI;

    [SerializeField]
    private GameObject Melee1, Ranged1, Mage1;
    private CharacterMovement[] enemies;
    private CharacterMovement[] friendly;
    private int numEnemy = 3;
    private int numFriendly = 3;
    private int currentChar = 0;
    private bool friendlyLoaded = false;
    private bool enemyLoaded = false;
    public bool enemyReady = false;
    
    private bool animationPhase = true;
    private bool animating = false;
    public bool readyForTurn = false;
    private float animationGap = .3f;

    public void InstantiateSelf()
    {
        // instantiate melee character
        GameObject melee = BuildChar(PlayerPrefs.GetString("Melee", "Melee1"), false);
        GridManager.Instance.MoveToAndInsert(
            melee,
            PlayerPrefs.GetInt("MeleeX", 0),
            PlayerPrefs.GetInt("MeleeY", 0));
        // instantiate ranged character
        GameObject ranged = BuildChar(PlayerPrefs.GetString("Ranged", "Ranged1"), false);
        GridManager.Instance.MoveToAndInsert(
            ranged,
            PlayerPrefs.GetInt("RangedX", 1),
            PlayerPrefs.GetInt("RangedY", 0));
        // instantiate mage character
        GameObject mage = BuildChar(PlayerPrefs.GetString("Mage", "Mage1"), false);
        GridManager.Instance.MoveToAndInsert(
            mage,
            PlayerPrefs.GetInt("MageX", 2),
            PlayerPrefs.GetInt("MageY", 0));

        // set those characterMovements into the friendly array
        friendly = new CharacterMovement[] {
            melee.GetComponent<CharacterMovement>(),
            ranged.GetComponent<CharacterMovement>(),
            mage.GetComponent<CharacterMovement>()};

        friendlyLoaded = true;
        CheckLoading();
        CheckBothReady();

        // For single player mode
        if (EventHandler.Instance == null) {
            InstantiateEnemies(
                "Melee1", 15, 15,
                "Ranged1", 14, 15,
                "Mage1", 13, 15);
        }
    }

    public void InstantiateEnemies(
        string meleeChar, int meleeX, int meleeY,
        string rangedChar, int rangedX, int rangedY,
        string mageChar, int mageX, int mageY)
    {
        // instantiate melee character
        GameObject melee = BuildChar(meleeChar, true);
        GridManager.Instance.MoveToAndInsert(melee, meleeX, meleeY);

        // instantiate ranged character
        GameObject ranged = BuildChar(rangedChar, true);
        GridManager.Instance.MoveToAndInsert(ranged, rangedX, rangedY);

        // instantiate mage character
        GameObject mage = BuildChar(mageChar, true);
        GridManager.Instance.MoveToAndInsert(mage, mageX, mageY);

        // set those characterMovements into the friendly array
        enemies = new CharacterMovement[] {
            melee.GetComponent<CharacterMovement>(),
            ranged.GetComponent<CharacterMovement>(),
            mage.GetComponent<CharacterMovement>()};

        enemyLoaded = true;
        CheckLoading();
        CheckBothReady();
    }

    private GameObject BuildChar(string prefabName, bool isEnemy)
    {
        GameObject obj = null;
        switch (prefabName)
        {
            case "Melee1":
                obj = Melee1;
                break;
            case "Ranged1":
                obj = Ranged1;
                break;
            case "Mage1":
                obj = Mage1;
                break;
            default:
                obj = Melee1;
                break;
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
                }

                if (Input.GetButtonDown("Previous Character")) {
                    DeactivateCurrent();
                    currentChar = (currentChar + numFriendly - 1) % numFriendly;
                    ActivateCurrent();
                }
            }

            if (animationPhase && !animating) {
                animating = true;
                StartCoroutine(AnimateActions());
            }
        } else {
            if (Input.GetKeyDown(KeyCode.Escape)) {
                SceneManager.LoadScene("Main Menu");
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
                friendly = Array.FindAll(friendly, character => !character.Equals(dead));
            } else {
                friendly = Array.FindAll(friendly, character => !character.Equals(dead));
                currentChar--;
            }
        } else {
            Lose();
        }
    }

    // currently only taking case where all enemies die
    private void Lose() {
        defeatUI.SetActive(true);
    }

    private void Win() {
        victoryUI.SetActive(true);
    }
    

    void DeactivateCurrent()
    {
        friendly[currentChar].Deactivate();
    }

    void ActivateCurrent()
    {
        friendly[currentChar].Activate();
    }

    IEnumerator StartTurn()
    {
        Debug.Log("Turn start");
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
    }

    public void EnemyTurn()
    {
        Debug.Log("Waiting for opponent turn to end");        
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
}
