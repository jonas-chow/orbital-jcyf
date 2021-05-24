using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private CharacterMovement[] characters;
    private int numChars;
    private int currentChar = 0;

    // TODO: account for chars that died

    // Start is called before the first frame update
    void Start()
    {
        characters = GetComponentsInChildren<CharacterMovement>();
        numChars = characters.Length;
        if (numChars > 0) {
            characters[currentChar].init();
            ActivateCurrent();
        }
    }

    // Update is called once per frame
    void Update()
    {
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

    void DeactivateCurrent()
    {
        characters[currentChar].Deactivate();
    }

    void ActivateCurrent()
    {
        characters[currentChar].Activate();
    }
}
