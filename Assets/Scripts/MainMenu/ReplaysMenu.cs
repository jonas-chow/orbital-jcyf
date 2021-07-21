using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReplaysMenu : MonoBehaviour
{
    public RectTransform content;
    public GameObject listingPrefab;
    public ReplayListing selected;
    private List<ReplayListing> listings = new List<ReplayListing>();

    public void Back()
    {
        AudioManager.Instance.Play("Click");
        if (selected != null)
        {
            selected.Deselect();
            selected = null;
        }
        gameObject.SetActive(false);
    }

    void Start()
    {
        List<string> replays = SaveSystem.GetReplays();
        int count = replays.Count;

        content.sizeDelta = new Vector2(content.sizeDelta.x, count * 100f);
        for (int i = 0; i < count; i++)
        {
            GameObject listing = Instantiate(listingPrefab, content.transform);
            ReplayListing replayListing = listing.GetComponent<ReplayListing>();
            replayListing.Init(replays[i], this, i);
            listings.Add(replayListing);
        }
    }

    public void Select(ReplayListing listing)
    {
        AudioManager.Instance.Play("Click");
        if (selected != null)
        {
            selected.Deselect();
        }
        selected = listing;
        PlayerPrefs.SetString("ReplayPath", selected.replayPath);
    }

    public void Delete()
    {
        AudioManager.Instance.Play("Click");
        if (selected == null) { return; }

        SaveSystem.DeleteReplay(selected.replayPath);
        int idx = listings.FindIndex(listing => listing.Equals(selected));
        GameObject.Destroy(listings[idx].gameObject);
        listings.RemoveAt(idx);
        for (int i = 0; i < listings.Count; i++)
        {
            listings[i].Move(i);
        }
        selected = null;
    }

    public void Play()
    {
        AudioManager.Instance.Play("Click");
        if (selected == null) { return; }

        SceneManager.LoadScene(1);
    }
}
