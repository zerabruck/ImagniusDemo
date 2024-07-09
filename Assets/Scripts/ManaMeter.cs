using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManaMeter : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    // public Image fill;
    public void SetMaxMana(float health)
    {
    slider.maxValue = health;
    slider.value = health;
    // fill.color = gradient.Evaluate (1f) ;
    }
    public void SetMana(float health)
    {
    slider.value = health;
    // fill.color = gradient.Evaluate(slider.normalizedValue);
    }
}