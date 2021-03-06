using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ReplayListing : MonoBehaviour
{
    public Image[] friendlySprites = new Image[3];
    public Image[] opponentSprites = new Image[3];
    public TextMeshProUGUI p1Name;
    public TextMeshProUGUI p2Name;
    public TextMeshProUGUI datetime;
    public ReplaysMenu menu;
    public Replay replay;
    public string replayPath;
    public int index;
    public Image selection;

    public void Init(string replayPath, ReplaysMenu menu, int index)
    {
        Replay replay = SaveSystem.LoadReplay(replayPath);

        p1Name.text = replay.friendlyName;
        p1Name.color = replay.victory ? Color.green : Color.red;
        p2Name.text = replay.opponentName;
        p2Name.color = replay.victory ? Color.red : Color.green;

        datetime.text = replay.datetime;
        for (int i = 0; i < 3; i++)
        {
            friendlySprites[i].sprite = SpriteManager.Instance.GetSprite(replay.friendlyChars[i], true);
            opponentSprites[i].sprite = SpriteManager.Instance.GetSprite(replay.opponentChars[i], false);
        }

        this.replay = replay;
        this.replayPath = replayPath;
        this.menu = menu;
        this.index = index;
        Move(index);

    }

    public void SelectListing()
    {
        menu.Select(this);
        selection.color = new Color(0f, 0f, 0f, 0.5f);
    }

    public void Deselect()
    {
        selection.color = new Color(0f, 0f, 0f, 0f);
    }

    public void Move(int index)
    {
        transform.localPosition = Vector3.down * 100 * index;
    }
}
