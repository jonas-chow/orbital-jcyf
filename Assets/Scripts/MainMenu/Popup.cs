using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Popup : MonoBehaviour
{
    private static Popup instance;

    [SerializeField]
    private TextMeshProUGUI popupText, notificationText;
    [SerializeField]
    private GameObject popup, notification;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        } else {
            instance = this;
        }
    }
    
    public static void StartPopup(string message)
    {
        instance.popupText.text = message;
        instance.popup.SetActive(true);
    }

    public static void StopPopup()
    {
        instance.popup.SetActive(false);
    } 

    public static void Notify(string message)
    {
        instance.notificationText.text = message;
        instance.notification.SetActive(true);
    }

    public void CloseNotification()
    {
        AudioManager.Instance.Play("Click");
        notification.SetActive(false);
    }
}
