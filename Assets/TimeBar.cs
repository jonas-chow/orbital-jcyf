using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeBar : MonoBehaviour
{
    private float value = 1f;
    private float turnTime;
    public Slider slider;
    public Gradient gradient;
    public Image fill;

    // Divide by 3 because that's how long the turn takes
    void FixedUpdate()
    {
        value -= Time.fixedDeltaTime / 3;
        slider.value = value;
        fill.color = gradient.Evaluate(value);
    }

    public void Reset(float turnTime)
    {
        this.value = 1f;
        this.turnTime = turnTime;
    }
}
