using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeBar : MonoBehaviour
{
    private static TimeBar instance;
    public static TimeBar Instance { get { return instance; } }
    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        } else {
            instance = this;
        }
    }

    private float value = 0f;
    private float turnTime = 4f;
    public Slider slider;
    public Gradient gradient;
    public Image fill;

    // Divide by 3 because that's how long the turn takes
    void FixedUpdate()
    {
        value -= Time.fixedDeltaTime / turnTime;
        slider.value = value;
        fill.color = gradient.Evaluate(value);
    }

    public void Reset()
    {
        this.value = 1f;
    }

    public bool IsTurn()
    {
        return value > 0;
    }
}
