using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReplaysMenu : MonoBehaviour
{
    public void Back()
    {
        AudioManager.Instance.Play("Click");
        gameObject.SetActive(false);
    }

    void Start()
    {
        List<string> replays = SaveSystem.GetReplays();
        if (replays.Count == 0)
        {
            Debug.Log("No replays found");
        }

        foreach (string title in replays)
        {
            Debug.Log(title);
            Debug.Log(SaveSystem.LoadReplay(title).ToString());
        }
    }
}
