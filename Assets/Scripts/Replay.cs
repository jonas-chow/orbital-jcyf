using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

// Note: you can tell if you started first or if enemy started first
// Since the turn end action happens at the start and end of your turn
// if the first action is a turn end, that means you started first
// in other words, if the enemy starts first by default, you can safely use the end turn as a swap trigger

public class Replay
{
    public string friendlyName;
    public string opponentName;
    // datetime.Now in ddMMyyyyHHmm format
    public string datetime;
    // actions transformed into JSON
    public string[] actions;

    public string[] friendlyChars;
    public string[] opponentChars;

    public Replay()
    {
        datetime = DateTime.Now.ToString("dd/MM/yy\nHH:mm:ss");

        friendlyName = PlayerPrefs.GetString("Username", "");
        
        if (EventHandler.Instance != null) {
            opponentName = EventHandler.Instance.enemyName;
        } else {
            opponentName = "unknown";
        }
    }

    public void SetFriendlies(string[] friendlyChars)
    {
        this.friendlyChars = friendlyChars;
    }

    public void SetEnemies(string[] opponentChars)
    {
        this.opponentChars = opponentChars;
    }

    public void SaveReplay(Queue<Action> actions)
    {
        int size = actions.Count;
        this.actions = new string[size];

        for (int i = 0; i < size; i++)
        {
            this.actions[i] = JsonUtility.ToJson(actions.Dequeue());
        }

        SaveSystem.SaveReplay(this);
    }

    public override string ToString()
    {
        string str = "";
        foreach (string action in actions)
        {
            str += action;
            str += " ";
        }
        return str;
    }

    public string GetNumericDatetime()
    {
        string[] split = datetime.Split('/', '\n', ':');
        string output = "";
        foreach (string substr in split)
        {
            output += substr;
        }
        return output;
    }
}
