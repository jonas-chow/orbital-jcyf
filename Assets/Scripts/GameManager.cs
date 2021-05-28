using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private CharacterMovement[] characters;
    private int numChars;
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
        characters = Array.FindAll(characters, c => c.isControllable);

        numChars = characters.Length;
        if (numChars > 0) {
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
                currentChar = (currentChar + 1) % numChars;
                ActivateCurrent();
            }

            if (Input.GetButtonDown("Previous Character")) {
                DeactivateCurrent();
                currentChar = (currentChar + numChars - 1) % numChars;
                ActivateCurrent();
            }
        }

        if (animationPhase && !animating) {
            animating = true;
            StartCoroutine(AnimateActions());
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
