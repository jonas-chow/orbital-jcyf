using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    public GameObject gameEndUI;
    private CharacterMovement[] characters;
    private CharacterMovement[] friendly;
    private HealthBar hp;
    private static int numEnemy;
    private int numChar;
    private int numFriendly;
    private int currentChar = 0;
    [SerializeField]
    private ActionQueue actionQueue;
    [SerializeField]
    private TimeBar timeBar;
    
    private bool animationPhase = true;
    private bool animating = false;
    private bool isTurn = false;
    private bool readyForTurn = true;
    private float turnTimer = 3f;
    private float animationGap = .3f;

    // TODO: account for chars that died

    // Start is called before the first frame update
    void Start()
    {   
        characters = GetComponentsInChildren<CharacterMovement>();
        friendly = Array.FindAll(characters, c => c.isControllable);
        numChar = characters.Length;
        numFriendly = friendly.Length;
        numEnemy =  numChar - numFriendly;

        
        if (numFriendly > 0) {
            characters[currentChar].init();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (readyForTurn) {
            readyForTurn = false;
            StartCoroutine(StartTurn());
        }

        if (isTurn) {
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

        GameEnded();
    }
    
    // do I want this to be static idk?
    public static void RemoveEnemy() {
        numEnemy -= 1;
    }
    
    public void RemoveFriendly() {
        numFriendly -=1;
    }

    // currently only taking case where all enemies die
    public void GameEnded() {
        if (numEnemy <= 0) {
            gameEndUI.SetActive(true);
        }
    }
    

    void DeactivateCurrent()
    {
        characters[currentChar].Deactivate();
    }

    void ActivateCurrent()
    {
        characters[currentChar].Activate();
    }

    IEnumerator StartTurn()
    {
        Debug.Log("Turn start");
        ActivateCurrent();
        isTurn = true;
        animationPhase = true;
        timeBar.Reset(turnTimer);
        yield return new WaitForSeconds(turnTimer);
        isTurn = false;
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
        while (isTurn || actionQueue.hasActions()) {
            if (actionQueue.hasActions()) {
                actionQueue.ExecuteNext();
            }
            yield return new WaitForSeconds(animationGap);
        }
        actionQueue.ResetQueue();
        animationPhase = false;
        animating = false;
    }
}
