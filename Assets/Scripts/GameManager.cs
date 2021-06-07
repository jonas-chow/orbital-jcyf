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

    // rmb to remove this
    void Start()
    {
        InstantiateSelf();
    }

    [SerializeField]
    private GameObject gameEndUI;

    [SerializeField]
    private GameObject Melee1, Ranged1, Mage1;
    private CharacterMovement[] enemies;
    private CharacterMovement[] friendly;
    private int numEnemy = 3;
    private int numFriendly = 3;
    private int currentChar = 0;
    
    private bool animationPhase = true;
    private bool animating = false;
    private bool readyForTurn = true;
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
    // TODO: make prefabs with both friendly and enemy
    // clean up the code
    // send events related to instantiation
    // flip the grid for the enemy stuff in event handler

    

/*
    // Start is called before the first frame update
    void Start()
    {   
        characters = GetComponentsInChildren<CharacterMovement>();
        friendly = Array.FindAll(characters, c => c.isFriendly);
        numChar = characters.Length;
        numFriendly = friendly.Length;
        numEnemy =  numChar - numFriendly;

        
        if (numFriendly > 0) {
            characters[currentChar].init();
        }
    }
*/

    // Update is called once per frame
    void Update()
    {
        if (numEnemy > 0) {
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
            GameEnded();
        }
    }
    
    public void RemoveEnemy() {
        numEnemy -= 1;
    }
    
    public void RemoveFriendly() {
        numFriendly -=1;
    }

    // currently only taking case where all enemies die
    private void GameEnded() {
        gameEndUI.SetActive(true);
        if (Input.anyKeyDown) {
            SceneManager.LoadScene("Main Menu");
        }
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

        // artificially wait for animations
        Debug.Log("Your turn in one second");
        yield return new WaitForSeconds(1f);
        readyForTurn = true;
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
}
