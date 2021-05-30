using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class EndGame : MonoBehaviour
{
    public TMP_Text endGameMsg;
    private GameManager game;

    // Update is called once per frame
    void Update()
    {
      endGameMsg.text = "VICTORY!";
    }
}
